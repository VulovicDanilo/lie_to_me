using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    class GameService
    {
        private readonly string BaseAddress = "http://localhost:56864/";
        private readonly string GetGamesPath = "api/Games";

        public List<Game> GetGames()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);

            var games = client.GetAsync(GetGamesPath).Result; // deserialize this
        }

    }
}
