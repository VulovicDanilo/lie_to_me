using DataLayer.Models;
using DataLayer.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI2.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private UnitOfWork unit = new UnitOfWork();
        [HttpPost]
        [Route("add")]
        public HttpResponseMessage AddUser([FromBody] User user)
        {
            try
            {
                unit.UserRepository.Add(user);
                unit.Save();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
        }
        [HttpPost]
        [Route("login")]
        public HttpResponseMessage LoginUser([FromBody] string username, [FromBody] string password)
        {
            User user = unit.UserRepository.Find(username);
            if (user != null)
            {
                if (user.Password == password)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "password is incorrect");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "username is incorrect");
            }
        }
    }
}