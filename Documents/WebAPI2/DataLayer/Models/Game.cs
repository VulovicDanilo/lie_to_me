using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Column(name: "StartTime")]
        public DateTime? StartTime { get; set; }
        [Column(name: "EndTime")]
        public DateTime? EndTime { get; set; }
        public Alignment? Winner { get; set; } 



        #region NotMapped
        [NotMapped]
        public GameContext GameContext { get; set; }
        [NotMapped]
        public Player Owner { get; set; }


        #endregion

        public Game()
        {
            GameContext = new GameContext();
        }

        public Game(GameContext context)
            :base()
        {
            if (context != null)
            {
                GameContext = context;
            }
        }
        public bool Full
        {
            get
            {
                return GameContext.Players.Count == GameContext.MAX_PLAYERS;
            }
        }
        public override string ToString()
        {
            return "Game mode: " + GameContext.GameMode + " | Players: "
                + GameContext.Players.Count() + "/" + GameContext.MAX_PLAYERS;
        }
    }
}