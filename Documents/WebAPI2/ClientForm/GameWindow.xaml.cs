using BusinessLayer;
using ClientForm.Controls;
using DataLayer.DTOs;
using DataLayer.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ClientForm
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private List<string> Messages { get; set; } = new List<string>();
        private ClientContext Context { get; set; }
        private Player Player { get; set; }
        private string ExchangeName { get; set; }
        private string ContextQueue { get; set; }
        private string LobbyQueue { get; set; }
        private ConnectionFactory connectionFactory;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer ContextConsumer = null;
        private string ContextConsumerTag = "";
        private EventingBasicConsumer LobbyInfoConsumer = null;
        private string LobbyInfoConsumerTag = "";

        private List<PlayerControl> PlayerControls { get; set; }

        public GameWindow(Player player, string exchangeName)
        {
            InitializeComponent();
            ExchangeName = exchangeName;
            Player = player;
            lbxInfo.ItemsSource = Messages;
            this.Closing += CloseStuff;
            InitContextListener();
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            timer.Tick += new EventHandler(TickHandler);
            timer.Start();

            this.lblState.Content = GameState.Lobby.ToString();

            canvasLobby.Visibility = Visibility.Visible;
            canvasName.Visibility = Visibility.Collapsed;
            canvasRoleNameDisplay.Visibility = Visibility.Collapsed;
            canvasGame.Visibility = Visibility.Collapsed;

            GameService gameService = new GameService();
            gameService.RequestContextBroadcast(exchangeName);
        }

        private int timerSeconds = 1;
        private DispatcherTimer timer;
        private void TickHandler(object sender, EventArgs e)
        {
            timer.Stop();
            if(timerSeconds != 0)
            {
                timerSeconds--;
                var timespan = TimeSpan.FromSeconds(timerSeconds);
                this.lblTimer.Content = string.Format("{1:D2}m:{2:D2}s",timespan.Hours,timespan.Minutes,timespan.Seconds);
            }
            timer.Start();
        }

        private void CloseStuff(object sender, CancelEventArgs args)
        {
            if (channel != null)
            {
                channel.BasicCancel(ContextConsumerTag);
                channel.BasicCancel(LobbyInfoConsumerTag);
            }
            if (channel.IsOpen)
            {
                channel.Close();
            }
            if (connection.IsOpen)
            {
                connection.Close();
            }

            if (Context != null)
            {
                if (Context.GameState == GameState.Lobby)
                {
                    PlayerService playerService = new PlayerService();
                    GameService gameService = new GameService();
                    if (Context.Players.Count > 1)
                    {
                        if (Context.OwnerId == this.Player.Id)
                        {
                            var newOwner = Context.Players.Where(x => x.PlayerId != Player.Id).FirstOrDefault();
                            gameService.SetGameOwner(Context.GameId, newOwner.PlayerId);
                        }
                        playerService.DeletePlayer(Player.Id, Context.GameId);
                    }
                    else
                    {
                        playerService.DeletePlayer(Player.Id, Context.GameId);
                        gameService.DeleteGame(Context.GameId);
                    }
                }
                else
                {
                    // kill player ; publish that the player died
                    Player.Alive = false;
                    PlayerService playerService = new PlayerService();
                    playerService.UpdatePlayer(Player, Context.GameId);
                }
            }
        }

        private string GetQueue()
        {
            connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: ExchangeName, type: "topic");
            return channel.QueueDeclare(exclusive: false).QueueName;
        }

        private void InitContextListener()
        {
            string contextKey = ((int)MessageQueueChannel.ContextBroadcast).ToString();
            string lobbyInfoKey = ((int)MessageQueueChannel.LobbyInfo).ToString();
            ContextQueue = GetQueue();
            LobbyQueue = GetQueue();
            if (channel != null)
            {
                channel.QueueUnbind(ContextQueue, ExchangeName, contextKey, null);
                channel.QueueUnbind(LobbyQueue, ExchangeName, lobbyInfoKey, null);
            }

            channel.QueueBind(queue: ContextQueue, 
                exchange: ExchangeName, 
                routingKey: contextKey);

            channel.QueueBind(queue: LobbyQueue, 
                exchange: ExchangeName, 
                routingKey: lobbyInfoKey);

            ContextConsumer = new EventingBasicConsumer(channel);
            ContextConsumer.Received += (model, ea) => ContextChange(model, ea);
            LobbyInfoConsumer = new EventingBasicConsumer(channel);
            LobbyInfoConsumer.Received += (model, ea) => LobbyInfoArrived(model, ea);

            ContextConsumerTag = channel.BasicConsume(queue: ContextQueue, 
                autoAck: true,
                consumer: ContextConsumer);

            LobbyInfoConsumerTag = channel.BasicConsume(queue: LobbyQueue,
                autoAck: true,
                consumer: LobbyInfoConsumer);

        }

        private void LobbyInfoArrived(object model, BasicDeliverEventArgs args)
        {
            // TODO
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body);
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                var date = DateTime.Now;
                AddMessage(message);
            }));
        }

        private void ContextChange(object model, BasicDeliverEventArgs args)
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body);
            var routingKey = args.RoutingKey;
            ClientContext newContext = JsonConvert.DeserializeObject<ClientContext>(message);
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                var date = DateTime.Now;
                if (Context == null)
                {
                    Context = newContext;
                    this.Visibility = Visibility.Visible;

                    PlayerControls = new List<PlayerControl>(Context.MaxPlayers);
                }
                else
                {
                    if(Context.GameState != newContext.GameState)
                    {
                        this.lblState.Content = newContext.GameState.ToString();
                        this.timerSeconds = newContext.Duration;
                        if (newContext.GameState == GameState.NameSelection)
                        {
                            UpdateUiNameSelection();
                        }
                        else if (newContext.GameState == GameState.RoleDistribution)
                        {
                            PlayerService service = new PlayerService();
                            Player.Role = service.RequestStrategy(Context.GameId, Player.Id);
                            lblRoleName.Content = "you are " + Player.Role.RoleName;
                            lblRoleDescription.Content = Player.Role.Description;
                            lblRoleGoal.Content = "Goal: " + Player.Role.Goal;
                            UpdateUiRoleDistribution();
                        }
                        else if (newContext.GameState == GameState.Discussion)
                        {
                            DrawPlayerControls();
                            UpdateUiGame();
                        }
                    }
                    Context = newContext; // for now
                }
                if (Context.OwnerId == this.Player.Id)
                {
                    this.btnStart.Visibility = Visibility.Visible;
                }
                else
                {
                    this.btnStart.Visibility = Visibility.Collapsed;
                }
                AddMessage("context updated");
                Refresh();
            }));
        }

        private void DrawPlayerControls()
        {
            int i = 0;
            foreach(var player in Context.Players)
            {
                var playerControl = new PlayerControl(player);
                playerControl.Margin = new Thickness(0, i * 30, 0, 0);
                PlayerControls.Add(playerControl);


                i++;
            }
            foreach(var playerControl in PlayerControls)
            {
                canvasGame.Children.Add(playerControl);
            }
        }

        private void Refresh()
        {
            lbxInfo.Items.Refresh();
            lblPlayerCount.Content = "player count: " + Context.Players.Count;
            lbxInfo.SelectedIndex = lbxInfo.Items.Count - 1;
            lbxInfo.ScrollIntoView(lbxInfo.SelectedItem);
            lbxInfo.SelectedIndex = -1;
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (Context.Players.Count < Context.MaxPlayers)
            {
                GameService service = new GameService();
                service.StartNameSelectionPhase(Context.GameId);
            }
            else
            {
                AddMessage("not enough players. " + (Context.MaxPlayers - Context.Players.Count) + " more...");
            }
        }
        private void AddMessage(string info)
        {
            var date = DateTime.Now;
            Messages.Add(date.ToString("HH:mm:ss") + ": " + info);
            Refresh();
        }

        private void UpdateUiNameSelection()
        {
            canvasLobby.Visibility = Visibility.Collapsed;
            canvasName.Visibility = Visibility.Visible;
        }

        private void UpdateUiRoleDistribution()
        {
            canvasName.Visibility = Visibility.Collapsed;
            canvasRoleNameDisplay.Visibility = Visibility.Visible;
        }

        private void UpdateUiGame()
        {
            canvasRoleNameDisplay.Visibility = Visibility.Collapsed;
            canvasGame.Visibility = Visibility.Visible;
        }

        private void BtnNameSubmit_Click(object sender, RoutedEventArgs e)
        {
            string fakename = txtFakeName.Text;
            PlayerService service = new PlayerService();
            bool success = service.AddFakeName(this.Player.Id, fakename, Context.GameId);
            if(success)
            {
                btnNameSubmit.IsEnabled = false;
                lblFakeNameStatus.Content = "fake name added";
                lblFakeNameStatus.Foreground = Brushes.LightGreen;
            }
            else
            {
                lblFakeNameStatus.Content = "name already taken";
            }
        }
    }
}
