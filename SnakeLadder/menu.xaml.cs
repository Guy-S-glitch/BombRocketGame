﻿using gameLogic;
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
        //AnimationLogic animationLogic = new AnimationLogic();
        
        private static int turn = 1;   //keep tracks of who's turn is it 
        private static int randomRocketBomb;    //used to contain the random amount of spaces the rocket/bomb send you
        
        private static int currentRow, futureRow, currentColumn, futureColumn;
        private void How_much_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pregameLogic.How_much_SelectionChanged(sender, e, ref How_much, ref keep_data, ref select_players, ref info);
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            pregameLogic.info_Click(sender,e, ref info,ref keep_data, ref Dice, ref turn_text);
            int initial = 0;
            foreach (Player iniplayer in pregameLogic.playerData)
            {
                GridPP.Children.Remove(pregameLogic.playerData[initial].TextBlock);
                Grid.SetRow(pregameLogic.playerData[initial].TextBlock, 1);
                Grid.SetColumn(pregameLogic.playerData[initial].TextBlock, 0);
                GridPP.Children.Add(pregameLogic.playerData[initial].TextBlock);
                initial++;
            }
            RollOrWait.Content = "You can roll";

        }
        private static Random random = new Random();
        private static int currentPlace, nextPlace;
        public menu()
        {
            InitializeComponent();
            ColorTable();   // create the playing board
            How_much.SelectedIndex = 0;
            this.KeyDown += MainWindow_KeyDown;   //access responsive key game
        }
        private static bool TooMuch = false;   //flag raised to determain if the player surpass 100 and call relative animation
        private static bool winner = false;
        private void Dice_Click(object sender, RoutedEventArgs e)
        {
            RollOrWait.Content = "Wait";
            Bomb_rocket_text.Text = "";
            int rolled = random.Next(1, 7);   //present a cube throw
            DiceOutput(rolled);   //the random number showed to the player
            currentPlace = pregameLogic.playerData[turn - 1].Place;
            pregameLogic.playerData[turn - 1].Place += rolled;//advance the player relatively to his throw
            
            if (pregameLogic.playerData[turn - 1].Place > 100)
            {
                TooMuch = true;
                pregameLogic.playerData[turn - 1].Place = 100 - (pregameLogic.playerData[turn - 1].Place - 100);    //if the player's roll surpass 100 he'll go back the amount he went over
            }
            else if (pregameLogic.playerData[turn - 1].Place == 100)   //if the player land on 100 he wins
            {
                winner = true;
            }
            nextPlace = pregameLogic.playerData[turn - 1].Place;

           
            currentRow = gridInfo()[0];
            futureRow = gridInfo()[1];
            currentColumn = gridInfo()[2];
            futureColumn = gridInfo()[3];

            ColumnRowAnimation(sender, e);   //call the animation
        }
        private List<int> gridInfo()
        {
            int currentRow, futureRow, currentColumn, futureColumn;
            currentRow =    //every row is 10 spaces, every row is from column 1-10
                 currentPlace == 0 ? 1    //at the start the current place is 0 and the row of 0 is 1
               : currentPlace % 10 != 0 ? 1 + currentPlace / 10    //example: player on place 8-> 8/10=0 but we start our column from 1 therefore we'll add 1 to match our board-> 1+8/10=1+0-> row=1
               : currentPlace / 10;   //example: player land on 10-> 10/10=1, since every row is 10 spaces, in this case, it will be wrong to add 1-> 10/10=1 -> row=1

            futureRow =
                 nextPlace % 10 != 0 ? 1 + nextPlace / 10
                 : nextPlace / 10;


            currentColumn =   //every column is 10 spaces, every colomn is from row 1-10, every 2nd row the direction is changed-> example:1st row goes left to right, 2nd row goes from right to left 
                currentPlace == 0 ? 0   //at the start the current place is 0 and the column of 0 is 0
              : currentRow % 2 != 0 && currentPlace % 10 == 0 ? 10   //example:10-> 10's row is 1 using the row's formula, 1%2!=0 and 10%10=0 -> column is set to 10
              : currentRow % 2 == 0 && currentPlace % 10 == 0 ? 1   //example:20-> 20's row is 2, 2%2=0 and 20%10=0 -> column is set to 1
              : currentRow % 2 == 0 && currentPlace % 10 != 0 ? 11 - (currentPlace % 10)   //example:11-> 11's row is 2, 2%2=0 and 11%10=1 -> since every 2nd row the path is from right to left we'll count from the right ->11-(11%10)=11-1-> column=10    
              : currentPlace % 10;   //example:1-> 1's row is 1, 1%2!=0 and 1%10=1 -> column=1


            futureColumn =
                futureRow % 2 != 0 && nextPlace % 10 == 0 ? 10
                : futureRow % 2 == 0 && nextPlace % 10 == 0 ? 1
                : futureRow % 2 == 0 && nextPlace % 10 != 0 ? 11 - (nextPlace % 10)
                : nextPlace % 10;
           
            return new List<int> { currentRow, futureRow, currentColumn, futureColumn };
        }   //contains where every player needs to go according to the roll

        private void Winner(object sender,EventArgs e)
        {
            MessageBox.Show($"player {turn}. {pregameLogic.playerData[turn - 1].Name} won");
            Celebration celebration = new Celebration(pregameLogic.playerData, pregameLogic.players);
            this.Close();   // close current window
            celebration.Show();   // goes to celebration screen
        }
        private void ColumnRowAnimation(object sender, EventArgs e)
        {
            //animationLogic.ColumnRowAnimationData(ref Dice, ref currentRow, ref futureRow, ref currentColumn, ref futureColumn, ref TooMuch, ref turn, ref bombFlag);
            Dice.IsEnabled = false;   //while the animation running the dice button isn't available
            if (winner)    //player land on the 100 space
            {
                Int32Animation columnMove = new Int32Animation
                {
                    From = currentColumn,
                    To = futureColumn,
                    Duration = TimeSpan.FromMilliseconds(350)
                };
                columnMove.Completed += Winner;   

                pregameLogic.playerData[turn - 1].TextBlock.BeginAnimation(Grid.ColumnProperty, columnMove);
            }
            else if (currentRow == futureRow && !TooMuch) //same row
            {
                Int32Animation columnMove = new Int32Animation
                {
                    From = currentColumn,
                    To = futureColumn,
                    Duration = TimeSpan.FromMilliseconds(350)
                };
                columnMove.Completed += CheckForExtas;   //after player got to ouhisr final place check if he land on a bomb/rocket

                pregameLogic.playerData[turn - 1].TextBlock.BeginAnimation(Grid.ColumnProperty, columnMove);
            }
            else if (TooMuch)   //animation for ricocheting 100 if went over
            {
                Int32Animation MoveTo100 = new Int32Animation
                {
                    From = currentColumn,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(350)
                };
                TooMuch = false;
                currentColumn = (int)MoveTo100.To;
                MoveTo100.Completed += ColumnRowAnimation;    //after touching 100 go to your actual place
                pregameLogic.playerData[turn - 1].TextBlock.BeginAnimation(Grid.ColumnProperty, MoveTo100);
            }
            else if (!bombFlag) //difference rows and direction is positive toward the finish
            {
                if (currentRow % 2 == 1)//direction from left to right
                {
                    Int32Animation columnMoveL2R = new Int32Animation
                    {
                        From = currentColumn,
                        To = 10,
                        Duration = TimeSpan.FromMilliseconds(350)
                    };
                    columnMoveL2R.Completed += ColumnMove_Completed;
                    currentColumn = (int)columnMoveL2R.To;
                    pregameLogic.playerData[turn - 1].TextBlock.BeginAnimation(Grid.ColumnProperty, columnMoveL2R);

                }
                else//direction from right to left
                {
                    Int32Animation columnMoveR2L = new Int32Animation
                    {
                        From = currentColumn,
                        To = 1,
                        Duration = TimeSpan.FromMilliseconds(350)
                    };
                    columnMoveR2L.Completed += ColumnMove_Completed;
                    currentColumn = (int)columnMoveR2L.To;
                    pregameLogic.playerData[turn - 1].TextBlock.BeginAnimation(Grid.ColumnProperty, columnMoveR2L);

                }
            }
            else     //direction is negative away from the finish
            {
                if (currentRow % 2 == 0)//direction from right to left
                {
                    Int32Animation columnMoveL2R = new Int32Animation
                    {
                        From = currentColumn,
                        To = 10,
                        Duration = TimeSpan.FromMilliseconds(350)
                    };
                    columnMoveL2R.Completed += ColumnMove_Completed;
                    currentColumn = (int)columnMoveL2R.To;
                    pregameLogic.playerData[turn - 1].TextBlock.BeginAnimation(Grid.ColumnProperty, columnMoveL2R);

                }
                else//direction from left to right
                {
                    Int32Animation columnMoveR2L = new Int32Animation
                    {
                        From = currentColumn,
                        To = 1,
                        Duration = TimeSpan.FromMilliseconds(350)
                    };
                    columnMoveR2L.Completed += ColumnMove_Completed;
                    currentColumn = (int)columnMoveR2L.To;
                    pregameLogic.playerData[turn - 1].TextBlock.BeginAnimation(Grid.ColumnProperty, columnMoveR2L);

                }
            }
        }
        private bool bombFlag = false;   //if player land on bomb raise flag
        private void CheckForExtas(object sender, EventArgs e)
        {
            int total = 0;   //count the total spaces player gained/lost cause of bombs/rockets
            bool boostFlag = false;   //if player land on rocket raise flag
            bool falg=false;   //if player land on anything raise flag 
            bombFlag = false;
            List<int> boost = new List<int>() { 4, 23, 29, 44, 63, 71 };   //places of every rocket on the board
            foreach (int placeBoost in boost) if (placeBoost == nextPlace)
                {
                    //if player land on rocket show the character  on the rocket before moving him
                    Thread.Sleep(100);
                    randomRocketBomb = random.Next(1, 7);   //landing on rocket makes you gain randomly between 1-6 spaces
                    total += randomRocketBomb;
                    currentPlace = pregameLogic.playerData[turn - 1].Place;     
                    pregameLogic.playerData[turn - 1].Place += randomRocketBomb;
                    nextPlace = pregameLogic.playerData[turn - 1].Place;
                    falg = true;
                    boostFlag = true;
                    break;  
                }
            
            List<int> Bomb = new List<int>() { 15, 72, 81, 94, 98 };   //places of every bomb on the board
            if(!falg)   //if player land on rocket skip the bomb check
            foreach (int placeBomb in Bomb) if (placeBomb == pregameLogic.playerData[turn - 1].Place)
                {
                    //if player land on bomb show the character  on the bomb before moving him
                    Thread.Sleep(100);
                    randomRocketBomb = random.Next(1, 13);   //landing on bomb makes you lose randomly between 1-12 spaces
                    total -= randomRocketBomb;
                    currentPlace = pregameLogic.playerData[turn - 1].Place;
                    pregameLogic.playerData[turn - 1].Place -= randomRocketBomb;
                    nextPlace = pregameLogic.playerData[turn - 1].Place;
                    falg = true;
                    bombFlag = true;
                    break;
                }


            //give the player relative message about how much he gain/lost from landing on bombs/rockets
            Bomb_rocket_text.Text = bombFlag ? $"Player {turn} hit bombs. in total lose {Math.Abs(total)} steps"
                : boostFlag ? $"Player {turn} hit boosts. in total gain {total} steps": Bomb_rocket_text.Text;
            if (falg)//falg raised mean the player land on bomb/rocket and now he's in difference place, therefore we'll need to check if the player land on another bomb/rocket 
            {
                currentRow = gridInfo()[0];
                futureRow = gridInfo()[1];
                currentColumn = gridInfo()[2];
                futureColumn = gridInfo()[3];
                ColumnRowAnimation(sender, e);
            }
            else next_turn(sender, e);

        }
        private void next_turn(object sender, EventArgs e)
        {
            RollOrWait.Content = "You can roll";
            Dice.IsEnabled = true;
            turn = turn == pregameLogic.players ? 1    //if it's the last player's turn then next turn returned to the first player-> 3 players, it's player 3's turn, next turn will be player 1
    : turn + 1;   //else it's the next player turn -> 3 players, it's player 2's turn, next turn will be player 3
            turn_text.Text = $"Player {turn}'s turn";
        }

        private void ColumnMove_Completed(object sender, EventArgs e)
        {
            if (!bombFlag)
            {
                Int32Animation rowMove = new Int32Animation
                {
                    From = currentRow,
                    To = currentRow + 1,
                    Duration = TimeSpan.FromMilliseconds(100)
                };
                currentRow++;
                rowMove.Completed += ColumnRowAnimation;
                pregameLogic.playerData[turn - 1].TextBlock.BeginAnimation(Grid.RowProperty, rowMove);
            }
            else
            {
                Int32Animation rowMove = new Int32Animation
                {
                    From = currentRow,
                    To = currentRow -1 ,
                    Duration = TimeSpan.FromMilliseconds(100)
                };
                currentRow--;
                rowMove.Completed += ColumnRowAnimation;
                pregameLogic.playerData[turn - 1].TextBlock.BeginAnimation(Grid.RowProperty, rowMove);
            }
        }


        private void DiceOutput(int roll)   //showing suitable picture according to the player's roll
        {
            decoration.DiceOutput(ref roll, ref Imagin);
        }      

        private void ColorTable()   //since we have the movement path of the game, it's possible to set the movement to 1 space and fill each space with decoration
        {
            decoration.ColorTable(GridPP);
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)   //once a key pressed enter the method
        {
            switch (e.Key)
            {
                case Key.Escape:   //if player press esc ask him if he's sure he want to exit the game
                    ExitConform();
                    break;
                case Key.Home:   //if player press home ask him if he's sure he want to return to the menu
                    HomeConform();
                    break;
                case Key.Tab:
                    if (Dice.IsEnabled) Dice_Click(sender, e);
                    break;
                default: break;
            }
        }
        private void back_Click(object sender, RoutedEventArgs e)   //if player press menu button return to the menu 
        {
            HomeConform();
        }
        private void exit_Click(object sender, RoutedEventArgs e)   //if player press the exit button exit the game
        {
            ExitConform();
        }
        private void ExitConform()
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
        private void HomeConform()
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
            {
                comboBox.IsEnabled = false;
                
                foreach (Player player in pregameLogic.playerData)
                {
                    player.charactersHere.Remove(comboBox.SelectedItem.ToString());
                }
            }
           
        }
    }
}
