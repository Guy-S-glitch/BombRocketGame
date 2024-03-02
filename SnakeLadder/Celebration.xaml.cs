using GameLogic.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SnakeLadder
{
    public partial class Celebration : Window
    {
        private static string _exitMessage = "Are you sure you want to exit", _exitCaption = "Conform Exit", _exitYes = "thanks for playing", _exitNo = "returning to the game";
        private static string _menuMessage = "Are you sure you want to return to the menu", _menuCaption = "Confirm Menu", _menuYes = "going back to menu", _menuNo = "returning to the game";
        private static string _first = "🥇 First", _second = "🥈 Second", _third = "🥉 third", _fourth = "Fourth", _fifth = "Fifth", _sixth = "Sixth";
        private static short _max, _addAtSortedWay, _numOfCurrentmax, _checkEveryPlayer;
        private List<Label> _Ranks = new List<Label>()
            {
                new Label{Content=_first,Foreground=new SolidColorBrush(Colors.Gold)},
                new Label{Content=_second,Foreground=new SolidColorBrush(Colors.Silver)},
                new Label{Content=_third,Foreground=new SolidColorBrush(Colors.Brown)},
                new Label{Content=_fourth},
                new Label{Content=_fifth},
                new Label{Content=_sixth}
            };
        public Celebration(List<Player> Data,short players)
        {
            InitializeComponent();
            addPositions(players);
            EndGame.ItemsSource = sortPlayersPosition(Data);
        }
        private static List<Player> sortPlayersPosition(List<Player> Data)
        {
            List<Player> _backUpList = new List<Player>();
            short count = (short)Data.Count;
            Player backUpPlayer = Data[0];
            // Sort the players by their _max place value, after player declared as having the _max value we'll make this variable difference and will ignore it
            foreach (Player player in Data) player.SetTrash(true);   
            for (_checkEveryPlayer = 0; _checkEveryPlayer < count; _checkEveryPlayer++)
            {
                _max = 0; _addAtSortedWay = 0; _numOfCurrentmax = 0;
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
            return _backUpList;
        }
        private void addPositions(short players)
        {
            foreach (Label addLable in _Ranks)
            {
                ranks.Items.Add(addLable);
            }
            for (int deleteAt = 5; deleteAt >= players; deleteAt--) ranks.Items.RemoveAt(deleteAt);
        }
        private void gif_MediaEnded(object sender, RoutedEventArgs e) { gif.Position = TimeSpan.FromMilliseconds(1); }


        // Once a key pressed enter the method
        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)   
        {
            switch (e.Key)
            {
                case Key.Escape:
                    // If player press esc ask him if he's sure he want to exit the gam
                    ExitConfirm();
                    break;
                case Key.Home:
                    // If player press home ask him if he's sure he want to return to the menu
                    HomeConfirm();
                    break;
            }
        }

        // If player press menu button return to the menu
        private void back_Click(object sender, RoutedEventArgs e) 
        { 
            HomeConfirm();
        }

        // If player press the exit button exit the game
        private void exit_Click(object sender, RoutedEventArgs e) 
        {
            ExitConfirm();
        }  
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
