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
        public bool Alive { get; set; }
        public PlayerControl()
        {
            InitializeComponent();
            btnAction.Visibility = Visibility.Collapsed;
            btnGuilty.Visibility = Visibility.Collapsed;
            btnInnocent.Visibility = Visibility.Collapsed;
            btnVote.Visibility = Visibility.Collapsed;
        }

        public PlayerControl(InGamePlayer player)
            :this()
        {
            Number = player.Number;
            FakeName = player.FakeName;
            ImagePath = player.ImagePath;
            Alive = player.Alive;
            Fill();
        }

        public void Fill()
        {
            labName.Content = Number.ToString() + " " + FakeName;
            // ShowImage();
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

        public void EnableVoting()
        {
            if (Alive)
            {
                btnVote.Visibility = Visibility.Visible;
                btnAction.Visibility = Visibility.Collapsed;
                btnGuilty.Visibility = Visibility.Collapsed;
                btnInnocent.Visibility = Visibility.Collapsed;
            }
        }

        public void EnableAction()
        {
            if (Alive)
            {
                btnAction.Visibility = Visibility.Visible;
                btnVote.Visibility = Visibility.Collapsed;
                btnGuilty.Visibility = Visibility.Collapsed;
                btnInnocent.Visibility = Visibility.Collapsed;
            }
        }
        public void EnableJudgement()
        {
            if (Alive)
            {
                btnAction.Visibility = Visibility.Collapsed;
                btnVote.Visibility = Visibility.Collapsed;
                btnGuilty.Visibility = Visibility.Visible;
                btnInnocent.Visibility = Visibility.Visible;
            }
        }
        public void DisableButtons()
        {
            if (Alive)
            {
                btnAction.Visibility = Visibility.Collapsed;
                btnVote.Visibility = Visibility.Collapsed;
                btnGuilty.Visibility = Visibility.Collapsed;
                btnInnocent.Visibility = Visibility.Collapsed;
            }
        }
        public void Die()
        {
            Alive = false;
            Background = Brushes.Red;
            labName.Content += " - dead...";
        }
    }
}
