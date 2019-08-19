using DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLayer
{
    public class UserService
    {
        private readonly string BaseAddress = "http://localhost:56864/";
        private readonly string AddUserPath = "/api/users/add";
        private readonly string UserLoginPath = "/api/users/login";
        public bool AddUser(string email, string username, string password, string imagePath)
        {
            bool userCreated = false;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);

            User user = new User()
            {
                Email = email,
                UserName = username,
                Password = password,
            };

            string filename = imagePath;
            var fileStream = File.Open(filename, FileMode.Open);
            var fileInfo = new FileInfo(filename);

            user.ImagePath = email + fileInfo.Extension;

            var values = new Dictionary<string, string>()
            {
                {"UserName", username},
                {"Email", email},
                {"Password", password},
                {"ImagePath", user.ImagePath }
            };
            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


            HttpResponseMessage msg = client.PostAsync(AddUserPath, content).Result;

            if (msg.StatusCode == HttpStatusCode.OK)
            {
                userCreated = true;
            }

            var imageContent = new MultipartFormDataContent();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileInfo.FullName));
            imageContent.Add(streamContent, "file", user.ImagePath);

            HttpResponseMessage response = client.PostAsync("api/images/store", imageContent).Result;
                        

            return userCreated;
        }

        public User Login(string username, string password)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);
            var values = new Dictionary<string, string>()
            {
                {"username", username},
                {"password", password},
            };
            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = client.PostAsync(UserLoginPath, content).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            User user = JsonConvert.DeserializeObject<User>(json);

            return user;
        }
    }
}
