using System.Windows;

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
