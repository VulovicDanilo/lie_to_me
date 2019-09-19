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
        public int MaxPlayers { get; set; }
        public GameState GameState { get; set; }
        public GameMode GameMode { get; set; }
        public List<InGamePlayer> Players { get; set; }
        public List<DeadPlayer> DeadPlayers { get; set; }
        public List<InGamePlayer> Winners { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public int GameId { get; set; }
        public int Duration { get; set; }
        public List<InGamePlayer> Mafia { get; set; }

        public ClientContext()
        {
            Players = new List<InGamePlayer>();
            DeadPlayers = new List<DeadPlayer>();
            Winners = new List<InGamePlayer>();
            Mafia = new List<InGamePlayer>();
        }

        public ClientContext(Game game)
        {
            GameState = game.GameState;
            GameMode = game.GameMode;
            GameId = game.Id;
            OwnerId = game.Owner.Id;
            OwnerName = game.Owner.User.UserName;
            Duration = game.Duration;
            MaxPlayers = game.MAX_PLAYERS;

            Winners = new List<InGamePlayer>();
            Players = new List<InGamePlayer>();
            DeadPlayers = new List<DeadPlayer>();
            Mafia = new List<InGamePlayer>();

            foreach (var player in game.Players)
            {
                if (player.Alive == false)
                {
                    DeadPlayers.Add(DeadPlayer.ToDTO(player));
                }
                else
                {
                    Players.Add(InGamePlayer.ToDTO(player));
                }
                if(player.Role.Alignment == Alignment.Mafia)
                {
                    Mafia.Add(InGamePlayer.ToDTO(player));
                }
            }
        }

        public static ClientContext ToContext(Game game)
        {
            return new ClientContext(game);
        }
    }
}
