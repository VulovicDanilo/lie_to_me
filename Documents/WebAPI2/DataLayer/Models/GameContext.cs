using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Extensions;

namespace DataLayer.Models
{
    public class GameContext
    {
        public readonly int MAX_PLAYERS = 10;
        public GameState GameState { get; set; }
        public GameMode GameMode { get; set; }
        public List<Player> Players { get; set; }
        public List<Player> Winners { get; set; }

        public GameContext()
        {
            Players = new List<Player>(MAX_PLAYERS);
            Winners = new List<Player>();
        }

        public bool AddPlayer(Player player)
        {
            if (Players.Count < MAX_PLAYERS)
            {
                Players.Add(player);
                return true;
            }
            else
            {
                // soba puna
                return false; 
            }
        }

        public bool RemovePlayer(Player player)
        {
            return Players.RemoveAll(x => x.Id == player.Id) > 0;
        }

        public void NextState()
        {
            GameState = GameState.Next();
        }
    }
}