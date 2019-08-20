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
        // public List<Player> Players { get; set; }
        public List<Player> Winners { get; set; }

        public GameContext()
        {
            Winners = new List<Player>();
        }

        public void NextState()
        {
            GameState = GameState.Next();
        }
    }
}