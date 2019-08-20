using DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class GamesListJson
    {
        public List<Game> games { get; set; }
        public int count { get; set; }

        public GamesListJson()
        {
            games = new List<Game>();
        }
    }
}
