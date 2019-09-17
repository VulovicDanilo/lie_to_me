using BusinessLayer;
using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientForm.Controls
{
    /// <summary>
    /// Interaction logic for PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        public int Number { get; set; }
        public string FakeName { get; set; }
        public string ImagePath { get; set; }
        public PlayerControl()
        {
            InitializeComponent();
        }

        public PlayerControl(InGamePlayer player)
            :base()
        {
            Number = player.Number;
            FakeName = player.FakeName;
            ImagePath = player.ImagePath;

            Fill();
        }

        public void Fill()
        {
            labName.Content = Number.ToString() + " " + FakeName;
            ShowImage();
        }

        private void ShowImage()
        {
            ImageService service = new ImageService();
            var stream = service.GetImage(ImagePath);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            // just in case you want to load the image in another thread
            bitmapImage.Freeze();
            imgAvatar.Source = bitmapImage;
            imgAvatar.Stretch = Stretch.Fill;
        }
    }
}
