using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class GameListing
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string GameMode { get; set; }
        public string PlayersCount { get; set; }
        public string MaxPlayers { get; set; }

        public static GameListing ToDTO(Game game, string username)
        {
            var dto = new GameListing()
            {
                Id = game.Id,
                Owner = username,
                GameMode = game.GameMode.ToString(),
                PlayersCount = game.Players.Count.ToString(),
                MaxPlayers = game.MAX_PLAYERS.ToString(),
            };
            return dto;
        }

        public override string ToString()
        {
            return "Owner: " + Owner + " Game Mode: " + GameMode
                + " Players: " + PlayersCount + "/" + MaxPlayers;
        }
    }
}
