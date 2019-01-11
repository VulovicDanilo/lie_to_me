using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> messages = new List<string>();
        private ConnectionFactory connectionFactory;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer consumer = null;
        private string routingKey;
        private string queue;
        public MainWindow()
        {
            InitializeComponent();
            queue = InitConnection();
            cbx.Items.Add("Channel 0");
            cbx.Items.Add("Channel 1");
            cbx.Items.Add("Channel 2");
            cbx.SelectedIndex = 0;
            InitListener(cbx.SelectedIndex.ToString(), queue);
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs args)
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body);
            var routingKey = args.RoutingKey;
            Dispatcher.Invoke(() =>
            {
                string newMessage = DateTime.Now.ToLongTimeString() + ": " + message;
                messages.Add(newMessage);
                logs.Items.Add(newMessage);
            });

        }

        private string InitConnection()
        {
            connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
            return channel.QueueDeclare().QueueName;
        }

        private void InitListener(String route, string queueName)
        {
            if (routingKey != null)
            {
                channel.QueueUnbind(queueName, "topic_logs", routingKey, null);
            }
            routingKey = route;

            channel.QueueBind(queue: queueName,
                              exchange: "topic_logs",
                              routingKey: routingKey);


            consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) => Consumer_Received(model, ea);

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (channel.IsOpen)
                channel.Close();
            if (connection.IsOpen)
                connection.Close();
        }

        private void BtnRoute_Click(object sender, RoutedEventArgs e)
        {
            InitListener(cbx.SelectedIndex.ToString(), queue);
        }

        private void Cbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string next_channel = cbx.SelectedIndex.ToString();
            Dispatcher.Invoke(() =>
            {
                if (next_channel != routingKey && routingKey != null)
                {
                    logs.Items.Add("***Changing from Channel " + routingKey + " to Channel " + next_channel + "***");
                }
            });
            InitListener(next_channel, queue);
        }

        private void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            logs.Items.Clear();
        }
    }
}
