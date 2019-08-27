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
                if (game.Players.Count == 0)
                {
                    game.Owner = player;
                }
                game.Players.Add(player);
                unit.Save();

                var context = GameDictionary.Get(player.GameId);

                QueueService.BroadcastContext(game.Id.ToString(), MessageQueueChannel.ContextBroadcast, context);

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
                    return Request.CreateResponse(HttpStatusCode.OK, player);
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
        public HttpResponseMessage DeletePlayer([FromBody] Player player)
        {
            try
            {
                int gameId = player.GameId;
                unit.PlayerRepository.Delete(player.Id);

                GameDictionary.RemovePlayer(player.GameId, player.Id);

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