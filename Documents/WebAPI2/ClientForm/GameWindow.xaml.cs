using BusinessLayer;
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

namespace ClientForm
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private List<string> Messages { get; set; } = new List<string>();
        private ClientContext context { get; set; }
        private Player Player { get; set; }
        private string ExchangeName { get; set; }
        private string QueueName { get; set; }
        private ConnectionFactory connectionFactory;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer ContextConsumer = null;
        private EventingBasicConsumer LobbyInfoConsumer = null;
        public GameWindow(Player player, string exchangeName)
        {
            InitializeComponent();
            ExchangeName = exchangeName;
            Player = player;
            lbxInfo.ItemsSource = Messages;
            this.Closing += CloseStuff;
            InitContextListener();

            this.Visibility = Visibility.Collapsed;
            GameService gameService = new GameService();
            gameService.RequestContextBroadcast(exchangeName);
        }

        private void CloseStuff(object sender, CancelEventArgs args)
        {
            if (channel.IsOpen)
                channel.Close();
            if (connection.IsOpen)
                connection.Close();

            if (context != null)
            {
                if (context.GameState == GameState.Lobby)
                {
                    PlayerService playerService = new PlayerService();
                    playerService.DeletePlayer(Player.Id, context.GameId);
                    if (context.Players.Count == 1)
                    {
                        GameService gameService = new GameService();
                        gameService.DeleteGame(context.GameId);
                    }
                }
                else
                {
                    // kill player ; publish that the player died
                    Player.Alive = false;
                    PlayerService playerService = new PlayerService();
                    playerService.UpdatePlayer(Player, context.GameId);
                }
            }
        }

        private string InitConnection()
        {
            connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: ExchangeName, type: "topic");
            return channel.QueueDeclare().QueueName;
        }

        private void InitContextListener()
        {
            string contextKey = ((int)MessageQueueChannel.ContextBroadcast).ToString();
            string lobbyInfoKey = ((int)MessageQueueChannel.LobbyInfo).ToString();
            QueueName = InitConnection();
            if (channel != null)
            {
                channel.QueueUnbind(QueueName, ExchangeName, contextKey, null);
                channel.QueueUnbind(QueueName, ExchangeName, lobbyInfoKey, null);
            }

            channel.QueueBind(queue: QueueName, 
                exchange: ExchangeName, 
                routingKey: contextKey);
            channel.QueueBind(queue: QueueName, 
                exchange: ExchangeName, 
                routingKey: lobbyInfoKey);

            ContextConsumer = new EventingBasicConsumer(channel);
            ContextConsumer.Received += (model, ea) => ContextChange(model, ea);
            LobbyInfoConsumer = new EventingBasicConsumer(channel);
            LobbyInfoConsumer.Received += (model, ea) => LobbyInfoArrived(model, ea);

            channel.BasicConsume(queue: QueueName, 
                autoAck: true,
                consumer: ContextConsumer);

            channel.BasicConsume(queue: QueueName,
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
                Messages.Add(date.ToString("HH:mm:ss") + ": " + message);
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
                Messages.Add(date.ToString("HH:mm:ss") + ": context updated");
                if (context == null)
                {
                    context = newContext;
                    Refresh();
                    this.Visibility = Visibility.Visible;
                }
                else
                {
                    // TODO AF
                    // COMPARE THE DIFF AND PRINT THE DIFF TO lbxInfo
                    context = newContext; // for now
                    Refresh();
                }
                if (context.OwnerId == this.Player.Id)
                {
                    this.btnStart.Visibility = Visibility.Visible;
                }
                else
                {
                    this.btnStart.Visibility = Visibility.Collapsed;
                }
            }));
        }

        private void Refresh()
        {
            lbxInfo.Items.Refresh();
            lblPlayerCount.Content = "player count: " + context.Players.Count;
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
