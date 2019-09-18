using DataLayer.Models;
using DataLayer.Repositories;
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
            if (game.Players.Count == 1)
            {
                game.Owner = newGame.Owner;
            }
            return true;
        }

        public static void Remove(int id)
        {
            Games[id] = null;
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

        public static bool SetName(int gameId,int playerId, string fakeName)
        {
            var game = Get(gameId);
            var player = game.Players.Where(x => x.Id == playerId).FirstOrDefault();

            var playersWithNames = game.Players.Where(x => x.FakeName == fakeName);

            if (playersWithNames.Count() == 0)
            {
                player.FakeName = fakeName;
                return true;
            }
            else
            {
                return false;
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

        public static void NameSelectionStarted(int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();
            game.GameState = GameState.NameSelection;
            game.Timer.Tick += (sender, e) => NameSelectionEnded(sender, e, gameId);
            int dur = game.Duration;

            QueueService.BroadcastLobbyInfo(gameId.ToString(), "choose your name, you have " + dur + " seconds");
            game.Timer.Interval = new TimeSpan(0, 0, dur);
            game.Timer.Start();
        }

        
        private static void NameSelectionEnded(object sender, EventArgs e,int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();

            game.AssignNamesAndRoles();

            using (UnitOfWork unit = new UnitOfWork())
            {
                foreach(var player in game.Players)
                {
                    unit.PlayerRepository.Update(player);
                }
                unit.Save();
            }

            QueueService.BroadcastContext(game.Id.ToString(), game);

            RoleDistributionStarted(game.Id);
        }

        private static void RoleDistributionStarted(int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();
            game.GameState = GameState.RoleDistribution;
            game.Timer.Tick += (sender, e) => RoleDistributionEnded(sender, e, gameId);
            int dur = game.Duration;

            QueueService.BroadcastLobbyInfo(gameId.ToString(), "roles have been distributed");
            game.Timer.Interval = new TimeSpan(0, 0, dur);
            game.Timer.Start();
        }

        private static void RoleDistributionEnded(object sender, EventArgs e, int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();

            StartGame(gameId);
        }

        private static void StartGame(int gameId)
        {
            QueueService.BroadcastLobbyInfo(gameId.ToString(), "game started");

            DayPhaseStarted(gameId);
        }

        private static void DayPhaseStarted(int gameId)
        {
            var game = Get(gameId);
            game.Day++;

            QueueService.BroadcastLobbyInfo(gameId.ToString(), "day " + game.Day + " started...");

        }
    }
}