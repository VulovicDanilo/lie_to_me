using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class Player:IEntity
    {
        public int PlayerID { get; set; }
        public virtual User User { get; set; }
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