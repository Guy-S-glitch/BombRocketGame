using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SnakeLadder
{
    public partial class menu
    {
        private void ColumnAnimation(object sender, EventArgs e)
        {
            Int32Animation columnMove = new Int32Animation();   //set our value for the column animation
            ColumnAnimationInitialValues(ref columnMove);   //set the initial values
            ColumnAnimationCases(ref columnMove);   //check which animation needed
            pregameLogic.playerData[_turn - 1].GetBlock().BeginAnimation(Grid.ColumnProperty, columnMove);   //execute the animation
        }
        private void ColumnAnimationInitialValues(ref Int32Animation columnMove)
        {
            Dice.IsEnabled = false;   //while the animation running the dice button isn't available
            columnMove.From = _currentColumn;
            columnMove.To = _futureColumn;
            columnMove.Duration = TimeSpan.FromMilliseconds(350);
        }
        private void ColumnAnimationCases(ref Int32Animation columnMove)
        {
            if (_winFlag)   //player land on 100 block and won
            {
                columnMove.Completed += Winner;
                _winFlag = false;
            }

            else if (_ricochet100Flag)   //player surpass 100 and needs to go back
            {
                _ricochet100Flag = false;
                columnMove.To = 1;
                columnMove.Completed += ColumnAnimation;
            }

            else if (_currentRow != _futureRow)   //player need to change row to move
            {
                if (_bombFlag) { if (_currentRow % 2 == 0) columnMove.To = 10; else columnMove.To = 1; }   //player land on bomb therefore his movement will be negative
                else { if (_currentRow % 2 == 1) columnMove.To = 10; else columnMove.To = 1; }   //else his movement will be positive
                columnMove.Completed += RowAnimation;
            }

            else columnMove.Completed += LookForBombRocket;   //player's next block is on the same row which means we can only refer to the column
            _currentColumn = (short)columnMove.To;
        }


        private void RowAnimation(object sender, EventArgs e)
        {
            Int32Animation rowMove = new Int32Animation();   //set our value for the row animation
            RowAnimationInitialValues(ref rowMove);   //set the initial values
            RowAnimationCases(ref rowMove);   //check which animation needed
            pregameLogic.playerData[_turn - 1].GetBlock().BeginAnimation(Grid.RowProperty, rowMove);   //execute the animation
        }
        private void RowAnimationInitialValues(ref Int32Animation rowMove)
        {
            rowMove.From = _currentRow;
            rowMove.Duration = TimeSpan.FromMilliseconds(100);
        }
        private void RowAnimationCases(ref Int32Animation rowMove)
        {
            if (_bombFlag) rowMove.To = _currentRow - 1;
            else rowMove.To = _currentRow + 1;
            _currentRow = (short)rowMove.To;
            rowMove.Completed += ColumnAnimation;
        }


        private void LookForBombRocket(object sender, EventArgs e)
        {
            BombRocketInitialValues();   //reset the needed values so they wont be effected by previous setting
            BoockCheckBombRocket();   //check if the current block is bomb or rocket
            ExecuteBombRocketeffects(sender, e);   //execute relative output
        }
        private static void BombRocketInitialValues()
        {
            _boostFlag = false;
            _bombFlag = false;
        }
        private void BoockCheckBombRocket()
        {
            for (short checkForBombRocket = 0; checkForBombRocket < _bombRocketPlaces.Count(); checkForBombRocket++)
                if (_nextPlace == _bombRocketPlaces[checkForBombRocket])
                {
                    Thread.Sleep(100);
                    if (checkForBombRocket < 6) //landing on rocket
                    {
                        _randomRocketBomb = (short)_random.Next(1, 7);
                        _boostFlag = true;
                    }
                    else
                    {
                        _randomRocketBomb = (short)_random.Next(-12, 0);
                        _bombFlag = true;
                    }
                    SetBombRocketEffects();
                    break;
                }
        }
        private void SetBombRocketEffects()
        {
            _currentPlace = (short)pregameLogic.playerData[_turn - 1].Place;
            pregameLogic.playerData[_turn - 1].Place += _randomRocketBomb;
            _nextPlace = (short)pregameLogic.playerData[_turn - 1].Place;

        }
        private void ExecuteBombRocketeffects(object sender, EventArgs e)
        {
            //give the player relative message about how much he gain/lost from landing on bombs/rockets
            Bomb_rocket_text.Text = _bombFlag ? $"Player {_turn} hit bombs. in total lose {Math.Abs(_randomRocketBomb)} steps" : _boostFlag ? $"Player {_turn} hit boosts. in total gain {_randomRocketBomb} steps" : Bomb_rocket_text.Text;

            if (_boostFlag || _bombFlag)   //falg raised mean the player land on bomb/rocket and now he's in difference place, therefore we'll need to check if the player land on another bomb/rocket 
            {
                GetRowColumnInfo();
                ColumnAnimation(sender, e);
            }
            else _next_turn_values(sender, e);
        }


        private void _next_turn_values(object sender, EventArgs e)   //set values for the next turn
        {
            RollOrWait.Content = _rollText;
            Dice.IsEnabled = true;
            _turn = (short)(_turn == pregameLogic.GetPlayers() ? 1 : _turn + 1);
            //  player=3, turn=3, next turn=1  // player=3, turn=2, next turn=3
            turn_text.Text = $"Player {_turn}'s turn";
        }

        private void Winner(object sender, EventArgs e)   //once a player won, the current window close and the celebration window open
        {
            MessageBox.Show($"player {_turn}. {pregameLogic.playerData[_turn - 1].Name} won");
            Celebration celebration = new Celebration(pregameLogic.playerData, pregameLogic.GetPlayers());
            this.Close();   // close current window
            celebration.Show();   // goes to celebration screen
        }
    }
}
