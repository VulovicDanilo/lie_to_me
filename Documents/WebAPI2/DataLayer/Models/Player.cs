using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int User_Id { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("Game")]
        public int Game_Id { get; set; }
        public virtual Game Game { get; set; }
        public RoleName? RoleName { get; set; }

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
        public bool Alive { get; set; } = true;
        [NotMapped]
        public bool Winner { get; set; } = false;
        [NotMapped]
        public Role Role { get; set; }

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

        public void SetRole(Role role)
        {
            Role = role;
            RoleName = role.RoleName;
        }
    }
}