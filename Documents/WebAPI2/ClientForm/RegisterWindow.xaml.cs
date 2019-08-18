using BusinessLayer;
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

        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (tbxEmail.Text.Length > 0 &&
                Regex.IsMatch(tbxEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                if (tbxUsername.Text.Length > 0)
                {
                    if (pbxPass.Password.Length > 6)
                    {
                        UserService service = new UserService();
                        bool added = await service.AddUserAsync(tbxEmail.Text, tbxUsername.Text, pbxPass.Password, ImageName);
                        if (added == true)
                            this.Close();
                        else
                            lblRegisterInfo.Content = "Couldnt create user";

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
