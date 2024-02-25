using gameLogic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GameLogic.Models
{

    public class Player
    {

        private List<string> _charactesList = new List<string>() { "🐱", "🐼", "🐻", "🐨", "🐮", "🐷", "🐹", "🐭", "🐰", "🐵", "🐶" };
        public List<string> charactersHere { get { return _charactesList; } }
        public short Id { get; set; }
        private bool _trash { get; set; }
        public string Name { get; set; }   //contain player's name
        public string strIcons { get; set; }   //contain the player's character 
        public short Place { get; set; }   //contain the current place of the player
        
        private TextBlock _textBlock { get; set; }   //showing the data of the player on the board 
        public Player(short id, short place, TextBlock block)
        {

            Id = id;
            Place = place;
            _textBlock = block;
            TextboxSettings(ref block);
        }
        private void TextboxSettings(ref TextBlock block)
        {
            block.Text = strIcons;
            block.FontSize = 20;
            Panel.SetZIndex(block, 2); 
            block.VerticalAlignment = VerticalAlignment.Bottom;
            block.FontWeight = FontWeights.Bold;
        }
        public TextBlock GetBlock() { return _textBlock; }
        public bool GetTrash() { return _trash; }
        public void SetTrash(bool setTrash) { _trash = setTrash; }

    }
}
