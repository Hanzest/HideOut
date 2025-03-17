using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class Saver
    {
        private int _level;
        private int _theme;
        private Player _player;

        public Saver(Player player) 
        {
            _level = 0;
            _theme = SplashKit.Rnd(0, 2);
            _player = player;
        }
        public int Level 
        { 
            get { return _level; } 
            set { _level = value; }
        }
        public void Save(Player player)
        {
            _player = player;
            _level = _level + 1;
        }
        // Implement Skill Tree Storing
    }
}
