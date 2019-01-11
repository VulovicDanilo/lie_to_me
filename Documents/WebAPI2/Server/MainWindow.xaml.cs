using Microsoft.Win32;
using Newtonsoft.Json;
using RabbitMQ.Client;
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

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Cimey cimey;
        public MainWindow()
        {
            InitializeComponent();
            cimey = new Cimey() { Name = "Dacha Pogacha", Age = 22, Avg = 9.17 };
            rbtn_Day.IsChecked = true;
        }

        private void Btn_send_Click(object sender, RoutedEventArgs e)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs",
                                        type: "topic");

                var routingKey = "1";

                string send_string = "";

                foreach (object grid in LogicalTreeHelper.GetChildren(this))
                {
                    foreach (object control in LogicalTreeHelper.GetChildren((Grid)grid))
                    {
                        if (control is RadioButton)
                        {
                            RadioButton rb = control as RadioButton;
                            if (rb.IsChecked == true)
                            {
                                send_string += (control as RadioButton).Content.ToString() + " ";
                                if (rb.Name == "rbtn_Inform")
                                    send_string += tbx_Information.Text + " ";
                            }
                        }
                    }
                }
                var body = Encoding.UTF8.GetBytes(send_string);
                channel.BasicPublish(exchange: "topic_logs",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
            }

        }

        private void BtnSendCimey_Click(object sender, RoutedEventArgs e)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs",
                                        type: "topic");

                var routingKey = "2";

                string sendString = "";

                cimey = new Cimey() { Name = tbxName.Text, Age = int.Parse(tbxAge.Text), Avg = double.Parse(tbxAvg.Text) };
                sendString = JsonConvert.SerializeObject(cimey);

                labJson.Content = sendString;

                var body = Encoding.UTF8.GetBytes(sendString);
                channel.BasicPublish(exchange: "topic_logs",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
            }

        }
    }
}
