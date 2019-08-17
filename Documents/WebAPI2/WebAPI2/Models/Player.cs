using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual Game Game { get; set; }

        #region NotMapped
        [NotMapped]
        public List<ChatMessage> ChatLog { get; set; }
        [NotMapped]
        public int Number { get; set; }
        [NotMapped]
        public string FakeName { get; set; }
        [NotMapped]
        public string LastWill { get; set; }
        [NotMapped]
        public bool Alive { get; set; }
        [NotMapped]
        public bool Winner { get; set; } = false;

        #endregion

        public Player()
        {
            ChatLog = new List<ChatMessage>();
            Alive = true;
            Winner = false;
            LastWill = String.Empty;
        }

        public Player(User user)
            : base()
        {
            User = user;
        }
    }
}