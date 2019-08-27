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
        private string QueueName { get; set; }
        private ConnectionFactory connectionFactory;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer ContextConsumer = null;
        public GameWindow(Player player, string queueName)
        {
            InitializeComponent();
            QueueName = queueName;
            Player = player;
            lbxInfo.ItemsSource = Messages;
            this.Closing += CloseConnections;
            InitContextListener();
        }

        private void CloseConnections(object sender, CancelEventArgs args)
        {
            if (channel.IsOpen)
                channel.Close();
            if (connection.IsOpen)
                connection.Close();
            // TODO
            // HANDLE PLAYER DELETION IF LOBBY WAS IN LOBBY
        }

        private string InitConnection()
        {
            connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
            return channel.QueueDeclare().QueueName;
        }

        private void InitContextListener(string routingKey = "0")
        {
            string queue = InitConnection();
            if (routingKey != null)
            {
                channel.QueueUnbind(QueueName, "topic_logs", routingKey, null);
            }

            channel.QueueBind(queue: queue,
                              exchange: QueueName,
                              routingKey: routingKey);


            ContextConsumer = new EventingBasicConsumer(channel);
            ContextConsumer.Received += (model, ea) => ContextChange(model, ea);

            channel.BasicConsume(queue: queue,
                                 autoAck: true,
                                 consumer: ContextConsumer);

        }

        private void ContextChange(object model, BasicDeliverEventArgs args)
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body);
            var routingKey = args.RoutingKey;
            ClientContext newContext = JsonConvert.DeserializeObject<ClientContext>(message);
            Dispatcher.Invoke(() =>
            {
                var date = DateTime.Now;
                Messages.Add("Context updated: " + date.Hour + ":" + date.Minute + ":" + date.Second);
                
            });
        }
    }
}
