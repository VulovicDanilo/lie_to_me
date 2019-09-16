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
        private int counter = 0;
        public GameWindow(Player player, string exchangeName)
        {
            InitializeComponent();
            ExchangeName = exchangeName;
            Player = player;
            lbxInfo.ItemsSource = Messages;
            this.Closing += CloseStuff;
            InitContextListener(((int)MessageQueueChannel.ContextBroadcast).ToString());

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
                    playerService.DeletePlayer(Player);
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

        private void InitContextListener(string channelKey)
        {
            QueueName = InitConnection();
            if (channel != null)
            {
                channel.QueueUnbind(QueueName, ExchangeName, channelKey, null);
            }

            channel.QueueBind(queue: QueueName,
                              exchange: ExchangeName,
                              routingKey: channelKey);


            ContextConsumer = new EventingBasicConsumer(channel);
            ContextConsumer.Received += (model, ea) => ContextChange(model, ea);

            channel.BasicConsume(queue: QueueName,
                                 autoAck: true,
                                 consumer: ContextConsumer);

        }

        private void ContextChange(object model, BasicDeliverEventArgs args)
        {
            counter++;
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body);
            var routingKey = args.RoutingKey;
            ClientContext newContext = JsonConvert.DeserializeObject<ClientContext>(message);
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                var date = DateTime.Now;
                Messages.Add("Context updated: " + date.ToString("HH:mm:ss"));

                if (context == null)
                {
                    context = newContext;
                    Refresh();
                    this.Visibility = Visibility.Visible;
                    this.Title = context.OwnerName + "'s game";
                }
                else
                {
                    // TODO AF
                    // COMPARE THE DIFF AND PRINT THE DIFF TO lbxInfo
                    context = newContext; // for now
                    Refresh();
                }
            }));
        }

        private void Refresh()
        {
            lbxInfo.Items.Refresh();
            lblPlayerCount.Content = "Player Count: " + context.Players.Count;
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
