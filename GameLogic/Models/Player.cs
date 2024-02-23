using gameLogic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static System.Net.Mime.MediaTypeNames;

namespace GameLogic.Models
{

    public class Player
    {
        private List<string> _charara = new List<string>() { "🐱", "🐼", "🐻", "🐨", "🐮", "🐷", "🐹", "🐭", "🐰", "🐵", "🐶" };
        private List<string> _charactersHere { get { return _charara; } }
        private int _id { get; set; }


        // trash is blah blah blah, or AT LEAST meaningfull name like _isFinishedFlag
        private string _trash { get; set; }
        private bool _isFinishedFlag { get; set; }


        private string _name { get; set; }   //contain player's name
        private string _strIcons { get; set; }   //contain the player's character 
        private int _place { get; set; }   //contain the current place of the player
        private TextBlock _textBlock { get; set; }   //showing the data of the player on the board 
       
        private int defaultFontSize = 20;

        public Player(int id, int place, TextBlock text)
        {
            _id = id;
            _place = place;
            populate_TextBlock(text, _strIcons, defaultFontSize);
            setDefaults();
        }

        // populate _textBlock
        private void populate_TextBlock(TextBlock text,string strIcons, int fontSize)
        {
            _textBlock = text;
            _textBlock.Text = strIcons;
            _textBlock.FontSize = fontSize;
        }

        // The default params for the player
        private void setDefaults()
        {
            System.Windows.Controls.Panel.SetZIndex(_textBlock, 2);   //makes it so the player's character  will be on the board but under any message that will appear
            _textBlock.VerticalAlignment = VerticalAlignment.Bottom;
            _textBlock.FontWeight = FontWeights.Bold;
        }

        public int getId()
        {
            return _id;      
        }

        public string getName()
        {
            return _name;
        }
    }
}
