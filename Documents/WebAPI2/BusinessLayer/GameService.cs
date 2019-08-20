using DataLayer.DTOs;
using DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class GameService
    {
        private readonly string BaseAddress = "http://localhost:56864/";
        private readonly string GetGamesPath = "api/games/all";
        private readonly string AddGamePath = "api/games/add";


        public List<Game> GetGames()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);

            
            var msg = client.GetAsync(GetGamesPath).Result; // deserialize this

            List<Game> games = new List<Game>();

            string json = msg.Content.ReadAsStringAsync().Result;

            List<Game> list = (List<Game>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Game>));

            if (list != null)
                games = list;
            
            return games;
        }

        public Game AddGame(DateTime? startTime, DateTime? endTime, Alignment? winner = Alignment.NotDecided)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);

            var values = new Dictionary<string, string>()
            {
                {"StartTime", startTime.ToString()},
                {"EndTime", endTime.ToString()},
                {"Winner", winner.ToString() }
            };
            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var msg = client.PostAsync(AddGamePath, content).Result;

            Game gotBack = JsonConvert.DeserializeObject<Game>(msg.Content.ReadAsStringAsync().Result);

            return gotBack;
        }

    }
}
