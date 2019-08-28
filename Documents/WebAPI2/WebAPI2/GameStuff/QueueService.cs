using DataLayer.DTOs;
using DataLayer.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebAPI2.GameStuff
{
    public static class QueueService
    {
        public static void BroadcastContext(string exchangeName, MessageQueueChannel routingKey, Game game)
        {
            ClientContext context = ClientContext.ToContext(game);
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName,
                                        type: "topic");



                string sendString = JsonConvert.SerializeObject(context);
                var body = Encoding.UTF8.GetBytes(sendString);
                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: ((int)routingKey).ToString(),
                                     basicProperties: null,
                                     body: body);
            }
        }
        public static void BroadcastLobbyInfo(string exchangeName, MessageQueueChannel routingKey, string info)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName,
                                        type: "fanout");



                string sendString = info;
                var body = Encoding.UTF8.GetBytes(sendString);
                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: ((int)routingKey).ToString(),
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}