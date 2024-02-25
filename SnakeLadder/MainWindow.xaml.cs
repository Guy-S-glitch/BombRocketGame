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
using static SnakeLadder.MainWindow;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

namespace SnakeLadder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            menu menyou = new menu();
            menyou.Show();
            this.Close();
        }
        private void Rules_Click(object sender, RoutedEventArgs e)
        {
            ruls.Visibility = Visibility.Visible;
            Men.Visibility = Visibility.Hidden;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void toMen_Click(object sender, RoutedEventArgs e)
        {
            ruls.Visibility = Visibility.Hidden;
            Men.Visibility = Visibility.Visible;
        }
    }

}
