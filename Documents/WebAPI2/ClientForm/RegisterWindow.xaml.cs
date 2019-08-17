using DataLayer.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientForm
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private User User { get; set; }
        private string ImageName { get; set; } = String.Empty;
        public RegisterWindow()
        {
            InitializeComponent();
            User = new User();
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ImageName = op.FileName;

                using (var stream = new FileStream(ImageName, FileMode.Open))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze(); // just in case you want to load the image in another thread
                    imgPlane.Source = bitmapImage;
                    imgPlane.Stretch = Stretch.Fill;
                }
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (tbxEmail.Text.Length > 0 &&
                Regex.IsMatch(tbxEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                if (tbxUsername.Text.Length > 0)
                {
                    if (pbxPass.Password.Length > 6)
                    {
                        // Add user
                        if (ImageName != String.Empty && new FileInfo(ImageName).Exists)
                        {
                            #region UploadImage
                            HttpClient client = new HttpClient();
                            client.BaseAddress = new Uri("http://localhost:56864/");

                            string filename = ImageName;
                            var fileStream = File.Open(filename, FileMode.Open);
                            var fileInfo = new FileInfo(filename);

                            var content = new MultipartFormDataContent();
                            var streamContent = new StreamContent(fileStream);
                            streamContent.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileInfo.FullName));
                            content.Add(streamContent, "file", fileInfo.Name);



                            Task upload = client.PostAsync("api/Image/Store", content).ContinueWith(task =>
                            {
                                var response = task.Result;

                                if (response.IsSuccessStatusCode)
                                {
                                    this.Dispatcher.Invoke(async () =>
                                    {
                                        string json = await response.Content.ReadAsStringAsync();
                                        string[] data = json.Split('\"');
                                        ImageName = data[3];
                                    });
                                }
                            });
                            #endregion
                            MessageBox.Show("Success\nEmail: " + tbxEmail.Text + "\n" +
                                        "Username: " + tbxUsername.Text + "\n" +
                                        "Pass: mrk, nema. \n" +
                                        "Image name: " + ImageName);
                        }
                        else
                        {
                            // fall back to default avatar
                            MessageBox.Show("Success\nEmail: " + tbxEmail.Text + "\n" +
                                "Username: " + tbxUsername.Text + "\n" +
                                "Pass: mrk, nema. \n" +
                                "Image name: " + ImageName);
                        }
                    }
                    else
                    {
                        lblRegisterInfo.Content = "password doesn't meet criteria";
                    }
                }
                else
                {
                    lblRegisterInfo.Content = "username field is empty";
                }
            }
            else
            {
                lblRegisterInfo.Content = "bad email form";
            }
        }
    }
}
