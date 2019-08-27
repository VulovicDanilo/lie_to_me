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
    public class PlayerService
    {
        private readonly string BaseAddress = "http://localhost:56864/";
        private readonly string AddPlayerPath = "/api/players/add";
        private readonly string DeletePlayerPath = "/api/players/delete";
        private readonly string UpdatePlayerPath = "/api/players/update";
        public Player AddPlayer(RoleName? role, User user, int gameId)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);


            var values = new Dictionary<string, string>()
            {
                {"RoleName", role.ToString()},
                {"User_Id", user.Id.ToString()},
                {"GameId", gameId.ToString() }
            };
            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


            HttpResponseMessage msg = client.PostAsync(AddPlayerPath, content).Result;

            Player player = JsonConvert.DeserializeObject<Player>(msg.Content.ReadAsStringAsync().Result);
            return player;
        }
        public bool DeletePlayer(Player player)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);


            var values = new Dictionary<string, string>()
            {
                {"Id", player.Id.ToString()},
                {"GameId", player.GameId.ToString() }
            };
            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


            HttpResponseMessage msg = client.DeleteAsync(DeletePlayerPath + content).Result;

            return msg.StatusCode == HttpStatusCode.OK ? true : false;

        }
        public bool Update(Player player)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);


            var values = new Dictionary<string, string>()
            {
                {"Id", player.Id.ToString() },
                {"RoleName", player.RoleName.ToString()},
                {"User_Id", player.User_Id.ToString() },
            };
            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


            HttpResponseMessage msg = client.PutAsync(UpdatePlayerPath, content).Result;

            return msg.StatusCode == HttpStatusCode.OK ? true : false;

        }
    }
}
