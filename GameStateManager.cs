using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class GameStateManager
    {
        private GameState _currentState;
        public GameStateManager()
        {
            _currentState = GameState.MainMenu;
        }
        public void SetState(GameState newState)
        {
            _currentState = newState;
        }
        public GameState GetState()
        {
            return _currentState;
        }
    }
}
