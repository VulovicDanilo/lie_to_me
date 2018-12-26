using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="dd-mm-yyyy")]
        public DateTime StartTime { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd-mm-yyyy")]
        public DateTime EndTime { get; set; }
        public List<DeadPlayer> Graveyard { get; set; }
        public List<Player> Winners { get; set; }
        public Alignment Winner { get; set; }
        public GameMode GameMode { get; set; }
    }
}