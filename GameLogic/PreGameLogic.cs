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
    public class PreGame
    {
        public List<string> strings = new List<string>() { "🐱", "🐼", "🐻", "🐨", "🐮", "🐷", "🐹", "🐭", "🐰", "🐵", "🐶" };   //after failing to get the character  directly from the window the best option is to get the index of the chosen character  and compare it with list that will have the same values
        public List<Player> playerData = new List<Player>();      //keeps the data about every player
        public int players;
        public void How_much_SelectionChanged(object sender, SelectionChangedEventArgs e,ref ComboBox How_much,ref ListBox here,ref StackPanel sele,ref Button info)
        {
            if (How_much.SelectedIndex != 0)   //the only way to set how many player are there is to choose item with number value
            {
                players = How_much.SelectedIndex;
                How_much.IsEnabled = false;   //after the amount of players has chosen this combobox is unrelevent
                //lablePlayers.Content = $"{players} players chosen";
                for (int i = 0; i < players; i++) playerData.Add(new Player(i + 1, 0, new TextBlock()));   //initialize the stats of evert player
                here.ItemsSource = playerData;   //after we filled the needed stats we'll send the information to a listbox that will ask the players for the remaining needed information
                
                int left = 0;
                int bottom = 0;
                for (int padd = 0; padd < players; padd++)
                {
                    playerData[padd].TextBlock.Padding = new Thickness(left, 0, 0, bottom);   //set a specipic place for every player so in case multiple player will be on the same box they could see their character 
                                                                                                        //since 6 players is the maximum amount of players we can have
                    left += 25;
                    left = padd == 2 ? 0 : left;                  //order will be: 1)Left   2)Middle 3)Rigth  4)Left 5)Middle 6)Rigth
                    bottom = padd == 2 ? bottom + 25 : bottom;    //order will be: 1)Bottom 2)Bottom 3)Bottom 4)Top  5)Top    6)Top

                    //minimum height of the screen will have to be: 10+(10*minimumHeightOfOneBlock)+10>height -> minimumHeightOfOneBlock=2*25=50 -> 10+10*50+10= 520 = minimum height
                    //minimum width of the screen will have to be: 150+(10*minimumWidthOfOneBlock)+150>width -> minimumWidthOfOneBlock=3*25=75 -> 150+(10*75)+150= 1050 = minimum width
                }
                How_much.Visibility = Visibility.Hidden;
                sele.Visibility = Visibility.Hidden;

                //after selecting how many playing now we need to know information on each player
                info.Visibility = Visibility.Visible;
                here.Visibility = Visibility.Visible;

            }
        }
        public void info_Click(object sender, RoutedEventArgs e, ref Button info, ref ListBox here, ref Button Dice, ref TextBlock its)
        { 
            
            try   //there will be an error if the player will send null info, thus we'll use try & catch
            {
                
                foreach (Player player in playerData)
                {
                    if (string.IsNullOrEmpty(player.Name) || string.IsNullOrEmpty(player.strIcons)) throw new ArgumentNullException();   //name is requried
                                                                                               
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
