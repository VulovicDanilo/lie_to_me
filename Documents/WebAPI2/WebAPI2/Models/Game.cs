using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class Game
    {
        public int GameID { get; set; }
        public List<TownRole> TownRoles { get; set; }
        public List<String> ChatLog { get; set; }
        public List<Player> Players { get; set; }
        public GameState GameState { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<DeadPlayer> Graveyard { get; set; }
        public List<Player> Winners { get; set; }
        public Alignment Winner { get; set; }
        public GameMode GameMode { get; set; }
    }
}