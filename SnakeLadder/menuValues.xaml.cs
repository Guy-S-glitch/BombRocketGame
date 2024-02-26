using gameLogic;
using GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameLogic.Enum;

namespace SnakeLadder
{
    partial class menu
    {
        Decoration decoration = new Decoration();
        PreGame pregameLogic = new PreGame();



        private static Random _random = new Random();

        private static short _turn = 1;   //keep tracks of who's turn is it 
        private static short _randomRocketBomb;    //used to contain the random amount of spaces the rocket/bomb send you
        private static short _currentRow, _futureRow, _currentColumn, _futureColumn, _currentPlace, _nextPlace;   //used to navigate the character on the grid
        private static short[] _bombRocketPlaces = { (short)RocketBomb.rocket1, (short)RocketBomb.rocket2, (short)RocketBomb.rocket3 , (short)RocketBomb.rocket4,
                                                     (short)RocketBomb.rocket5, (short)RocketBomb.rocket6, (short)RocketBomb.bomb1,  (short)RocketBomb.bomb2,
                                                     (short)RocketBomb.bomb3,  (short)RocketBomb.bomb4, (short)RocketBomb.bomb5
        };
        private static bool _bombFlag = false, _boostFlag = false;   //if player land on bomb raise flag
        private static bool _ricochet100Flag = false, _winFlag = false;   //flag raised to determain if the player surpass 100 and call relative animation

        private static string _rollText = "You can roll", _waitText = "Wait";
        private static string _exitMessage = "Are you sure you want to exit", _exitCaption = "Conform Exit", _exitYes = "thanks for playing", _exitNo = "returning to the game";
        private static string _menuMessage = "Are you sure you want to return to the menu", _menuCaption = "Confirm Menu", _menuYes = "going back to menu", _menuNo = "returning to the game";

    }
}
