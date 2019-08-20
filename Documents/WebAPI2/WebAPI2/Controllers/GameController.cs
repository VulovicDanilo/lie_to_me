using DataLayer.Models;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI2.Controllers
{
    [RoutePrefix("api/games")]
    public class GameController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [Route("all")]
        [HttpGet]
        public List<Game> GetGames()
        {
            try
            {
                return unitOfWork.GameRepository.List;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("lobby")]
        [HttpGet]
        public List<Game> GetInLobbyGames()
        {
            try
            {
                return unitOfWork.GameRepository.List
                    .Where(x => x.GameContext.GameState == GameState.Lobby).ToList();

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
                return Request.CreateResponse(HttpStatusCode.OK, game);
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

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteGame([FromUri] Game game)
        {
            try
            {
                unitOfWork.GameRepository.Delete(game);
                unitOfWork.Save();
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
