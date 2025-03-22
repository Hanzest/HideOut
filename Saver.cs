using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class Saver
    {
        private int _level;
        private string _name;
        private int _health;
        private int _energy;
        private int _coin;
        private bool _isLost;
        private bool _isSaved;
        private BuffManager _buffManager;
        private StreamWriter _writer;
        public Saver() 
        {
            _isSaved = false;
            _isLost = false;
            _level = 1;
            _buffManager = new BuffManager();
        }
        public int Level 
        { 
            get { return _level; } 
            set { _level = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public int Energy
        {
            get { return _energy; }
            set { _energy = value; }
        }
        public int Coin
        {
            get { return _coin; }
            set { _coin = value; }
        }
        public void Save(Player player)
        {
            _name = player.Name; 
            _health = player.Health;
            _coin = player.Coin;
            _energy = player.Energy;
            IsSaved = true;
            _level = _level + 1;
            //_buffManager = buffManager;
            //_writer = new StreamWriter("Resource\\SaveGame.txt");
            //try
            //{
            //    _writer.WriteLine(_level);
            //    _writer.WriteLine(_name);
            //    _writer.WriteLine(_health);
            //    _writer.WriteLine(_coin);
            //    _writer.WriteLine(_energy);
            //}
            //finally
            //{
            //    _writer.Close();
            //}
        }
        public void Write()
        {
            Console.WriteLine($"Is Saved: {_level} {_name} {_health}");
            _writer = new StreamWriter("Resource\\SaveGame.txt");
            try
            {
                Console.WriteLine($"Is Saved: {_level} {_name} {_health}");
                _writer.WriteLine(_level);
                _writer.WriteLine(_name);
                _writer.WriteLine(_health);
                _writer.WriteLine(_coin);
                _writer.WriteLine(_energy);
            }
            finally
            {
                _writer.Close();
            }
        }
        public bool IsLost
        {
            get { return _isLost; }
            set { _isLost = value; }
        }
        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; }
        }
        // Implement Skill Tree Storing
    }
}
