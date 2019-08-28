using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class GameListingDTO
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string GameMode { get; set; }
        public string PlayersCount { get; set; }
        public string MaxPlayers { get; set; }

        public static GameListingDTO ToDTO(Game game)
        {
            var dto = new GameListingDTO()
            {
                Id = game.Id,
                Owner = game.Owner.User.UserName,
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
