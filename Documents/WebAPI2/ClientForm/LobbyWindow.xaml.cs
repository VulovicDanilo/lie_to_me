using DataLayer.Models;
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
using System.Windows.Shapes;
using BusinessLayer;

namespace ClientForm
{
    /// <summary>
    /// Interaction logic for LobbyWindow.xaml
    /// </summary>
    public partial class LobbyWindow : Window
    {
        public User User { get; set; }
        public List<Game> Games { get; set; }
        public LobbyWindow(User user)
        {
            InitializeComponent();
            User = user;
            GameService service = new GameService();

            Games = service.GetGames();


            RefreshPanel();
        }

        private void RefreshPanel()
        {
            GameService service = new GameService();
            Games = service.GetGames();

            listGames.Items.Clear();

            listGames.ItemsSource = Games;
        }

        private void BtnJoin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void Reveal(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
