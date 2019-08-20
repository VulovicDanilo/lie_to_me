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
                unit.PlayerRepository.Add(player);
                unit.Save();
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
        public HttpResponseMessage DeletePlayer([FromUri] Player id)
        {
            try
            {
                unit.PlayerRepository.Delete(id);
                unit.Save();
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