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
        // basic information
        private int _level;
        private string _name;
        private int _health;
        private int _maxHealth;
        private int _energy;
        private int _coin;
        // player's buff
        private int _increaseHealth;
        private int _increaseEnergy;
        private int _increaseArmor;
        private int _increaseVelocity;
        // enemy debuff
        private int _decreaseHP;
        private int _decreaseSpeed;
        private int _decreaseDamage;
        // number of Item in Inventory
        private int _numberOfItem;
        private List<string> _itemName;
        // control variable
        private bool _isAbleToContinue;
        private bool _isLost;
        private bool _isSaved;

        private StreamWriter _writer;
        public Saver() 
        {
            _isSaved = false;
            _isLost = false;
            _isAbleToContinue = false;
            _level = 1;
            _increaseHealth = 0;
            _increaseEnergy = 0;
            _increaseArmor = 0;
            _increaseVelocity = 0;
            _decreaseHP = 0;
            _decreaseSpeed = 0;
            _decreaseDamage = 0;
            _numberOfItem = 1;
            _itemName = new List<string>();
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
        public int MaxHealth
        {
            get { return _maxHealth; }
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
        public void SaveLost()
        {
            _isLost = true;
            _writer = new StreamWriter("Resource\\SaveGame.txt");
            try
            {
                _writer.WriteLine("no");
            }
            finally
            {
                _writer.Close();
            }
        }
        public void SaveFinish()
        {
            _writer = new StreamWriter("Resource\\SaveGame.txt");
            try
            {
                _writer.WriteLine("no");
            }
            finally
            {
                _writer.Close();
            }
        }
        public void SaveWin(Player player, ICharacterFactory pFactory ,ICharacterFactory eFactory)
        {
            _itemName.Clear();
            Console.WriteLine("Inside Game Manager: Save Win");
            IsSaved = true;
            _level = _level + 1;
            _name = player.Name; 
            _coin = player.Coin;
            _health = player.Health;
            _maxHealth = player.MaxHealth;
            _energy = player.Energy;
            // Player's buff
            PlayerFactory playerFactory = (PlayerFactory)pFactory;
            _increaseHealth = playerFactory.BonusHP;
            _increaseEnergy = playerFactory.BonusEnergy;
            _increaseArmor = playerFactory.BonusArmor;
            _increaseVelocity = playerFactory.BonusSpeed;
            
            // Enemy's buff
            EnemyFactory enemyFactory = (EnemyFactory)eFactory;
            _decreaseHP = enemyFactory.DecreaseHP;
            _decreaseSpeed = enemyFactory.DecreaseSpeed;
            _decreaseDamage = enemyFactory.DecreaseDamage;
            _numberOfItem = player.Inventory.Count;
            for(int i = 0; i < _numberOfItem; i++)
            {
                _itemName.Add(player.Inventory.GetItemByIndex(i).Name);
            }
        }
        public void SaveBuff(BuffManager buffManager)
        {
            _increaseHealth = buffManager.GetBuffIndex(0);
            _increaseEnergy = buffManager.GetBuffIndex(1);
            _increaseArmor = buffManager.GetBuffIndex(2);
            _increaseVelocity = buffManager.GetBuffIndex(3);
            _decreaseHP = buffManager.GetBuffIndex(4);
            _decreaseSpeed = buffManager.GetBuffIndex(5);
            _decreaseDamage = buffManager.GetBuffIndex(6);
        }
        public void Write()
        {
            Console.WriteLine($"Is Saved: {_level} {_name} {_health}");
            _writer = new StreamWriter("Resource\\SaveGame.txt");
            try
            {
                Console.WriteLine($"Is Saved: {_level} {_name} {_health}");
                _writer.WriteLine("yes");
                // Player's Buff
                _writer.WriteLine(_increaseHealth);
                _writer.WriteLine(_increaseEnergy);
                _writer.WriteLine(_increaseArmor);
                _writer.WriteLine(_increaseVelocity);
                Console.WriteLine($"Save Player's Buff: {_increaseHealth} {_increaseEnergy} {_increaseArmor} {_increaseVelocity}");
                // Enemy's Debuff
                _writer.WriteLine(_decreaseHP);
                _writer.WriteLine(_decreaseSpeed);
                _writer.WriteLine(_decreaseDamage);
                Console.WriteLine($"Save Enemy's Debuff: {_decreaseHP} {_decreaseSpeed} {_decreaseDamage}");
                // Basic Information
                _writer.WriteLine(_level);
                _writer.WriteLine(_name);
                _writer.WriteLine(_health);
                _writer.WriteLine(_coin);
                _writer.WriteLine(_energy);
                // Write Inventory
                _writer.WriteLine(_numberOfItem);
                for (int i = 0; i < _numberOfItem; i++)
                {
                    _writer.WriteLine(_itemName[i]);
                    Console.WriteLine($"Written Item: {_itemName[i]}");
                }
            }
            finally
            {
                _writer.Close();
            }
        }
        public void UpgradeHealth()
        {
            if(_coin > 50)
            {
                _health += 20;
                _coin -= 50;
                if (_health > _maxHealth)
                {
                    _health = _maxHealth;
                }
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
        public bool IsAbleToContinue
        {
            get { return _isAbleToContinue; }
            set { _isAbleToContinue = value; }
        }
    }
}
