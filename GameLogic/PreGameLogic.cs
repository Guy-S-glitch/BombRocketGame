﻿using GameLogic.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace gameLogic
{
    public class PreGame
    {
        // After failing to get the character  directly from the window the best option is to get the index of the chosen character
        // and compare it with list that will have the same values
        public List<string> strings = new List<string>() { "🐱", "🐼", "🐻", "🐨", "🐮", "🐷", "🐹", "🐭", "🐰", "🐵", "🐶" };
        // Keeps the data about every player
        public List<Player> playerData = new List<Player>();      
        public short players,distanceFromLeft = 0, distanceFromBottom = 0;
        private static short _startPoint = 0;
        private static string _turnText = "Player 1's turn", _catchText = "fill everything please";

        public void How_much_SelectionChanged(object sender, SelectionChangedEventArgs e,ref ComboBox How_much,ref ListBox keep_data, ref StackPanel select_players, ref Button info)
        {
            // The only way to set how many player are there is to choose item with number value
            if (How_much.SelectedIndex != 0)   
            {
                // Set data about every player
                initialPlayerSelectStats(How_much, ref keep_data);
                // Set a specipic place for every player so in case multiple player will be on the same box they could see their character
                placeForEveryCharacter();
                // Ignore unnecessary parts and show the relevant ones
                hideShow(ref How_much, ref select_players, ref info, ref keep_data);   
            }
        }
        private void initialPlayerSelectStats(ComboBox How_much, ref ListBox keep_data)
        {
            players = (short)How_much.SelectedIndex;
            // After the amount of players has chosen this combobox is unrelevent
            How_much.IsEnabled = false;
            // Initialize the stats of evert player
            for (short i = 0; i < players; i++) playerData.Add(new Player((short)(i + 1), _startPoint, new TextBlock()));
            // After we filled the needed stats we'll send the information to a listbox that will ask the players for the remaining needed information
            keep_data.ItemsSource = playerData;   
        }
        private void placeForEveryCharacter()
        {
            for (short padd = 0; padd < players; padd++)
            {
                playerData[padd].GetBlock().Padding = new Thickness(distanceFromLeft, 0, 0, distanceFromBottom);   
                distanceFromLeft += 25;
                // Order will be: 1)Left   2)Middle 3)Rigth  4)Left 5)Middle 6)Rigth
                distanceFromLeft = (short)(padd == 2 ? 0 : distanceFromLeft);
                // Order will be: 1)Bottom 2)Bottom 3)Bottom 4)Top  5)Top 6)Top
                distanceFromBottom = (short)(padd == 2 ? distanceFromBottom + 25 : distanceFromBottom);    
            }
        }
        private void hideShow(ref ComboBox How_much, ref StackPanel select_players, ref Button info, ref ListBox keep_data)
        {
            How_much.Visibility = Visibility.Hidden;
            select_players.Visibility = Visibility.Hidden;

            // After selecting how many playing now we need to know information on each player
            info.Visibility = Visibility.Visible;
            keep_data.Visibility = Visibility.Visible;
        }
        public short GetPlayers() { return players; }


        public void info_Click(object sender, RoutedEventArgs e, ref Button info, ref ListBox keep_data, ref Button Dice, ref TextBlock turn_text,ref Grid Game_Grid,ref Label RollOrWait,string _rollText)
        {
            // There will be an error if the player will send null info, thus we'll use try & catch
            try
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
                // Name and icon is requried
                if (string.IsNullOrEmpty(player.Name) || string.IsNullOrEmpty(player.strIcons)) throw new ArgumentNullException();   

                player.GetBlock().Text = player.strIcons;
            }
        }
        private void gameModeSetting(ref Button info, ref ListBox keep_data, ref Button Dice, ref TextBlock turn_text)
        {
            // After reciving the needed info these boxes isn't necessary
            info.Visibility = Visibility.Hidden;
            keep_data.Visibility = Visibility.Hidden;
            // Make the dice clickable
            Dice.IsEnabled = true;
            // Since it's the start, it's player 1's turn
            turn_text.Text = _turnText;   
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
