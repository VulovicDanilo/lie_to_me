using DataLayer.DTOs;
using DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly string DeleteGamePath = "api/games/delete";
        private readonly string RequestContext = "api/games/context";
        private readonly string SetOwnerPath = "api/games/owner";
        private readonly string StartNameSelectionPath = "api/games/name_selection";


        public List<GameListing> GetGames()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);


                var msg = client.GetAsync(GetGamesPath).Result;

                List<GameListing> games = new List<GameListing>();

                string json = msg.Content.ReadAsStringAsync().Result;

                List<GameListing> list = (List<GameListing>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<GameListing>));

                if (list != null)
                    games = list;

                return games;
            }
        }

        public int AddGame(DateTime? startTime, DateTime? endTime, Alignment? winner = Alignment.NotDecided)
        {
            using (HttpClient client = new HttpClient())
            {
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


                //Game gotBack = JsonConvert.DeserializeObject<Game>(msg.Content.ReadAsStringAsync().Result);

                //return gotBack;

                if (msg.StatusCode == HttpStatusCode.OK)
                {
                    string text = msg.Content.ReadAsStringAsync().Result;

                    return int.Parse(text);
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool RequestContextBroadcast(string queueName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);


                var values = new Dictionary<string, string>()
                {
                    {"queueName", queueName},
                };
                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


                HttpResponseMessage msg = client.GetAsync(RequestContext + "?queueName=" + queueName).Result;


                return msg.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }

        public bool SetGameOwner(int gameId, int playerId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                var values = new Dictionary<string, string>()
                {
                    {"gameId", gameId.ToString() },
                    {"playerId", playerId.ToString() },
                };

                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage msg = client.PutAsync(SetOwnerPath, content).Result;

                return msg.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }

        public bool DeleteGame(int gameId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                HttpResponseMessage msg = client.DeleteAsync(DeleteGamePath + "?Id=" + gameId.ToString()).Result;

                return msg.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }

        public bool StartNameSelectionPhase(int gameId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                HttpResponseMessage msg = client.GetAsync(StartNameSelectionPath + "&gameId=" + gameId.ToString()).Result;

                return msg.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }
    }
}
