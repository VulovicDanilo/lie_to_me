using DataLayer.DTOs;
using DataLayer.Models;
using DataLayer.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Helpers;

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
                user.Password = SecurePasswordHasher.Hash(user.Password);
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
        public HttpResponseMessage LoginUser([FromBody] LoginModel loginModel)
        {
            User user = unit.UserRepository.Find(loginModel.Username);
            if (user != null)
            {
                if (SecurePasswordHasher.Verify(loginModel.Password, user.Password))
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