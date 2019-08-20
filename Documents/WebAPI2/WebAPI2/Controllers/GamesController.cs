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
    public class GamesController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [HttpGet]
        public List<Game> GetGames()
        {
            try
            {
                return  unitOfWork.GameRepository.List;
            } catch(Exception e)
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
            } catch (Exception ex)
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
                /* Should't game repository resolve Update? -- in player service it's done manually */
                unitOfWork.GameRepository.Update(game);
                unitOfWork.Save();
                return Request.CreateResponse(HttpStatusCode.OK, game);
            } catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to update player");
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
            } catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to delete game");
            }
        }
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}
