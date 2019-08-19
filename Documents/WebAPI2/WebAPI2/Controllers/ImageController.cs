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
    [RoutePrefix("api/images")]
    public class ImageController : ApiController
    {
        [HttpPost]
        [Route("store")]
        public HttpResponseMessage StoreImage()
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
        [Route("get")]
        public HttpResponseMessage GetImage([FromUri]string imageName)
        {
            if (imageName != String.Empty)
            {
                var response = new HttpResponseMessage();

                // TODO FETCH ImageName from User
                string path = HttpContext.Current.Server.MapPath("~/UserImages/" + imageName);
                if (!File.Exists(path))
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }
                FileInfo fileInfo = new FileInfo(path);
                FileStream stream = File.OpenRead(path);

                var imageContent = new MultipartFormDataContent();
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileInfo.FullName));
                imageContent.Add(streamContent, "file", fileInfo.Name);

                response.Content = imageContent;

                return response;

            }
            else
            {
                string message = "Image path isn't provided";
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
        }
    }
}