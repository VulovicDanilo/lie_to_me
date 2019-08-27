using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class ClientContext
    {
        public readonly int MAX_PLAYERS = 10;
        public GameState GameState { get; set; }
        public GameMode GameMode { get; set; }
        public List<InGamePlayer> Players { get; set; }
        public List<DeadPlayer> DeadPlayers { get; set; }
        public List<InGamePlayer> Winners { get; set; }
        public int GameId { get; set; }

        public ClientContext(Game game)
        {
            GameState = game.GameState;
            GameMode = game.GameMode;
            GameId = game.Id;

            Winners = new List<InGamePlayer>();
            Players = new List<InGamePlayer>();
            DeadPlayers = new List<DeadPlayer>();

            foreach(var player in game.Players)
            {
                if (player.Alive == false)
                {
                    DeadPlayers.Add(DeadPlayer.ToDTO(player));
                }
                else
                {
                    Players.Add(InGamePlayer.ToDTO(player));
                }
            }
        }

        public static ClientContext ToContext(Game game)
        {
            return new ClientContext(game);
        }
    }
}
