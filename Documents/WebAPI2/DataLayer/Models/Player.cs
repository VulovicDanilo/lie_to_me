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
        public RoleName? RoleName { get; set; }

        #region NotMapped
        [NotMapped]
        public List<string> NightLog { get; set; }
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
        public RoleStrategy Role { get; set; }
        [NotMapped]
        public int GameId { get; set; }
        [NotMapped]
        public bool Suspicious { get; set; } = false;
        [NotMapped]
        public bool Done { get; set; } = false;
        [NotMapped]
        public List<Player> Visitors { get; set; } = new List<Player>();

        #endregion

        public Player()
        {
            NightLog = new List<string>();
            Alive = true;
            Winner = false;
            LastWill = String.Empty;
            RoleName = null;
        }

        public Player(User user)
            : base()
        {
            User = user;
        }

        public void SetRole(RoleStrategy role)
        {
            Role = role;
            RoleName = role.RoleName;
        }

        private int cursor = 0;
        public void Kill(Player killedBy)
        {
            bool done = false;
            while (cursor < Visitors.Count && !done)
            {
                var visitor = Visitors[cursor++];
                if (visitor.Role.RoleName == Models.RoleName.BodyGuard && !visitor.Done)
                {
                    killedBy.Kill(visitor);
                    visitor.Kill(killedBy);
                    killedBy.Done = true;
                    visitor.Done = true;
                    done = true;
                    string log = "you have been saved by a bodyguard";
                    this.AddLog(log);
                }
                else if (visitor.Role.RoleName == Models.RoleName.Doctor && !visitor.Done)
                {
                    killedBy.Done = true;
                    visitor.Done = true;
                    done = true;
                    string log = "you have been healed by a doctor";
                    this.AddLog(log);
                }
            }
            if (!done)
            {
                this.Alive = false;
                string log = "you've been killed by ";
                if(this.Role.Alignment == Alignment.Town && killedBy.Role.RoleName == Models.RoleName.Vigilante)
                {
                    killedBy.Alive = false;
                    string vigilanteLog = "you killed town folk, you die from guilt";
                    killedBy.AddLog(vigilanteLog);
                }

                log += killedBy.Role.Alignment == Alignment.Mafia ? "mafia" : killedBy.Role.RoleName.ToString().ToLower();
                this.AddLog(log);
            }
        }
        public void AddVisitor(Player player)
        {
            Visitors.Add(player);
        }
        public void AddLog(string log)
        {
            NightLog.Add(log);
        }
        public void ResetDone()
        {
            Done = false;
        }

        public void Reset()
        {
            Visitors.Clear();
            NightLog.Clear();
            Done = false;
            ResetSuspicous();
        }
        private void ResetSuspicous()
        {
            if (this.Role.Alignment == Alignment.Mafia && this.Role.RoleName != Models.RoleName.Godfather)
            {
                Suspicious = true;
            }
            else
            {
                Suspicious = false;
            }
        }
    }
}