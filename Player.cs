using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class Player : Character
    {
        private int _armor;
        private int _maxArmor;
        private int _energy;
        private int _maxEnergy;
        // Player size: 96 x 128
        public Player(string name, int health, CharacterType type, int x, int y, float velocity) : base(name, health, type)
        {
            X = x;
            Y = y;
            _maxEnergy = 200;
            _energy = 200;
            _armor = 30;
            _maxArmor = 30;
            Velocity = velocity;
        }
        public void SetUp(int armor, int energy)
        {
            _armor = armor;
            _maxArmor = armor;
            _energy = energy;
            _maxEnergy = energy;
        }
        public void UseItem()
        {

        }
        public int Armor
        {
            get { return _armor; }
            set { _armor = value; }
        }
        public int MaxArmor
        {
            get { return _maxArmor; }
            set { _maxArmor = value; }
        }
        public int Energy
        {
            get { return _energy; }
            set { _energy = value; }
        }
        public int MaxEnergy
        {
            get { return _maxEnergy; }
            set { _maxEnergy = value; }
        }
        public void EnergyChanged(int value)
        {
            if (_energy + value> _maxEnergy)
            {
                _energy = _maxEnergy;
            } else { _energy += value; }
            if(Energy < 0)
            {
                _energy = 0;
            }
        }
        public void TakeDamage(int value, Point2D dir, Room[] rooms)
        {
            if (_armor - value > _maxArmor)
            {
                _armor = _maxArmor;
            }
            else
            {
                _armor -= value;
                if (CheckValidMove(X + dir.X * 1.5f, Y, rooms, 96, 128))
                {
                    X += dir.X * 1.5f;
                }
                if(CheckValidMove(X, Y + dir.Y * 1.5f, rooms, 96, 128))
                {
                    Y += dir.Y * 1.5f;
                }


                if (_armor < 0)
                {
                    HealthChanged(_armor);
                    _armor = 0;
                }
            }
        }
        public void UseItem()
    }
}
