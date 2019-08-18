using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebAPI2.Controllers
{
    [RoutePrefix("api/Image")]
    public class ImageController : ApiController
    {
        [HttpPost]
        [Route("Store")]
        public async Task<HttpResponseMessage> StoreImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                var postedFile = httpRequest.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {

                    int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    if (!AllowedFileExtensions.Contains(extension))
                    {

                        var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                    else if (postedFile.ContentLength > MaxContentLength)
                    {

                        var message = string.Format("Please Upload a file up to 1 mb.");

                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                    else
                    {
                        var filePath = HttpContext.Current.Server.MapPath("~/UserImages/" + postedFile.FileName);
                        postedFile.SaveAs(filePath);

                    }
                }
                else
                {
                    var res = string.Format("Falling back to default image");
                    dict.Add("info", res);
                    return Request.CreateResponse(HttpStatusCode.OK, dict);
                }
                var messageSuccess = string.Format(postedFile.FileName);
                return Request.CreateResponse(HttpStatusCode.OK, messageSuccess);
            }
            catch (Exception e)
            {
                var res = string.Format("Exception: " + e.Message);
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
        [HttpGet]
        [Route("api/GetImage")]
        public IHttpActionResult GetImage(int? id)
        {
            if (id.HasValue)
            {
                // TODO COMBINE PATH
                var response = new HttpResponseMessage();

                // TODO FETCH ImageName from User
                string imageName = "imeSlike.jpg";
                string path = HttpContext.Current.Server.MapPath("~/UserImages/" + id + "/" + imageName);
                if (File.Exists(path))
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return ResponseMessage(response);
                }
                FileStream stream = File.OpenRead(path);
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StreamContent(stream);

                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(path));
                response.Content.Headers.ContentLength = stream.Length;

                return ResponseMessage(response);
            }
            else
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, "id is not specified"));
            }
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}