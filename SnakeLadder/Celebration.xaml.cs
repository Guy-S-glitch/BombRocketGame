using gameLogic;
using GameLogic;
using GameLogic.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace SnakeLadder
{
    /// <summary>
    /// Interaction logic for Celebration.xaml
    /// </summary>
    public partial class Celebration : Window
    {
        public Celebration(List<Player> Data,int players)
        {
            
            InitializeComponent();
            List<Player> backup = new List<Player>();
            List<Label> ranky = new List<Label>()
            {
                new Label{Content="🥇 First",Foreground=new SolidColorBrush(Colors.Gold)},
                new Label{Content="🥈 Second",Foreground=new SolidColorBrush(Colors.Silver)},
                new Label{Content="🥉 third",Foreground=new SolidColorBrush(Colors.Brown)},
                new Label{Content="Fourth"},
                new Label{Content="Fifth"},
                new Label{Content="Sixth"}
            };
            foreach (Label addi in ranky)
            {
                ranks.Items.Add(addi);
            }
            for(int deleteAt=5;deleteAt>=players;deleteAt--) ranks.Items.RemoveAt(deleteAt);




            int count = Data.Count;
            Player backUpPlayer = Data[0];
            for (int i = 0; i < count; i++)
                {
                if (i == 0) foreach (Player player in Data) player.SetTrash(true);
                int max = 0;
                int j = 0;
                int k = 0;
                foreach (Player player in Data)
                {
                    if (player.Place >= max && player.GetTrash())
                    {
                        max = player.Place;
                        backUpPlayer = player;
                        j = k;
                    }
                    k++;
                }
                backup.Add(backUpPlayer);
                Data[j].SetTrash(false);
            }
            
            /*
            EndGame.ItemsSource = backup;
            */
            EndGame.ItemsSource = backup;
        }

        private void Amogus_MediaEnded(object sender, RoutedEventArgs e)
        {
            Amogus.Position = TimeSpan.FromMilliseconds(1);
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)   //once a key pressed enter the method
        {
            switch (e.Key)
            {
                case Key.Escape:   //if player press esc ask him if he's sure he want to exit the game
                    ExitConfirm();
                    break;
                case Key.Home:   //if player press home ask him if he's sure he want to return to the menu
                    HomeConfirm();
                    break;
                default: break;
            }
        }
        private void back_Click(object sender, RoutedEventArgs e)   //if player press menu button return to the menu 
        {
            HomeConfirm();
        }
        private void exit_Click(object sender, RoutedEventArgs e)   //if player press the exit button exit the game
        {
            ExitConfirm();
        }
        private void ExitConfirm()
        {
            object var = MessageBox.Show("Are you sure you  want to exit", "Conform Exit", MessageBoxButton.YesNo);
            switch (var)
            {
                case MessageBoxResult.Yes:
                    MessageBox.Show("thanks for playing");
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("returning to the game");
                    break;

            }
        }
        private void HomeConfirm()
        {
            object var = MessageBox.Show("Are you sure you  want to return to the menu", "Conform Menu", MessageBoxButton.YesNo);
            switch (var)
            {
                case MessageBoxResult.Yes:
                    MessageBox.Show("going back to menu");
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("returning to the game");
                    break;

            }
        }
    }
}
