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
        private string _exitMessage = "Are you sure you want to exit", _exitCaption = "Conform Exit", _exitYes = "thanks for playing", _exitNo = "returning to the game";
        private string _menuMessage = "Are you sure you want to return to the menu", _menuCaption = "Confirm Menu", _menuYes = "going back to menu", _menuNo = "returning to the game";
        private short _max, _addAtSortedWay, _numOfCurrentmax, _checkEveryPlayer;
        private List<Player> _backUpList = new List<Player>();
        private List<Label> _Ranks = new List<Label>()
            {
                new Label{Content="🥇 First",Foreground=new SolidColorBrush(Colors.Gold)},
                new Label{Content="🥈 Second",Foreground=new SolidColorBrush(Colors.Silver)},
                new Label{Content="🥉 third",Foreground=new SolidColorBrush(Colors.Brown)},
                new Label{Content="Fourth"},
                new Label{Content="Fifth"},
                new Label{Content="Sixth"}
            };
        public Celebration(List<Player> Data,short players)
        {
            InitializeComponent();

            foreach (Label addLable in _Ranks)
            {
                ranks.Items.Add(addLable);
            }
            for(int deleteAt=5;deleteAt>=players;deleteAt--) ranks.Items.RemoveAt(deleteAt);

            short count = (short)Data.Count;
            Player backUpPlayer = Data[0];
            foreach (Player player in Data) player.SetTrash(true);   //sort the players by their _max place value, after player declared as having the _max value we'll make this variable difference and will ignore it
            for (_checkEveryPlayer = 0; _checkEveryPlayer < count; _checkEveryPlayer++)
                {
                _max = 0;
                _addAtSortedWay = 0;
                _numOfCurrentmax = 0;
                foreach (Player player in Data)
                {
                    if (player.Place >= _max && player.GetTrash())
                    {
                        _max = player.Place;
                        backUpPlayer = player;
                        _addAtSortedWay = _numOfCurrentmax;
                    }
                    _numOfCurrentmax++;
                }
                _backUpList.Add(backUpPlayer);
                Data[_addAtSortedWay].SetTrash(false);
            }
            EndGame.ItemsSource = _backUpList;
        }

        private void gif_MediaEnded(object sender, RoutedEventArgs e)
        {
            gif.Position = TimeSpan.FromMilliseconds(1);
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
            }
        }
        private void back_Click(object sender, RoutedEventArgs e) { HomeConfirm(); }   //if player press menu button return to the menu 
        private void exit_Click(object sender, RoutedEventArgs e) { ExitConfirm(); }  //if player press the exit button exit the game
        private void ExitConfirm()
        {
            object var = MessageBox.Show(_exitMessage, _exitCaption, MessageBoxButton.YesNo);
            switch (var)
            {
                case MessageBoxResult.Yes:
                    MessageBox.Show(_exitYes);
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show(_exitNo);
                    break;

            }
        }
        private void HomeConfirm()
        {
            object var = MessageBox.Show(_menuMessage, _menuCaption, MessageBoxButton.YesNo);
            switch (var)
            {
                case MessageBoxResult.Yes:
                    MessageBox.Show(_menuYes);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show(_menuNo);
                    break;

            }
        }
    }
}
