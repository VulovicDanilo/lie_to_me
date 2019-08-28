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
using System.Threading;
using DataLayer.DTOs;

namespace ClientForm
{
    /// <summary>
    /// Interaction logic for LobbyWindow.xaml
    /// </summary>
    public partial class LobbyWindow : Window
    {
        public User User { get; set; }
        public List<GameListingDTO> Games { get; set; }
        public LobbyWindow(User user)
        {
            InitializeComponent();
            User = user;
            this.Title += " - " + User.UserName;
            GameService service = new GameService();
            RefreshPanel();
        }

        private void RefreshPanel()
        {
            GameService service = new GameService();
            Games = service.GetGames();

            listGames.Items.Refresh();
        }

        private void BtnJoin_Click(object sender, RoutedEventArgs e)
        {
            if (listGames.SelectedIndex != -1)
            {
                int index = listGames.SelectedIndex;
                int gameId = Games.ElementAt(index).Id;

                PlayerService playerService = new PlayerService();
                Player player = playerService.AddPlayer(null, User, gameId);
                if (player != null)
                {
                    string queueName = gameId.ToString();


                    this.Hide();
                    this.lblInfo.Content = "";
                    GameWindow gameWindow = new GameWindow(player, queueName);
                    gameWindow.Closed += new EventHandler(this.Reveal);
                    gameWindow.ShowDialog();
                }
                else
                {
                    lblInfo.Content = "couldnt join, room might be full.";
                    RefreshPanel();
                }
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            GameService service = new GameService();

            DateTime? start = DateTime.Now;
            DateTime? end = DateTime.Now;
            int gameId = service.AddGame(start, end);

            PlayerService playerService = new PlayerService();
            Player player = playerService.AddPlayer(null, User, gameId);

            if (player != null)
            {
                string queueName = gameId.ToString();
                
                this.Hide();
                this.lblInfo.Content = "";
                GameWindow gameWindow = new GameWindow(player, queueName);
                gameWindow.Closed += new EventHandler(this.Reveal);
                gameWindow.ShowDialog();
            }
        }

        private void Reveal(object sender, EventArgs e)
        {
            RefreshPanel();
            this.Show();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshPanel();
        }
    }
}
