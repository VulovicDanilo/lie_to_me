using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class Gamer
    {
        public int GamerID { get; set; }
        public string Email { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string UserName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

    }
}