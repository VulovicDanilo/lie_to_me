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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private string MessageQueue { get; set; }
        private string DeadMessageQueue { get; set; }
        private string PrivateMessageQueue { get; set; }
        private ConnectionFactory connectionFactory;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer ContextConsumer = null;
        private string ContextConsumerTag = "";
        private EventingBasicConsumer LobbyInfoConsumer = null;
        private string LobbyInfoConsumerTag = "";
        private EventingBasicConsumer MessageConsumer = null;
        private string MessageConsumerTag { get; set; }
        private EventingBasicConsumer DeadMessageConsumer = null;
        private string DeadMessageConsumerTag { get; set; }
        private EventingBasicConsumer PrivateMessageConsumer = null;
        private string PrivateMessageConsumerTag { get; set; }

        private int VotingChoice { get; set; } = -1;
        private JudgementVote JudgementChoice { get; set; } = JudgementVote.Abstained;
        private ExecuteActionModel ActionChoice { get; set; }

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
            if (timerSeconds != 0)
            {
                timerSeconds--;
                var timespan = TimeSpan.FromSeconds(timerSeconds);
                this.lblTimer.Content = string.Format("{1:D2}m:{2:D2}s", timespan.Hours, timespan.Minutes, timespan.Seconds);
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
            string messageKey = ((int)MessageQueueChannel.ChatMessageAlive).ToString();
            string deadMessageKey = ((int)MessageQueueChannel.ChatMessageDead).ToString();
            string privateMessageKey = (MessageQueueChannel.PrivateChannelOffset + Player.Id).ToString();

            ContextQueue = GetQueue();
            LobbyQueue = GetQueue();
            MessageQueue = GetQueue();
            DeadMessageQueue = GetQueue();
            PrivateMessageQueue = GetQueue();

            if (channel != null)
            {
                channel.QueueUnbind(ContextQueue, ExchangeName, contextKey, null);
                channel.QueueUnbind(LobbyQueue, ExchangeName, lobbyInfoKey, null);
                channel.QueueUnbind(MessageQueue, ExchangeName, messageKey, null);
                channel.QueueUnbind(DeadMessageQueue, ExchangeName, deadMessageKey, null);
                channel.QueueUnbind(PrivateMessageQueue, ExchangeName, privateMessageKey, null);
            }

            channel.QueueBind(queue: ContextQueue,
                exchange: ExchangeName,
                routingKey: contextKey);

            channel.QueueBind(queue: LobbyQueue,
                exchange: ExchangeName,
                routingKey: lobbyInfoKey);

            channel.QueueBind(queue: MessageQueue,
                exchange: ExchangeName,
                routingKey: messageKey);

            channel.QueueBind(queue: DeadMessageQueue,
                exchange: ExchangeName,
                routingKey: deadMessageKey);

            channel.QueueBind(queue: PrivateMessageQueue,
                exchange: ExchangeName,
                routingKey: privateMessageKey);

            ContextConsumer = new EventingBasicConsumer(channel);
            ContextConsumer.Received += (model, ea) => ContextChange(model, ea);
            ContextConsumerTag = Consume(ContextQueue, ContextConsumer);

            LobbyInfoConsumer = new EventingBasicConsumer(channel);
            LobbyInfoConsumer.Received += (model, ea) => MessageArrive(model, ea);
            LobbyInfoConsumerTag = Consume(LobbyQueue, LobbyInfoConsumer);

            MessageConsumer = new EventingBasicConsumer(channel);
            MessageConsumer.Received += (model, ea) => MessageArrive(model, ea);
            MessageConsumerTag = Consume(MessageQueue, MessageConsumer);

            DeadMessageConsumer = new EventingBasicConsumer(channel);
            DeadMessageConsumer.Received += (model, ea) => MessageArrive(model, ea);

            PrivateMessageConsumer = new EventingBasicConsumer(channel);
            PrivateMessageConsumer.Received += (model, ea) => NightLog(model, ea);
            PrivateMessageConsumerTag = Consume(PrivateMessageQueue, PrivateMessageConsumer);
        }

        private string Consume(string queueName, EventingBasicConsumer consumer)
        {
            return channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);
        }

        private void StopConsume(string tag)
        {
            if (channel != null)
            {
                channel.BasicCancel(tag);
            }
        }

        private void MessageArrive(object model, BasicDeliverEventArgs args)
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body);
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                var date = DateTime.Now;
                AddMessage(message);
            }));
        }
        private void NightLog(object model, BasicDeliverEventArgs args)
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body);
            var routingKey = args.RoutingKey;
            List<String> logs = (List<String>)JsonConvert.DeserializeObject<List<String>>(message);
            Messages.Add("---------- logs ----------");
            foreach(var log in logs)
            {
                Messages.Add(log);
            }
            Messages.Add("---------- logs ----------");
            Refresh();
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

                    PlayerControls = new List<PlayerControl>(10);
                }
                else
                {
                    if (Context.GameState != newContext.GameState)
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
                            Player.FakeName = Context.Players.Find(player => player.PlayerId == Player.Id).FakeName;
                            Player.Number = Context.Players.Find(player => player.PlayerId == Player.Id).Number;

                            lblRoleName.Content = Player.FakeName + ": " + Player.Role.RoleName.ToString().ToLower();
                            txtRoleDescription.Text = "ability: " + Player.Role.Description.ToLower();
                            lblRoleGoal.Content = "goal: " + Player.Role.Goal;
                            UpdateUiRoleDistribution();
                            DrawPlayerControls();
                        }
                        else if (newContext.GameState == GameState.Discussion)
                        {
                            if (Player.RoleName == RoleName.Medium)
                            {
                                StopConsume(DeadMessageConsumerTag);
                            }
                            UpdateUiGame();
                        }
                        else if (newContext.GameState == GameState.Voting)
                        {
                            int i = 0;
                            foreach (var playerControl in PlayerControls)
                            {
                                if (Player.Role.Alignment == Alignment.Mafia && newContext.Mafia.Exists(x => x.Number == i))
                                {

                                }
                                else if (Player.Number == i)
                                {

                                }
                                else
                                {
                                    playerControl.EnableVoting();
                                }
                                i++;
                            }
                        }
                        else if (newContext.GameState == GameState.Defence)
                        {
                            foreach (var playerControl in PlayerControls)
                            {
                                playerControl.DisableButtons();
                            }
                        }
                        else if (newContext.GameState == GameState.Judgement)
                        {
                            if (!(Player.Number == newContext.Accused.Number))
                            {
                                foreach (var playerControl in PlayerControls)
                                {
                                    if (playerControl.Number == newContext.Accused.Number)
                                    {
                                        playerControl.EnableJudgement();
                                    }
                                }
                            }
                        }
                        else if (newContext.GameState == GameState.LastWord)
                        {
                            foreach (var playerControl in PlayerControls)
                            {
                                playerControl.DisableButtons();
                            }
                        }
                        else if (newContext.GameState == GameState.Night)
                        {
                            foreach (var playerControl in PlayerControls)
                            {
                                playerControl.DisableButtons();
                            }
                            if (Player.RoleName == RoleName.Medium)
                            {
                                Consume(DeadMessageQueue, DeadMessageConsumer);
                            }
                            if (Player.RoleName == RoleName.Veteran)
                            {
                                PlayerControls[Player.Number].EnableAction();
                            }
                            else
                            {
                                if (Player.Role.CanVisit)
                                {
                                    if (Player.Role.Alignment == Alignment.Mafia)
                                    {
                                        foreach (var playerControl in PlayerControls)
                                        {
                                            if (newContext.Mafia.Exists(x => x.Number == playerControl.Number))
                                            {

                                            }
                                            else
                                            {
                                                playerControl.EnableAction();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var playerControl in PlayerControls)
                                        {
                                            if (playerControl.Number != Player.Number)
                                            {
                                                playerControl.EnableAction();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Context = newContext; // for now

                    foreach(var dead in Context.DeadPlayers)
                    {
                        PlayerControls[dead.Number].Die();
                        PlayerControls[dead.Number].Background = Brushes.Red;
                    }
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
            double controlWidth = canvasGame.Width - 50;
            double controlHeight = canvasGame.Height / 10 - 1;
            foreach (var player in Context.Players)
            {
                var playerControl = new PlayerControl(player);
                playerControl.Height = controlHeight;
                playerControl.Width = controlWidth;
                playerControl.Margin = new Thickness(10, i * controlHeight, 0, 0);
                PlayerControls.Add(playerControl);

                i++;
            };
            
            foreach (var playerControl in PlayerControls)
            {
                playerControl.btnVote.Click += HandleVote;
                playerControl.btnInnocent.Click += HandleInnocent;
                playerControl.btnGuilty.Click += HandleGuilty;
                playerControl.btnAction.Click += HandleAction;
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
            if (Context.Players.Count >= 3)
            {
                GameService service = new GameService();
                service.StartNameSelectionPhase(Context.GameId);
            }
            else
            {
                AddMessage("not enough players. " + (3 - Context.Players.Count) + " more...");
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
            if (success)
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

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (txtChat.Text != "" && Context.GameState != GameState.Lobby &&
                Context.GameState != GameState.NameSelection)
            {
                string text = txtChat.Text;
                PlayerService service = new PlayerService();
                ChatMessage message = new ChatMessage()
                {
                    GameId = Context.GameId,
                    Content = text,
                    PlayerId = Player.Id,
                    Time = DateTime.Now,
                    GameState = Context.GameState,
                };
               service.SendChatMessage(message);
            }
            txtChat.Clear();
        }
        private void HandleVote(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var grid = button.Parent as Grid;
            var playerControl = grid.Parent as PlayerControl;

            int newVotingChoice = playerControl.Number;
            if (VotingChoice != newVotingChoice)
            {
                PlayerService service = new PlayerService();
                service.Vote(Context.GameId, Player.Number, newVotingChoice);
                VotingChoice = newVotingChoice;
            }
        }
        private void HandleAction(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var grid = button.Parent as Grid;
            var playerControl = grid.Parent as PlayerControl;

            int doActionOn = playerControl.Number;
            if (ActionChoice == null)
            {
                ActionChoice = new ExecuteActionModel(Player.Number, doActionOn, Context.GameId);
                PlayerService service = new PlayerService();
                service.DoAction(ActionChoice);
            }
            if (ActionChoice.To != doActionOn)
            {
                PlayerService service = new PlayerService();
                service.UndoAction(ActionChoice);
                ActionChoice = new ExecuteActionModel(Player.Number, doActionOn, Context.GameId);
                service.DoAction(ActionChoice);
            }
        }

        private void HandleGuilty(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var grid = button.Parent as Grid;
            var playerControl = grid.Parent as PlayerControl;

            if (JudgementVote.Guilty != JudgementChoice)
            {
                JudgementChoice = JudgementVote.Guilty;
                PlayerService service = new PlayerService();
                service.JudgementVote(Context.GameId, Player.Number, JudgementChoice);
            }
        }

        private void HandleInnocent(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var grid = button.Parent as Grid;
            var playerControl = grid.Parent as PlayerControl;

            if (JudgementVote.Innocent != JudgementChoice)
            {
                JudgementChoice = JudgementVote.Innocent;
                PlayerService service = new PlayerService();
                service.JudgementVote(Context.GameId, Player.Number, JudgementChoice);
            }
        }
    }
}
