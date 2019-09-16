using DataLayer.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.GameStuff
{
    internal static class GameDictionary
    {
        private static IConnection Connection { get; set; }
        private static ConcurrentDictionary<int, Game> Games { get; set; }

        static GameDictionary()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            Connection = factory.CreateConnection();
            Games = new ConcurrentDictionary<int, Game>();
        }

        public static bool Add(Game game)
        {
            try
            {
                Games.TryAdd(game.Id, game);
                game.Channel = Connection.CreateModel();
                game.Channel.ExchangeDeclare(exchange: game.Id.ToString(),
                                            type: "topic");
                return true;
            }
            catch(Exception)
            {
                return false;
            }

        }
        public static Game Get(int id)
        {
            Game game;
            Games.TryGetValue(id, out game);
            return game;
        }
        public static bool AddPlayer(Game newGame)
        {
            var game = Get(newGame.Id);
            foreach(var player in newGame.Players)
            {
                if (!game.Players.Exists(x => x.Id == player.Id))
                {
                    game.Players.Add(player);
                }
            }
            game.Owner = newGame.Owner;
            return true;
        }

        public static void Remove(int id)
        {
            Games.TryRemove(id, out _);
        }

        public static void AddPlayer(int gameId, Player player)
        {
            var game = Get(gameId);
            game.Players.Add(player);
        }
        public static void RemovePlayer(int gameId, int playerId)
        {
            var game = Get(gameId);
            var player = game.Players.Where(x => x.Id == playerId).FirstOrDefault();
            if (player != null)
            {
                game.Players.Remove(player);
            }
        }

        public static void UpdatePlayer(int gameId, Player player)
        {
            var game = Get(gameId);
            var toUpdate = game.Players.Where(x => x.Id == player.Id).FirstOrDefault();
            if (toUpdate != null)
            {
                toUpdate.Alive = player.Alive;
                toUpdate.RoleName = player.RoleName;
            }
        }


        public static void SetOwner(Game game)
        {
            var toUpdate = Get(game.Id);
            toUpdate.Owner = game.Owner;
        }
    }
}