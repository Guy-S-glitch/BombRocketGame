using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GameLogic.Models
{
    /*
    * Player class represents a player in the game.
    * This class holds information about the player's ID, name, character icon, Place on the board etc.
    */
    public class Player
    {
        private List<string> _charactesList = new List<string>() { "🐱", "🐼", "🐻", "🐨", "🐮", "🐷", "🐹", "🐭", "🐰", "🐵", "🐶" };
        public List<string> charactersHere { get { return _charactesList; } }
        public short Id { get; set; }
        private bool _trash { get; set; }
        // Contain player's name
        public string Name { get; set; }
        // Contain the player's character 
        public string strIcons { get; set; }
        // Contain the current place of the player
        public short Place { get; set; }
        // Showing the data of the player on the board 
        private TextBlock _textBlock { get; set; }
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
        public TextBlock GetBlock()
        {
            return _textBlock;
        }
        public bool GetTrash()
        {
            return _trash;
        }
        public void SetTrash(bool setTrash)
        {
            _trash = setTrash;
        }

    }
}
