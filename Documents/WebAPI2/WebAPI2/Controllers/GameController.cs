using DataLayer.DTOs;
using DataLayer.Models;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.GameStuff;

namespace WebAPI2.Controllers
{
    [RoutePrefix("api/games")]
    public class GameController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [Route("all")]
        [HttpGet]
        public List<GameListingDTO> GetGames()
        {
            try
            {
                var games = unitOfWork.GameRepository.List;
                var dtos = new List<GameListingDTO>();
                foreach(var game in games)
                {
                    var fullGame = GameDictionary.Get(game.Id);
                    if (fullGame != null)
                    {
                        dtos.Add(GameListingDTO.ToDTO(fullGame));
                    }
                }
                return dtos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("add")]
        [HttpPost]
        public HttpResponseMessage AddGame([FromBody] Game game)
        {
            try
            {
                unitOfWork.GameRepository.Add(game);
                unitOfWork.Save();

                var context = GameDictionary.Add(game);


                return Request.CreateResponse(HttpStatusCode.OK, game.Id);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse
                    (HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage UpdateGame([FromBody] Game game)
        {
            try
            {
                unitOfWork.GameRepository.Update(game);
                unitOfWork.Save();
                return Request.CreateResponse(HttpStatusCode.OK, game);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to update player. Message: " + ex.Message);
            }
        }
        [Route("context")]
        [HttpGet]
        public HttpResponseMessage RequestContext([FromUri] string queueName)
        {
            try
            {
                var context = GameDictionary.Get(int.Parse(queueName));

                QueueService.BroadcastContext(queueName.ToString(), MessageQueueChannel.ContextBroadcast, context);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse
                    (HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteGame([FromUri] Game game)
        {
            try
            {
                unitOfWork.GameRepository.Delete(game);
                unitOfWork.Save();

                GameDictionary.Remove(game.Id);
                return Request.CreateResponse(HttpStatusCode.OK, "Game deleted");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to delete game. Message: " + ex.Message);
            }
        }
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
