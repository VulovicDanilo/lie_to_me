using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class Player:IEntity
    {
        public int PlayerID { get; set; }
        public virtual Gamer Gamer { get; set; }
        public virtual InGameRole InGameRole {get; set;}
        public string Will { get; set; }
        public bool Alive { get; set; }
        public string FakeName { get; set; }
        public int Number { get; set; }

        public int ID
        {
            get
            {
                return PlayerID;
}
        }
    }
}