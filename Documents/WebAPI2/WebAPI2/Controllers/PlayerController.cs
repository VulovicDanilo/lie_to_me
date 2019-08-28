using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using DataLayer.Models;
using DataLayer.Repositories;
using WebAPI2.GameStuff;

namespace WebAPI2.Controllers
{
    [RoutePrefix("api/players")]
    public class PlayerController : ApiController
    {
        private UnitOfWork unit = new UnitOfWork();
        [Route("add")]
        [HttpPost]
        public HttpResponseMessage AddPlayer([FromBody] Player player)
        {
            try
            {
                var game = unit.GameRepository.Find(player.GameId);
                var user = unit.UserRepository.Find(player.User_Id);
                player.User = user;
                if (game.Players.Count == 0)
                {
                    game.Owner = player;
                }
                if (game.Players.Count == game.MAX_PLAYERS)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Capacity reached");
                }
                game.Players.Add(player);
                unit.Save();

                GameDictionary.AddPlayer(game);

                // QueueService.BroadcastContext(game.Id.ToString(), MessageQueueChannel.ContextBroadcast, game);

                return Request.CreateResponse(HttpStatusCode.OK, player);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
        }
        [Route("update")]
        [HttpPut]
        public HttpResponseMessage UpdatePlayer([FromBody] Player player)
        {
            try
            {
                if (player.Id != 0)
                {
                    unit.PlayerRepository.Update(player);
                    unit.Save();

                    GameDictionary.UpdatePlayer(player.GameId, player);

                    var game = GameDictionary.Get(player.GameId);

                    QueueService.BroadcastContext(game.Id.ToString(), MessageQueueChannel.ContextBroadcast, game);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    throw new Exception("no such player to update");
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
        }
        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage DeletePlayer([FromUri] int id, [FromUri] int gameId)
        {
            try
            {
                unit.PlayerRepository.Delete(id);
                unit.Save();

                GameDictionary.RemovePlayer(gameId, id);
                var game = GameDictionary.Get(gameId);

                QueueService.BroadcastContext(game.Id.ToString(), MessageQueueChannel.ContextBroadcast, game);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
        }
        protected override void Dispose(bool disposing)
        {
            unit.Dispose();
            base.Dispose(disposing);
        }
    }
}