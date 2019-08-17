using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        public RegisterWindow()
        {
            InitializeComponent();
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
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:56864/");
                
                string filename = op.FileName;
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
                        this.Dispatcher.Invoke(() =>
                        {
                            lblInfo.Content = "Succesfully uploaded!";
                        });
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            lblInfo.Content = response.ReasonPhrase;
                        });
                    }
                });
            }
        }
    }
}
