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
    public class PlayerService
    {
        private readonly string BaseAddress = "http://localhost:56864/";
        private readonly string AddPlayerPath = "/api/players/add";
        private readonly string DeletePlayerPath = "/api/players/delete";
        private readonly string UpdatePlayerPath = "/api/players/update";
        private readonly string ChooseNamePath = "/api/players/name";
        private readonly string RequestRolePath = "/api/players/request_role";
        private readonly string ChatMessagePath = "/api/players/send_message";
        private readonly string VotingPath = "/api/players/voting_vote";
        private readonly string JudgementPath = "api/players/judgement_vote";
        private readonly string AddActionPath = "api/players/add_action";
        private readonly string RemoveActionPath = "api/players/remove_action";
        private readonly string LastWillPath = "api/players/lastwill";
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

            if (msg.StatusCode == HttpStatusCode.OK)
            {
                Player player = JsonConvert.DeserializeObject<Player>(msg.Content.ReadAsStringAsync().Result);
                return player;
            }
            else
            {
                return null;
            }
        }
        public bool DeletePlayer(Player player)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);

            HttpResponseMessage msg = client.DeleteAsync(DeletePlayerPath + "id=" + player.Id.ToString()
                + "&gameId=" + player.GameId.ToString()).Result;

            return msg.StatusCode == HttpStatusCode.OK ? true : false;

        }
        public bool DeletePlayer(int playerId, int gameId)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);


            var values = new Dictionary<string, string>()
            {
                {"Id", playerId.ToString()},
                {"GameId", gameId.ToString() }
            };
            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


            HttpResponseMessage msg = client.DeleteAsync(DeletePlayerPath + "?id=" + playerId.ToString() + "&gameId=" + gameId.ToString()).Result;

            return msg.StatusCode == HttpStatusCode.OK ? true : false;

        }
        public bool UpdatePlayer(Player player, int gameId)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);


            var values = new Dictionary<string, string>()
            {
                {"Id", player.Id.ToString() },
                {"RoleName", player.RoleName.ToString()},
                {"GameId", gameId.ToString() },
                {"Alive", player.Alive.ToString() }
            };
            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


            HttpResponseMessage msg = client.PutAsync(UpdatePlayerPath, content).Result;

            return msg.StatusCode == HttpStatusCode.OK ? true : false;
        }
        public bool AddFakeName(int playerId, string fakeName, int gameId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                var values = new Dictionary<string, string>()
                {
                    {"gameId", gameId.ToString() },
                    {"playerId", playerId.ToString() },
                    {"fakeName", fakeName }
                };

                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage msg = client.PostAsync(ChooseNamePath, content).Result;

                return msg.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }

        public RoleStrategy RequestStrategy(int gameId, int playerId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                var values = new Dictionary<string, string>()
                {
                    {"gameId", gameId.ToString() },
                    {"playerId", playerId.ToString() }
                };

                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage msg = client.PostAsync(RequestRolePath, content).Result;

                if (msg.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }
                else
                {
                    string json = msg.Content.ReadAsStringAsync().Result;

                    RoleStrategy strategy = JsonConvert.DeserializeObject<RoleStrategy>(json);
                    return strategy;
                }

            }
        }
        public bool SendChatMessage(ChatMessage chatMessage)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                var values = new Dictionary<string, string>()
                {
                    {"PlayerId", chatMessage.PlayerId.ToString() },
                    {"GameState", ((int)chatMessage.GameState).ToString() },
                    {"Content", chatMessage.Content },
                    {"GameId", chatMessage.GameId.ToString() },
                    {"Time", chatMessage.Time.ToString() }
                };

                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage msg = client.PostAsync(ChatMessagePath, content).Result;

                return msg.StatusCode == HttpStatusCode.OK ? true : false; 
            }
        }
        public bool Vote(int gameId, int voterNumber, int votedNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);


                var values = new Dictionary<string, string>()
                {
                    {"gameId", gameId.ToString()},
                    {"voterNumber", voterNumber.ToString()},
                    {"votedNumber", votedNumber.ToString() }
                };
                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage msg = client.PostAsync(VotingPath, content).Result;

                return msg.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }
        public bool JudgementVote(int gameId, int voterNumber, JudgementVote vote)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);


                var values = new Dictionary<string, string>()
                {
                    {"gameId", gameId.ToString()},
                    {"voterNumber", voterNumber.ToString()},
                    {"vote", vote.ToString() }
                };
                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage msg = client.PostAsync(JudgementPath, content).Result;

                return msg.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }
        public bool DoAction(ExecuteActionModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);


                var values = new Dictionary<string, string>()
                {
                    {"Who", model.Who.ToString()},
                    {"To", model.To.ToString()},
                    {"gameId", model.gameId.ToString() }
                };
                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage msg = client.PostAsync(AddActionPath, content).Result;

                return msg.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }
        public bool UndoAction(ExecuteActionModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);


                var values = new Dictionary<string, string>()
                {
                    {"Who", model.Who.ToString()},
                    {"To", model.To.ToString()},
                    {"gameId", model.gameId.ToString() }    
                };
                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage msg = client.PostAsync(RemoveActionPath, content).Result;

                return msg.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }
        public bool SetLastWill(LastWillModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);
                var values = new Dictionary<string, string>()
                {
                    {"GameId", model.GameId.ToString()},
                    {"Number", model.Number.ToString()},
                    {"LastWill", model.LastWill.ToString() }
                };
                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage msg = client.PostAsync(LastWillPath, content).Result;

                return msg.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }
    }
}
