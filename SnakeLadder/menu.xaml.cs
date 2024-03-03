using GameLogic.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace SnakeLadder
{
    public partial class menu : Window
    {

        private void How_much_SelectionChanged(object sender, SelectionChangedEventArgs e) { pregameLogic.How_much_SelectionChanged(sender, e, ref How_much, ref keep_data, ref select_players, ref info); }
        private void info_Click(object sender, RoutedEventArgs e) { pregameLogic.info_Click(sender, e, ref info, ref keep_data, ref Dice, ref turn_text, ref Game_Grid, ref RollOrWait, _rollText); }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
            {
                comboBox.IsEnabled = false;
                foreach (Player player in pregameLogic.playerData) player.charactersHere.Remove(comboBox.SelectedItem.ToString());
            }
        }

        // Since, we have the movement path of the game, it's possible to set the movement to 1 space and fill each space with decoration
        private void ColorTable() { decoration.ColorTable(Game_Grid); }   

        public menu()
        {
            InitializeComponent();
            // Create the playing board
            ColorTable();   
            How_much.SelectedIndex = 0 ;
            // Access responsive key game
            this.KeyDown += MainWindow_KeyDown;   
        }

        // Contains where every player needs to go according to the roll
        private void GetRowColumnInfo() 
        {
            // Every row is 10 spaces, every row is from column 1-10
            _currentRow = (short)(_currentPlace == 0 ? 1 : _currentPlace % 10 != 0 ? 1 + _currentPlace / 10 : _currentPlace / 10);
            _futureRow = (short)(_nextPlace % 10 != 0 ? 1 + _nextPlace / 10 : _nextPlace / 10);
            // place=0 => row=1 // place=1 => row= 1+1/10 = 1+0= 1 // place=10 => row= 10/10 = 1

            // Every column is 10 spaces, every colomn is from row 1-10, every 2nd row the direction is changed -> example:1st row goes left to right, 2nd row goes from right to left 
            _currentColumn = (short)(_currentPlace == 0 ? 0 : _currentRow % 2 != 0 && _currentPlace % 10 == 0 ? 10 : _currentRow % 2 == 0 && _currentPlace % 10 == 0 ? 1 : _currentRow % 2 == 0 && _currentPlace % 10 != 0 ? 11 - (_currentPlace % 10) : _currentPlace % 10);
            _futureColumn = (short)(_futureRow % 2 != 0 && _nextPlace % 10 == 0 ? 10 : _futureRow % 2 == 0 && _nextPlace % 10 == 0 ? 1 : _futureRow % 2 == 0 && _nextPlace % 10 != 0 ? 11 - (_nextPlace % 10) : _nextPlace % 10);
            // place=0 => column=0 // place=10 => row=1 => column=10  // place=20 => row=2 => column=1 // place=11 => row=2 => column= 11-(11%10) = 11-1 = 10 // place=1 => column= 1%10 = 1                                                                                                                                                                                              //example:10-> 10's row is 1 using the row's formula, 1%2!=0 and 10%10=0 -> column is set to 10
        }

        private void Dice_Click(object sender, RoutedEventArgs e)
        {
            // Set values on every new roll
            DiceInitialValues();
            // Check if player land or surpass 100 and give relative output
            CheckIfSurpassOrLand100();
            // Get info about where the player now and where he'll be after his roll
            GetRowColumnInfo();
            // Call the animation
            ColumnAnimation(sender, e);   
        }
        private void DiceInitialValues()
        {
            RollOrWait.Content = _waitText;
            Bomb_rocket_text.Text = string.Empty;
            // Present a cube throw
            short rolled = (short)_random.Next(1, 7);
            // The random number showed to the player
            DiceOutput(rolled);   
            _currentPlace = (short)pregameLogic.playerData[_turn - 1].Place;
            // Advance the player relatively to his throw
            pregameLogic.playerData[_turn - 1].Place += rolled;
            _nextPlace = (short)pregameLogic.playerData[_turn - 1].Place;
        }

        // Showing suitable picture according to the player's roll
        private void DiceOutput(short roll) { decoration.DiceOutput(ref roll, ref Imagin); }   
        private void CheckIfSurpassOrLand100()
        {
            if (pregameLogic.playerData[_turn - 1].Place > 100)
            {
                _ricochet100Flag = true;
                // If the player's roll surpass 100 he'll go back the amount he went over
                pregameLogic.playerData[_turn - 1].Place = (short)(100 - (pregameLogic.playerData[_turn - 1].Place - 100));    
                _nextPlace = (short)pregameLogic.playerData[_turn - 1].Place;
            }
            else if (pregameLogic.playerData[_turn - 1].Place == 100) _winFlag = true;
        }

        // Once a key pressed enter the method
        private void MainWindow_KeyDown(object sender,KeyEventArgs e)   
        {
            switch (e.Key)
            {
                case Key.Escape:
                    // If player press esc ask him if he's sure he want to exit the game
                    ExitConfirm();
                    break;
                case Key.Home:
                    // If player press home ask him if he's sure he want to return to the menu
                    HomeConfirm();
                    break;
                case Key.Tab:
                    if (Dice.IsEnabled) Dice_Click(sender, e);
                    break;
            }
        }
        private void back_Click(object sender, RoutedEventArgs e) { HomeConfirm(); } 
        private void exit_Click(object sender, RoutedEventArgs e) { ExitConfirm(); }

        // Ask if the player sure he want to exit
        private void ExitConfirm()   
        {
            object var = MessageBox.Show(_exitMessage, _exitCaption, MessageBoxButton.YesNo);
            switch (var)
            {
                case MessageBoxResult.Yes:
                    MessageBox.Show(_exitYes);
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show(_exitNo);
                    break;
            }
        }

        // Ask if the player sure he want to return to menu
        private void HomeConfirm()   
        {
            object var = MessageBox.Show(_menuMessage, _menuCaption, MessageBoxButton.YesNo);
            switch (var)
            {
                case MessageBoxResult.Yes:
                    MessageBox.Show(_menuYes);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show(_menuNo);
                    break;
            }
        }

    }
}

