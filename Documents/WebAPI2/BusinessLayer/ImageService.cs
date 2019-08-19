using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLayer
{
    public class ImageService
    {
        private readonly string BaseAddress = "http://localhost:56864/";
        private readonly string GetImagePath = "/api/images/get";

        public void StoreImage(FileInfo file, string imageName)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);
            FileStream stream = new FileStream(file.FullName, FileMode.Open);

            var imageContent = new MultipartFormDataContent();
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(file.FullName));
            imageContent.Add(streamContent, "file", imageName);

            HttpResponseMessage response = client.PostAsync("api/images/store", imageContent).Result;

        }

        public void GetImage(string imageName)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);

            HttpResponseMessage response = client.GetAsync(GetImagePath + "?imageName=" + imageName).Result;
        }
    }
}
