using DataLayer.Models;
using DataLayer.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;

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
            catch (Exception)
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
            foreach (var player in newGame.Players)
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

        public static bool SetName(int gameId, int playerId, string fakeName)
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
            int dur = game.Duration;


            QueueService.BroadcastContext(game.Id.ToString(), game);
            QueueService.BroadcastLobbyInfo(gameId.ToString(), "choose your name, you have " + dur + " seconds");

            game.Timer.Interval = dur * 1000;
            game.addElapsedEvent((sender, e) => NameSelectionEnded(sender, e, gameId));
            game.Timer.Start();
        }




        private static void NameSelectionEnded(object sender, EventArgs e, int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();
            game.ResetTimerEvents();

            game.AssignNamesAndRoles();

            using (UnitOfWork unit = new UnitOfWork())
            {
                foreach (var player in game.Players)
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
            game.ResetTimerEvents();

            game.GameState = GameState.RoleDistribution;

            game.addElapsedEvent((sender, e) => RoleDistributionEnded(sender, e, gameId));
            int dur = game.Duration;

            QueueService.BroadcastLobbyInfo(gameId.ToString(), "roles have been distributed");
            QueueService.BroadcastContext(game.Id.ToString(), game);
            game.Timer.Interval = 1000 * dur;
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
            var game = Get(gameId);
            game.GameState = GameState.Discussion;
            DiscussionPhase(gameId);

            QueueService.BroadcastContext(gameId.ToString(), game);

        }
        private static void DiscussionPhase(int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();
            game.ResetTimerEvents();

            game.Timer.Elapsed += (sender, e) => DiscussionPhaseEnded(sender, e, gameId);
            game.Timer.Interval = game.Duration * 1000;
            game.Timer.Start();

            QueueService.BroadcastLobbyInfo(gameId.ToString(), "day " + game.Day + " started");

        }

        private static void DiscussionPhaseEnded(object sender, EventArgs args, int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();

            game.ResolveDiscussion();

            QueueService.BroadcastContext(gameId.ToString(), game);


            VotingPhase(gameId);
        }
        private static void VotingPhase(int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();
            game.ResetTimerEvents();

            game.Timer.Elapsed += (sender, e) => VotingPhaseEnded(sender, e, gameId);
            game.Timer.Interval = game.Duration * 1000;
            game.Timer.Start();

            QueueService.BroadcastLobbyInfo(gameId.ToString(), "voting has started");

        }

        private static void VotingPhaseEnded(object sender, ElapsedEventArgs e, int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();

            game.ResolveVoting();

            QueueService.BroadcastContext(gameId.ToString(), game);

            if (game.GameState == GameState.Defence)
            {
                DefensePhase(gameId);
            }
            else
            {
                NightPhase(gameId);
            }
        }

        private static void DefensePhase(int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();
            game.ResetTimerEvents();

            game.Timer.Elapsed += (sender, e) => DefensePhaseEnded(sender, e, gameId);
            game.Timer.Interval = game.Duration * 1000;
            game.Timer.Start();

            QueueService.BroadcastLobbyInfo(gameId.ToString(), "defense phase started");

        }

        private static void DefensePhaseEnded(object sender, ElapsedEventArgs e, int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();

            game.ResolveDefence();

            JudgementPhase(gameId);
        }

        private static void JudgementPhase(int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();
            game.ResetTimerEvents();

            game.Timer.Elapsed += (sender, e) => JudgementPhaseEnded(sender, e, gameId);
            game.Timer.Interval = game.Duration * 1000;
            game.Timer.Start();

            QueueService.BroadcastLobbyInfo(gameId.ToString(), "judgement phase started");

        }

        private static void JudgementPhaseEnded(object sender, ElapsedEventArgs e, int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();

            game.ResolveJudgement();

            QueueService.BroadcastContext(gameId.ToString(), game);

            if (game.GameState == GameState.LastWord)
            {
                LastWordPhase(gameId);
            }
            else
            {
                NightPhase(gameId);
            }
        }

        private static void LastWordPhase(int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();
            game.ResetTimerEvents();

            game.Timer.Elapsed += (sender, e) => LastWordPhaseEnded(sender, e, gameId);
            game.Timer.Interval = game.Duration * 1000;
            game.Timer.Start();

            QueueService.BroadcastLobbyInfo(gameId.ToString(), "may your soul rest in peace");

        }

        private static void LastWordPhaseEnded(object sender, ElapsedEventArgs e, int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();

            game.ResolveLastWord();

            if (game.GameState != GameState.GameEnd)
            { 
                NightPhase(gameId);
            }
            else
            {
                EndGamePhase(gameId);
            }
        }

        private static void NightPhase(int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();
            game.ResetTimerEvents();

            game.Timer.Elapsed += (sender, e) => NightPhaseEnded(sender, e, gameId);
            game.Timer.Interval = game.Duration * 1000;
            game.Timer.Start();

            QueueService.BroadcastLobbyInfo(gameId.ToString(), "night " + game.Day + " started");

        }

        private static void NightPhaseEnded(object sender, ElapsedEventArgs e, int gameId)
        {
            var game = Get(gameId);
            game.Timer.Stop();

            game.ResolveNight();

            QueueService.BroadcastContext(gameId.ToString(), game);
            SendPrivateMessages(game);

            if (game.GameState != GameState.GameEnd)
            {
                
                DiscussionPhase(gameId);
            }
            else
            {
                EndGamePhase(gameId);
            }
        }
        private static void EndGamePhase(int gameId)
        {
            var game = Get(gameId);
            QueueService.BroadcastLobbyInfo(gameId.ToString(), "game ended");
            QueueService.BroadcastContext(gameId.ToString(), game);
        }
        private static void SendPrivateMessages(Game game)
        {
            string exchangeName = game.Id.ToString();
            foreach(var player in game.Players)
            {
                var logs = JsonConvert.SerializeObject(player.NightLog);
                QueueService.SendPrivateMessage(exchangeName, logs, player.Id);
            }
        }

    }
}