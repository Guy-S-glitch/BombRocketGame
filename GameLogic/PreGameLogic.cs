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
        public static short players,distanceFromLeft = 0, distanceFromBottom = 0;
        private string _turnText = "Player 1's turn", _catchText = "fill everything please";

        public void How_much_SelectionChanged(object sender, SelectionChangedEventArgs e,ref ComboBox How_much,ref ListBox keep_data, ref StackPanel select_players, ref Button info)
        {
            if (How_much.SelectedIndex != 0)   //the only way to set how many player are there is to choose item with number value
            {
                initialPlayerSelectStats(How_much, ref keep_data);
                placeForEveryCharacter();
                hideShow(ref How_much, ref select_players, ref info, ref keep_data);
            }
        }
        private void initialPlayerSelectStats(ComboBox How_much, ref ListBox keep_data)
        {
            players = (short)How_much.SelectedIndex;
            How_much.IsEnabled = false;   //after the amount of players has chosen this combobox is unrelevent
            for (short i = 0; i < players; i++) playerData.Add(new Player((short)(i + 1), 0, new TextBlock()));   //initialize the stats of evert player
            keep_data.ItemsSource = playerData;   //after we filled the needed stats we'll send the information to a listbox that will ask the players for the remaining needed information
        }
        private void placeForEveryCharacter()
        {
            for (short padd = 0; padd < players; padd++)
            {
                playerData[padd].GetBlock().Padding = new Thickness(distanceFromLeft, 0, 0, distanceFromBottom);   //set a specipic place for every player so in case multiple player will be on the same box they could see their character                                                                                                    //since 6 players is the maximum amount of players we can have
                distanceFromLeft += 25;
                distanceFromLeft = (short)(padd == 2 ? 0 : distanceFromLeft);                              //order will be: 1)Left   2)Middle 3)Rigth  4)Left 5)Middle 6)Rigth
                distanceFromBottom = (short)(padd == 2 ? distanceFromBottom + 25 : distanceFromBottom);    //order will be: 1)Bottom 2)Bottom 3)Bottom 4)Top  5)Top    6)Top
            }
        }
        private void hideShow(ref ComboBox How_much, ref StackPanel select_players, ref Button info, ref ListBox keep_data)
        {
            How_much.Visibility = Visibility.Hidden;
            select_players.Visibility = Visibility.Hidden;

            //after selecting how many playing now we need to know information on each player
            info.Visibility = Visibility.Visible;
            keep_data.Visibility = Visibility.Visible;
        }

        public short GetPlayers() { return players; }


        public void info_Click(object sender, RoutedEventArgs e, ref Button info, ref ListBox keep_data, ref Button Dice, ref TextBlock turn_text,ref Grid Game_Grid,ref Label RollOrWait,string _rollText)
        { 
            try   //there will be an error if the player will send null info, thus we'll use try & catch
            {
                CheckNullException(ref playerData);
                gameModeSetting(ref info, ref keep_data, ref Dice, ref turn_text);
                executeSetting(ref Game_Grid, ref RollOrWait, _rollText);
            }
            catch { MessageBox.Show(_catchText); }
        }

        private void CheckNullException(ref List<Player> playerData)
        {
            foreach (Player player in playerData)
            {
                if (string.IsNullOrEmpty(player.Name) || string.IsNullOrEmpty(player.strIcons)) throw new ArgumentNullException();   //name and icon is requried

                player.GetBlock().Text = player.strIcons;
            }
        }
        private void gameModeSetting(ref Button info, ref ListBox keep_data, ref Button Dice, ref TextBlock turn_text)
        {
            //after reciving the needed info these boxes isn't necessary
            info.Visibility = Visibility.Hidden;
            keep_data.Visibility = Visibility.Hidden;
            Dice.IsEnabled = true;   //make the dice clickable
            turn_text.Text = _turnText;   //since it's the start, it's player 1's turn
        }
        private void executeSetting(ref Grid Game_Grid, ref Label RollOrWait, string _rollText)
        {
            short setPlayersAt0 = 0;
            foreach (Player irrelevantName in playerData)
            {
                Game_Grid.Children.Remove(playerData[setPlayersAt0].GetBlock());
                Grid.SetRow(playerData[setPlayersAt0].GetBlock(), 1);
                Grid.SetColumn(playerData[setPlayersAt0].GetBlock(), 0);
                Game_Grid.Children.Add(playerData[setPlayersAt0].GetBlock());
                setPlayersAt0++;
            }
            RollOrWait.Content = _rollText;
        }
    }
}
