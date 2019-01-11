using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
            InitListener("0");
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Works");
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs args)
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body);
            var routingKey = args.RoutingKey;
            messages.Add(DateTime.Now.ToShortTimeString() + " -> " + message);
            Dispatcher.Invoke(() =>
            {
                logs.Items.Clear();
                foreach(var m in messages)
                {
                    logs.Items.Add(m);
                }
            });
        }

        private void InitListener(String route)
        {
            connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
            var queueName = channel.QueueDeclare().QueueName;


            channel.QueueBind(queue: queueName,
                              exchange: "topic_logs",
                              routingKey: route);


            consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) => Consumer_Received(model, ea);
            //{
            //    var body = ea.Body;
            //    var message = Encoding.UTF8.GetString(body);
            //    var routingKey = ea.RoutingKey;
            //    messages.Add(DateTime.Now.ToShortTimeString() + " -> " + message);
            //    logs.Items.Add(message);
            //    logs.Items.Refresh();

            //};
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

            InitListener(txtRoute.Text);
        }
    }
}
