using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ImageService
    {
        private readonly string BaseAddress = "http://localhost:56864/";
        private readonly string GetImagePath = "/api/images/get";

        public void GetImage(string imageName)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);

            HttpResponseMessage response = client.GetAsync(GetImagePath + "?imageName=" + imageName).Result;
        }
    }
}
