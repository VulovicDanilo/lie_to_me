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
            if (listGames.SelectedIndex != -1)
            {
                Game selected = listGames.SelectedItem as Game;
                if (!selected.Full)
                {
                    PlayerService playerService = new PlayerService();
                    Player player = playerService.AddPlayer(null, User, selected);

                    selected.GameContext.AddPlayer(player);

                    // JOINING
                    this.Hide();
                    lblInfo.Content = "";
                    GameWindow gameWindow = new GameWindow(selected, player);
                    gameWindow.Closed += new EventHandler(this.Reveal);
                    gameWindow.ShowDialog();
                }
                else
                {
                    lblInfo.Content = "room already full";
                    RefreshPanel();
                    listGames.UnselectAll();
                }
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game()
            {
                GameContext = new GameContext()
                {
                    Players = new List<Player>(),
                    GameMode = GameMode.Classic,
                    GameState = GameState.Lobby,
                    Winners = new List<Player>(),
                },
                Winner = Alignment.NotDecided,
                StartTime = null,
                EndTime = null,
            };

            GameService service = new GameService();
            game = service.AddGame(game.StartTime, game.EndTime, game.Winner);

            if (game != null)
            {
                PlayerService playerService = new PlayerService();
                Player player = playerService.AddPlayer(null, User, game);

                game.Owner = player;


                // CREATING

                this.Hide();
                lblInfo.Content = "";
                GameWindow gameWindow = new GameWindow(game, player);
                gameWindow.Closed += new EventHandler(this.Reveal);
                gameWindow.ShowDialog();
            }
            else
            {
                lblInfo.Content = "unable to create a room";
            }

        }

        private void Reveal(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
