using gameLogic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GameLogic.Models
{

    public class Player
    { 

        public List<string> charara = new List<string>() { "🐱", "🐼", "🐻", "🐨", "🐮", "🐷", "🐹", "🐭", "🐰", "🐵", "🐶" };
        public List<string> charactersHere { get { return charara; } }
        public int Id { get; set; }
        public string trash { get; set; }
        public string Name { get; set; }   //contain player's name
        public string strIcons { get; set; }   //contain the player's character 
        public int Place { get; set; }   //contain the current place of the player
        public TextBlock TextBlock { get; set; }   //showing the data of the player on the board 
        public Player(int id, int place, TextBlock text)
        {

            Id = id;
            Place = place;
            TextBlock = text;
            TextBlock.Text = strIcons;
            TextBlock.FontSize = 20;
            System.Windows.Controls.Panel.SetZIndex(TextBlock, 2);   //makes it so the player's character  will be on the board but under any message that will appear
            TextBlock.VerticalAlignment = VerticalAlignment.Bottom;
            TextBlock.FontWeight = FontWeights.Bold;
        }
    }
}
