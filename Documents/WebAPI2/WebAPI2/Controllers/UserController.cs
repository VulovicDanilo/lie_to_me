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


                string id = user.Id.ToString();

                var response = Request.CreateResponse(HttpStatusCode.OK, id);
                return response;
            }
            catch(Exception ex)
            {
                string message = "Hello";
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
        }
    }
}