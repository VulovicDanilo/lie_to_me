using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class Game
    {
        public int GameID { get; set; }
        //public List<Role> Roles { get; set; }
        public List<String> ChatLog { get; set; }
        public List<Player> Players { get; set; }
    }
}