using gameLogic;
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
    public partial class menu : Window
    {
        MainGameLogic gameLogic = new MainGameLogic();
        private static int players;
        private int turn = 1;   //keep tracks of who's turn is it 
        private static int boot;
        private void How_much_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (How_much.SelectedIndex != 0)   //the only way to set how many player are there is to choose item with number value
            {
                players = How_much.SelectedIndex;
                How_much.IsEnabled = false;   //after the amount of players has chosen this combobox is unrelevent
                lablePlayers.Content = $"{players} players chosen";
                for (int i = 0; i < players; i++) gameLogic.playerData.Add(new Player(i + 1, 0, new TextBlock()));   //initialize the stats of evert player
                here.ItemsSource = gameLogic.playerData;   //after we filled the needed stats we'll send the information to a listbox that will ask the players for the remaining needed information
                int left = 0;
                int bottom = 0;
                for (int padd = 0; padd < players; padd++)
                {
                    gameLogic.playerData[padd].TextBlock.Padding = new Thickness(left, 0, 0, bottom);   //set a specipic place for every player so in case multiple player will be on the same box they could see their charactor
                       //since 6 players is the maximum amount of players we can have
                    left += 25;
                    left = padd == 2 ? 0 : left;                  //order will be: 1)Left   2)Middle 3)Rigth  4)Left 5)Middle 6)Rigth
                    bottom = padd == 2 ? bottom + 25 : bottom;    //order will be: 1)Bottom 2)Bottom 3)Bottom 4)Top  5)Top    6)Top
                    
                       //minimum height of the screen will have to be: 10+(10*minimumHeightOfOneBlock)+10>height -> minimumHeightOfOneBlock=2*25=50 -> 10+10*50+10= 520 = minimum height
                       //minimum width of the screen will have to be: 150+(10*minimumWidthOfOneBlock)+150>width -> minimumWidthOfOneBlock=3*25=75 -> 150+(10*75)+150= 1050 = minimum width
                }
                How_much.Visibility = Visibility.Hidden;

                   //after selecting how many playing now we need to know information on each player
                info.Visibility = Visibility.Visible;
                here.Visibility = Visibility.Visible;
            }
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            gameLogic.info_Click(sender,e, ref info,ref  here, ref Dice, ref its);
            //   //after failing to get the charactor directly from the window the best option is to get the index of the chosen charactor and compare it with list that will have the same values
            //List<string> strings = new List<string>() { "🐱", "🐼", "🐻", "🐨", "🐮", "🐷", "🐹", "🐭", "🐰", "🐵", "🐶" };
            //try   //there will be an error if the player will send null info, thus we'll use try & catch
            //{
            //    foreach (Player player in gameLogic.playerData)   
            //    {
            //        if (string.IsNullOrEmpty(player.Name)) throw new ArgumentNullException();   //name is requried 
            //        player.strIcons = strings[player.intIcons].ToString();   //turning the charactor from number to their simbol
            //        player.TextBlock.Text = player.strIcons;
            //    }
            //       //after reciving the needed info these boxes isn't necessary
            //    info.Visibility = Visibility.Hidden;
            //    here.Visibility = Visibility.Hidden;

            //    Dice.IsEnabled = true;   //make the dice clickable
            //    its.Text = "Player 1's turn";   //since it's the start, it's player 1's turn
            //}
            //catch
            //{
            //    MessageBox.Show("fill everything please");
            //}
        }

        public menu()
        {
            InitializeComponent();
            ColorTable();   // create the playing board
            How_much.SelectedIndex = 0;
            this.KeyDown += MainWindow_KeyDown;   //access responsive key game
        }

        private void Dice_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int rolled = random.Next(1, 7);   //present a cube throw
            dikk(rolled);   //the random number showed to the player
            gameLogic.playerData[turn - 1].Place += rolled;   //advance the player relatively to his throw
            if (gameLogic.playerData[turn - 1].Place > 100) gameLogic.playerData[turn - 1].Place = 100 - (gameLogic.playerData[turn - 1].Place - 100);   //if the player's roll surpass 100 he'll go back the amount he went over
            else if (gameLogic.playerData[turn - 1].Place == 100)   //if the player land on 100 he wins
            {
                MessageBox.Show($"player {turn}. {gameLogic.playerData[turn - 1].Name} won");
                MainWindow main = new MainWindow();
                this.Close();   // close current window
                main.Show();   // goes back to menu
            }
               //we can use both "playerData" and "turn" to use the data of the player currently rolling. since "turn" resets at 1 and list start from 0 we'll need to subtrack 1 from turn to have perfect connection
            int Row =    //every row is 10 spaces, every row is from column 1-10
                 gameLogic.playerData[turn - 1].Place % 10 != 0 ? 1 + gameLogic.playerData[turn - 1].Place / 10    //example: player on place 8-> 8/10=0 but we start our column from 1 therefore we'll add 1 to match our board-> 1+8/10=1+0-> row=1
               : gameLogic.playerData[turn - 1].Place / 10;   //example: player land on 10-> 10/10=1, since every row is 10 spaces, in this case, it will be wrong to add 1-> 10/10=1 -> row=1

            int Colom =   //every column is 10 spaces, every colomn is from row 1-10, every 2nd row the direction is changed-> example:1st row goes left to right, 2nd row goes from right to left 
                Row % 2 != 0 && gameLogic.playerData[turn - 1].Place % 10 == 0 ? 10   //example:10-> 10's row is 1 using the row's formula, 1%2!=0 and 10%10=0 -> column is set to 10
              : Row % 2 == 0 && gameLogic.playerData[turn - 1].Place % 10 == 0 ? 1   //example:20-> 20's row is 2, 2%2=0 and 20%10=0 -> column is set to 1
              : Row % 2 == 0 && gameLogic.playerData[turn - 1].Place % 10 != 0 ? 11 - (gameLogic.playerData[turn - 1].Place % 10)   //example:11-> 11's row is 2, 2%2=0 and 11%10=1 -> since every 2nd row the path is from right to left we'll count from the right ->11-(11%10)=11-1-> column=10    
              : gameLogic.playerData[turn - 1].Place % 10;   //example:1-> 1's row is 1, 1%2!=0 and 1%10=1 -> column=1

            int total = 0;   //count the total spaces player gained/lost cause of bombs/rockets
            bool bom = false;   //if player land on bomb raise flag
            bool boos = false;   //if player land on rocket raise flag
            bool falg;   //if player land on anything raise flag 

            do    // check once if the current player land on either bomb or rocket
            {
                falg = false;
                List<int> boost = new List<int>() { 4, 23, 29, 44, 63, 71 };   //places of every rocket on the board
                foreach (int placeBoost in boost) if (placeBoost == gameLogic.playerData[turn - 1].Place)
                    {
                           //if player land on rocket show the charactor on the rocket before moving him
                        GridPP.Children.Remove(gameLogic.playerData[turn - 1].TextBlock);
                        Grid.SetRow(gameLogic.playerData[turn - 1].TextBlock, Row);
                        Grid.SetColumn(gameLogic.playerData[turn - 1].TextBlock, Colom);
                        GridPP.Children.Add(gameLogic.playerData[turn - 1].TextBlock);

                        MessageBox.Show($"Player {turn} hit rocket");
                        boot = random.Next(1, 7);   //landing on rocket makes you gain randomly between 1-6 spaces
                        total += boot;
                        gameLogic.playerData[turn - 1].Place += boot;
                        falg = true;
                        bom = true;
                    }
                List<int> Bomb = new List<int>() { 15, 72, 81, 94, 98 };   //places of every bomb on the board
                foreach (int placeBomb in Bomb) if (placeBomb == gameLogic.playerData[turn - 1].Place)
                    {
                           //if player land on bomb show the charactor on the bomb before moving him
                        GridPP.Children.Remove(gameLogic.playerData[turn - 1].TextBlock);
                        Grid.SetRow(gameLogic.playerData[turn - 1].TextBlock, Row);
                        Grid.SetColumn(gameLogic.playerData[turn - 1].TextBlock, Colom);
                        GridPP.Children.Add(gameLogic.playerData[turn - 1].TextBlock);
                        MessageBox.Show($"Player {turn} hit bomb");
                        boot = random.Next(1, 13);   //landing on bomb makes you lose randomly between 1-12 spaces
                        total -= boot;
                        gameLogic.playerData[turn - 1].Place -= boot;
                        falg = true;
                        boos = true;
                    }
                   //in case the bomb/rocket cuased a change of row/column check their value again
                Row = gameLogic.playerData[turn - 1].Place % 10 != 0 ? 1 + gameLogic.playerData[turn - 1].Place / 10 : gameLogic.playerData[turn - 1].Place / 10;
                Colom = Row % 2 != 0 && gameLogic.playerData[turn - 1].Place % 10 == 0 ? 10 : Row % 2 == 0 && gameLogic.playerData[turn - 1].Place % 10 == 0 ? 1 : Row % 2 == 0 && gameLogic.playerData[turn - 1].Place % 10 != 0 ? 11 - (gameLogic.playerData[turn - 1].Place % 10) : gameLogic.playerData[turn - 1].Place % 10;

            } while (falg);   //falg raised mean the player land on bomb/rocket and now he's in difference place, therefore we'll need to check if the player land on another bomb/rocket 

               //give the player relative message about how much he gain/lost from landing on bombs/rockets
            if (total > 0)
                message1.Text = boos && bom ? $"Player {turn} hit bombs\nand boosts. in total\ngain {total} steps" :
                                              $"Player {turn} hit boosts.\n in total gain {total} steps";
            else if (total < 0)
                message1.Text = boos && bom ? $"Player {turn} hit bombs\nand boosts. in total\nlose {Math.Abs(total)} steps" :
                                              $"Player {turn} hit bombs.\n in total lose {Math.Abs(total)} steps";
            else
                message1.Text = boos && bom ? $"Player {turn} hit bombs\nand boosts. but stayed\nin his space" :
                                               "";
            
            GridPP.Children.Remove(gameLogic.playerData[turn - 1].TextBlock);
            Grid.SetRow(gameLogic.playerData[turn - 1].TextBlock, Row);
            Grid.SetColumn(gameLogic.playerData[turn - 1].TextBlock, Colom);
            GridPP.Children.Add(gameLogic.playerData[turn - 1].TextBlock);
            turn = turn == players ? 1    //if it's the last player's turn then next turn returned to the first player-> 3 players, it's player 3's turn, next turn will be player 1
                : turn + 1;   //else it's the next player turn-> 3 players, it's player 2's turn, next turn will be player 3
            its.Text = $"Player {turn}'s turn";
        }

        private void dikk(int roll)   //showing suitable picture according to the player's roll
        {
            switch (roll)
            {
                case 1:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_1.jpg", UriKind.Relative)); break;
                case 2:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_2.jpg", UriKind.Relative)); break;
                case 3:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_3.jpg", UriKind.Relative)); break;
                case 4:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_4.jpg", UriKind.Relative)); break;
                case 5:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_5.jpg", UriKind.Relative)); break;
                default:
                    Imagin.Source = new BitmapImage(new Uri("images\\Dice_6.jpg", UriKind.Relative)); break;
            }
        }      


        private void ColorTable()   //since we have the movement path of the game, it's possible to set the movement to 1 space and fill each space with decoration
        {
            int start = 0;
            while (start < 100)
            {
                Label label = new Label();
                label.Background = start % 2 == 0 ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(175,191,29)) : new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 153, 81));   //we'll have 2 colors fill the table, every space the color will change
                start += 1;
                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();   //make rainbow color
                myLinearGradientBrush.StartPoint = new System.Windows.Point(0, 0);
                myLinearGradientBrush.EndPoint = new System.Windows.Point(1, 1);
                myLinearGradientBrush.GradientStops.Add(
                    new GradientStop(Colors.Yellow, 0.0));
                myLinearGradientBrush.GradientStops.Add(
                    new GradientStop(Colors.Red, 0.25));
                myLinearGradientBrush.GradientStops.Add(
                    new GradientStop(Colors.Blue, 0.75));
                myLinearGradientBrush.GradientStops.Add(
                    new GradientStop(Colors.LimeGreen, 1.0));
                label.BorderThickness = new Thickness(1);
                label.BorderBrush= myLinearGradientBrush;   //paint the border with the colors we set
                
                label.Content = start;   //every space will have it's own number
                label.FontSize = 18;
                label.HorizontalContentAlignment = HorizontalAlignment.Left;
                label.VerticalContentAlignment=VerticalAlignment.Top;
                label.Foreground = new SolidColorBrush(Colors.GhostWhite);

                int Row = start % 10 != 0 ? 1 + start / 10 : start / 10;
                int Colom = Row % 2 != 0 && start % 10 == 0 ? 10 : Row % 2 == 0 && start % 10 == 0 ? 1 : Row % 2 == 0 && start % 10 != 0 ? 11 - (start % 10) : start % 10;
                Grid.SetRow(label, Row);
                Grid.SetColumn(label, Colom);
                GridPP.Children.Add(label);
            }
        }
        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)   //once a key pressed enter the method
        {
            switch (e.Key)
            {
                case Key.Escape:   //if player press esc exit the game
                    MessageBox.Show("thanks for playing");
                    this.Close();
                    break;
                case Key.Q:
                    MessageBox.Show("going back to menu");   //if player press Q return to menu
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                    break;
                default: break;
            }
        }
        private void exit_Click(object sender, RoutedEventArgs e)   //if player press the exit button exit the game
        {
            MessageBox.Show("thanks for playing");
            this.Close();
        }
        
        private void back_Click(object sender, RoutedEventArgs e)   //if player press menu button return to the menu 
        {
            MessageBox.Show("going back to menu");
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

    }
}
