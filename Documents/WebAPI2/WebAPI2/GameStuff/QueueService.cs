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
        public static void BroadcastContext(string exchangeName, Game game)
        {
            MessageQueueChannel routingKey = MessageQueueChannel.ContextBroadcast;
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
        public static void BroadcastLobbyInfo(string exchangeName, string info)
        {
            MessageQueueChannel routingKey = MessageQueueChannel.LobbyInfo;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName,
                                        type: "topic");



                string sendString = info;
                var body = Encoding.UTF8.GetBytes(sendString);
                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: ((int)routingKey).ToString(),
                                     basicProperties: null,
                                     body: body);
            }
        }

        public static void BroadcastMessage(string exchangeName, string content, int playerId)
        {

            int gameId = Int32.Parse(exchangeName);
            Game game = GameDictionary.Get(gameId);
            var player = game.Players.Find(x => x.Id == playerId);

            if (    (game.GameState == GameState.Discussion)
                ||  (game.GameState == GameState.Night && player.Role.Alignment == Alignment.Mafia)
                ||  (game.Accused != null && game.Accused.Id == playerId && (game.GameState == GameState.Defence || game.GameState == GameState.LastWord))
                )
            {
                MessageQueueChannel routingKey = MessageQueueChannel.ChatMessageAlive;
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName,
                                            type: "topic");
                    var body = Encoding.UTF8.GetBytes(content);
                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: ((int)routingKey).ToString(),
                                         basicProperties: null,
                                         body: body);
                }
            }
        }

        public static void BroadcastMessageDead(string exchangeName, string info)
        {
            MessageQueueChannel routingKey = MessageQueueChannel.ChatMessageDead;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName,
                                        type: "topic");



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