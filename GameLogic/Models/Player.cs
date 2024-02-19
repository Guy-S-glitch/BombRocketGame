using gameLogic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GameLogic.Models
{

    public class Player
    { 

        PreGame spre = new PreGame();
        public List<string> charara = new List<string>() { "🐱", "🐼", "🐻", "🐨", "🐮", "🐷", "🐹", "🐭", "🐰", "🐵", "🐶" };
        public List<string> charactersHere { get { return charara; } }
        public int Id { get; set; }
        public string Name { get; set; }   //contain player's name
        public int intIcons { get; set; }   //get the character  of the player in number
        public string strIcons { get; set; }   //turning the property intIcons to a character 
        public int Place { get; set; }   //contain the current place of the player
        public TextBlock TextBlock { get; set; }   //showing the data of the player on the board 
        public Player(int id, int place, TextBlock text)
        {
            Id = id;
            Place = place;
            TextBlock = text;
            TextBlock.FontSize = 20;
            System.Windows.Controls.Panel.SetZIndex(TextBlock, 2);   //makes it so the player's character  will be on the board but under any message that will appear
            TextBlock.VerticalAlignment = VerticalAlignment.Bottom;
            TextBlock.FontWeight = FontWeights.Bold;
        }
    }
}
