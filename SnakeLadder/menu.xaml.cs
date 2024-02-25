using gameLogic;
using GameLogic;
using GameLogic.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace SnakeLadder
{
    public partial class menu : Window
    {
        Decoration decoration = new Decoration();
        PreGame pregameLogic = new PreGame();

        private static Random _random = new Random();
        
        private static short _turn = 1;   //keep tracks of who's turn is it 
        private static short _randomRocketBomb;    //used to contain the random amount of spaces the rocket/bomb send you
        private static short _currentRow, _futureRow, _currentColumn, _futureColumn, _currentPlace, _nextPlace;

        private static bool _bombFlag = false, _boostFlag = false;   //if player land on bomb raise flag
        private static bool _ricochet100Flag = false, _winFlag = false;   //flag raised to determain if the player surpass 100 and call relative animation


        private string _rollText = "You can roll", _waitText = "Wait";
        private string _exitMessage = "Are you sure you want to exit", _exitCaption = "Conform Exit", _exitYes = "thanks for playing", _exitNo = "returning to the game";
        private string _menuMessage = "Are you sure you want to return to the menu", _menuCaption = "Confirm Menu", _menuYes = "going back to menu", _menuNo = "returning to the game";

        private void How_much_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pregameLogic.How_much_SelectionChanged(sender, e, ref How_much, ref keep_data, ref select_players, ref info);
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            pregameLogic.info_Click(sender, e, ref info, ref keep_data, ref Dice, ref turn_text);
            short setPlayersAt0 = 0;
            foreach (Player irrelevant in pregameLogic.playerData)
            {
                Game_Grid.Children.Remove(pregameLogic.playerData[setPlayersAt0].GetBlock());
                Grid.SetRow(pregameLogic.playerData[setPlayersAt0].GetBlock(), 1);
                Grid.SetColumn(pregameLogic.playerData[setPlayersAt0].GetBlock(), 0);
                Game_Grid.Children.Add(pregameLogic.playerData[setPlayersAt0].GetBlock());
                setPlayersAt0++;
            }
            RollOrWait.Content = _rollText;

        }

        public menu()
        {
            InitializeComponent();
            ColorTable();   // create the playing board
            How_much.SelectedIndex = 0 ;
            this.KeyDown += MainWindow_KeyDown;   //access responsive key game
        }

        private void Dice_Click(object sender, RoutedEventArgs e)
        {
            RollOrWait.Content = _waitText;
            Bomb_rocket_text.Text = string.Empty;
            short rolled = (short)_random.Next(1, 7);   //present a cube throw
            DiceOutput(rolled);   //the random number showed to the player
            _currentPlace = (short)pregameLogic.playerData[_turn - 1].Place;
            pregameLogic.playerData[_turn - 1].Place += rolled;//advance the player relatively to his throw

            if (pregameLogic.playerData[_turn - 1].Place > 100)
            {
                _ricochet100Flag = true;
                pregameLogic.playerData[_turn - 1].Place = (short)(100 - (pregameLogic.playerData[_turn - 1].Place - 100));    //if the player's roll surpass 100 he'll go back the amount he went over
            }
            else if (pregameLogic.playerData[_turn - 1].Place == 100)   //if the player land on 100 he wins
            {
                _winFlag = true;
            }
            _nextPlace = (short)pregameLogic.playerData[_turn - 1].Place;

            
            _currentRow = gridInfo()[0];
            _futureRow = gridInfo()[1];
            _currentColumn = gridInfo()[2];
            _futureColumn = gridInfo()[3];

            ColumnRowAnimation(sender, e);   //call the animation
        }
        private List<short> gridInfo() //contains where every player needs to go according to the roll
        {
            //every row is 10 spaces, every row is from column 1-10
            _currentRow = (short)(_currentPlace == 0 ? 1 : _currentPlace % 10 != 0 ? 1 + _currentPlace / 10 : _currentPlace / 10);
            _futureRow = (short)(_nextPlace % 10 != 0 ? 1 + _nextPlace / 10 : _nextPlace / 10);
            // place=0 => row=1 // place=1 => row= 1+1/10 = 1+0= 1 // place=10 => row= 10/10 = 1

            //every column is 10 spaces, every colomn is from row 1-10, every 2nd row the direction is changed -> example:1st row goes left to right, 2nd row goes from right to left 
            _currentColumn = (short)(_currentPlace == 0 ? 0 : _currentRow % 2 != 0 && _currentPlace % 10 == 0 ? 10 : _currentRow % 2 == 0 && _currentPlace % 10 == 0 ? 1 : _currentRow % 2 == 0 && _currentPlace % 10 != 0 ? 11 - (_currentPlace % 10) : _currentPlace % 10);
            _futureColumn = (short)(_futureRow % 2 != 0 && _nextPlace % 10 == 0 ? 10 : _futureRow % 2 == 0 && _nextPlace % 10 == 0 ? 1 : _futureRow % 2 == 0 && _nextPlace % 10 != 0 ? 11 - (_nextPlace % 10) : _nextPlace % 10);
            // place=0 => column=0 // place=10 => row=1 => column=10  // place=20 => row=2 => column=1 // place=11 => row=2 => column= 11-(11%10) = 11-1 = 10 // place=1 => column= 1%10 = 1                                                                                                                                                                                              //example:10-> 10's row is 1 using the row's formula, 1%2!=0 and 10%10=0 -> column is set to 10

            return new List<short> { _currentRow, _futureRow, _currentColumn, _futureColumn };
        }  

        private void Winner(object sender, EventArgs e)
        {
            MessageBox.Show($"player {_turn}. {pregameLogic.playerData[_turn - 1].Name} won");
            Celebration celebration = new Celebration(pregameLogic.playerData, pregameLogic.GetPlayers());
            this.Close();   // close current window
            celebration.Show();   // goes to celebration screen
        }

        private void ColumnMove_Completed(object sender, EventArgs e)
        {
            Int32Animation rowMove = new Int32Animation
            {
                From = _currentRow,
                Duration = TimeSpan.FromMilliseconds(100)
            };
            if (_bombFlag) rowMove.To = _currentRow - 1;
            else rowMove.To = _currentRow + 1;
            _currentRow = (short)rowMove.To;
            rowMove.Completed += ColumnRowAnimation;
            pregameLogic.playerData[_turn - 1].GetBlock().BeginAnimation(Grid.RowProperty, rowMove);
        }

        private void ColumnRowAnimation(object sender, EventArgs e)
        {
            Dice.IsEnabled = false;   //while the animation running the dice button isn't available
            Int32Animation columnMove = new Int32Animation
            {
                From = _currentColumn,
                To = _futureColumn,
                Duration = TimeSpan.FromMilliseconds(350)
            };

            if (_winFlag)
            {
                columnMove.Completed += Winner;
                _winFlag = false;
            }

            else if (_ricochet100Flag)
            {
                _ricochet100Flag = false;
                columnMove.To = 1;
                columnMove.Completed += ColumnRowAnimation;
            }

            else if (_currentRow != _futureRow)
            {
                if (_bombFlag) { if (_currentRow % 2 == 0) columnMove.To = 10; else columnMove.To = 1; }
                else { if (_currentRow % 2 == 1) columnMove.To = 10; else columnMove.To = 1; }
                columnMove.Completed += ColumnMove_Completed;
            }

            else columnMove.Completed += CheckForExtas;   //after player got to ouhisr final place check if he land on a bomb/rocket
            _currentColumn = (short)columnMove.To;
            pregameLogic.playerData[_turn - 1].GetBlock().BeginAnimation(Grid.ColumnProperty, columnMove);
        }


        
        private void CheckForExtas(object sender, EventArgs e)
        {
            short total = 0;   //count the total spaces player gained/lost cause of bombs/rockets
            _boostFlag = false;   //if player land on rocket raise flag 
            _bombFlag = false;
            short[] bombRocketPlaces = { 4, 23, 29, 44, 63, 71, 15, 72, 81, 94, 98 };
            for (short checkForBombRocket = 0; checkForBombRocket < bombRocketPlaces.Count(); checkForBombRocket++) if (_nextPlace == bombRocketPlaces[checkForBombRocket])
                {
                    Thread.Sleep(100);
                    if (checkForBombRocket < 6) //landing on rocket
                    {
                        _randomRocketBomb = (short)_random.Next(1, 7);
                        _boostFlag = true;
                    }
                    else
                    {
                        _randomRocketBomb = (short)_random.Next(-12, 0);
                        _bombFlag = true;
                    }
                    total += _randomRocketBomb;
                    _currentPlace = (short)pregameLogic.playerData[_turn - 1].Place;
                    pregameLogic.playerData[_turn - 1].Place += _randomRocketBomb;
                    _nextPlace = (short)pregameLogic.playerData[_turn - 1].Place;
                    break;
                }

            //give the player relative message about how much he gain/lost from landing on bombs/rockets
            Bomb_rocket_text.Text = _bombFlag ? $"Player {_turn} hit bombs. in total lose {Math.Abs(total)} steps" : _boostFlag ? $"Player {_turn} hit boosts. in total gain {total} steps" : Bomb_rocket_text.Text;
           
            if (_boostFlag || _bombFlag)   //falg raised mean the player land on bomb/rocket and now he's in difference place, therefore we'll need to check if the player land on another bomb/rocket 
            {
                _currentRow = gridInfo()[0];
                _futureRow = gridInfo()[1];
                _currentColumn = gridInfo()[2];
                _futureColumn = gridInfo()[3];
                ColumnRowAnimation(sender, e);
            }
            else _next_turn_values(sender, e);
        }

        private void _next_turn_values(object sender, EventArgs e)
        {
            RollOrWait.Content = _rollText;
            Dice.IsEnabled = true;
            _turn = (short)(_turn == pregameLogic.GetPlayers() ? 1 : _turn + 1);
                //  player=3, turn=3, next turn=1  // player=3, turn=2, next turn=3
            turn_text.Text = $"Player {_turn}'s turn";
        }
 

        private void DiceOutput(short roll) { decoration.DiceOutput(ref roll, ref Imagin); }   //showing suitable picture according to the player's roll
        private void ColorTable() { decoration.ColorTable(Game_Grid); }   //since we have the movement path of the game, it's possible to set the movement to 1 space and fill each space with decoration
        private void MainWindow_KeyDown(object sender,KeyEventArgs e)   //once a key pressed enter the method
        {
            switch (e.Key)
            {
                case Key.Escape:   //if player press esc ask him if he's sure he want to exit the game
                    ExitConfirm();
                    break;
                case Key.Home:   //if player press home ask him if he's sure he want to return to the menu
                    HomeConfirm();
                    break;
                case Key.Tab:
                    if (Dice.IsEnabled) Dice_Click(sender, e);
                    break;
            }
        }
        private void back_Click(object sender, RoutedEventArgs e) { HomeConfirm(); }  //if player press menu button return to the menu 
        private void exit_Click(object sender, RoutedEventArgs e) { ExitConfirm(); }//if player press the exit button exit the game
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
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
            {
                comboBox.IsEnabled = false;
                foreach (Player player in pregameLogic.playerData) player.charactersHere.Remove(comboBox.SelectedItem.ToString());
            }
        }
    }
}

