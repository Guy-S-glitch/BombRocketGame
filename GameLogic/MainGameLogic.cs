using GameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace gameLogic
{
    public class MainGameLogic
    {
        public List<Player> playerData = new List<Player>();      //keeps the data about every player


        public void info_Click(object sender, RoutedEventArgs e, ref Button info, ref ListBox here, ref Button Dice, ref TextBlock its)
        {
            //after failing to get the charactor directly from the window the best option is to get the index of the chosen charactor and compare it with list that will have the same values
            List<string> strings = new List<string>() { "🐱", "🐼", "🐻", "🐨", "🐮", "🐷", "🐹", "🐭", "🐰", "🐵", "🐶" };
            try   //there will be an error if the player will send null info, thus we'll use try & catch
            {
                foreach (Player player in playerData)
                {
                    if (string.IsNullOrEmpty(player.Name)) throw new ArgumentNullException();   //name is requried 
                    player.strIcons = strings[player.intIcons].ToString();   //turning the charactor from number to their simbol
                    player.TextBlock.Text = player.strIcons;
                }
                //after reciving the needed info these boxes isn't necessary
                info.Visibility = Visibility.Hidden;
                here.Visibility = Visibility.Hidden;

                Dice.IsEnabled = true;   //make the dice clickable
                its.Text = "Player 1's turn";   //since it's the start, it's player 1's turn
            }
            catch
            {
                MessageBox.Show("fill everything please");
            }
        }

    }


}
