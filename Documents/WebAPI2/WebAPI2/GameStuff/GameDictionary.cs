using DataLayer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.GameStuff
{
    public static class GameDictionary
    {
        private static ConcurrentDictionary<int, Game> Games { get; set; }
            = new ConcurrentDictionary<int, Game>();

        public static bool Add(Game game)
        {
            return Games.TryAdd(game.Id, game);
            
        }
        public static Game Get(int id)
        {
            Game game;
            Games.TryGetValue(id, out game);
            return game;
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
            if (game.Players.Count == 0)
            {
                Remove(gameId);
            }
        }
    }
}