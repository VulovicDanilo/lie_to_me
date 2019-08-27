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
        public List<Game> GetGames()
        {
            try
            {
                return unitOfWork.GameRepository.List;
            }
            catch (Exception)
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
                throw new NotImplementedException();

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
