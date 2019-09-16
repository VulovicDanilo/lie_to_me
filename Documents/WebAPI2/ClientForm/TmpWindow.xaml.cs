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
    /// Interaction logic for TmpWindow.xaml
    /// </summary>
    public partial class TmpWindow : Window
    {
        public TmpWindow()
        {
            InitializeComponent();
            cnvSecond.Visibility = Visibility.Collapsed;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            cnvSecond.Visibility = Visibility.Collapsed;
            cnvMain.Visibility = Visibility.Visible;

        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            cnvMain.Visibility = Visibility.Collapsed;
            cnvSecond.Visibility = Visibility.Visible;
        }
    }
}
