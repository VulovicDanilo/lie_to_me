using BusinessLayer;
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

namespace ClientForm
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LblRegister_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            User user = new User();

            RegisterWindow regWindow = new RegisterWindow();

            regWindow.ShowDialog();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tbxUsername.Text != string.Empty)
            {
                if (pbxPassword.Password.Length > 6)
                {
                    UserService service = new UserService();
                    User loggedUser = service.Login(tbxUsername.Text, pbxPassword.Password);
                    if (loggedUser != null)
                    {
                        this.Hide();
                        WelcomeWindow welcome = new WelcomeWindow(loggedUser);
                        welcome.Closed += new EventHandler(this.Reveal);
                        welcome.ShowDialog();
                    }
                    else
                    {
                        lblInfo.Content = "wrong credentials";
                    }
                }
                else
                {
                    lblInfo.Content = "password too short";
                }
            }
            else
            {
                lblInfo.Content = "username field is empty";
            }
        }

        private void Reveal(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
