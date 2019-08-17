using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public Alignment Alignment { get; set; }



        #region NotMapped
        [NotMapped]
        public GameContext GameContext { get; set; }


        #endregion

        public Game()
        {
            GameContext = new GameContext();
            startTime = new DateTime();
            endTime = new DateTime();
        }

        public Game(GameContext context)
            :base()
        {
            if (context != null)
            {
                GameContext = context;
            }
        }
    }
}