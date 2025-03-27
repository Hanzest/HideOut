using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class GameManager
    {
        private string _name;
        private int _theme;
        private Saver _saver;
        private bool _isNotLost;
        private bool _isBuffUpdated;
        private bool _isFreeAll;

        private bool _isSetUp;
        private DrawGameObject _drawGameObject;
        private DrawMap _drawMap;
        private DrawText _drawText;
        private DrawStatusBoard _drawStatusBoard;
        private InputHandler _inputHandler;
        private ICharacterFactory _playerFactory;
        private ICharacterFactory _enemyFactory;
        private IItemFactory _weaponFactory;
        private IItemFactory _rewardFactory;
        private IItemFactory _gateFactory;
        private EffectFactory _effectFactory;
        private ProjectileFactory _projectileFactory;
        private Player _player;
        private Point2D _camera;
        private Point2D _centering;
        private Map _map;
        private HashSet<Character> _characters;
        private HashSet<Projectile> _projectiles;
        private HashSet<Effect> _effects;
        private HashSet<Item> _items;
        private ItemDrop _itemDrop;
        private Spawner _spawner;
        private Loader _loader;
        public GameManager()
        {
            _name = "default";
            _theme = 0;
            _saver = new Saver();
            _isNotLost = true;
            _isBuffUpdated = false;
            _isSetUp = false;
            _isFreeAll = false;
            _loader = new Loader();
            _drawText = new DrawText();
            _inputHandler = new InputHandler();
            _playerFactory = new PlayerFactory();
            _enemyFactory = new EnemyFactory();
            _effectFactory = new EffectFactory();
            _weaponFactory = new WeaponFactory();
            _rewardFactory = new RewardFactory();
            _gateFactory = new GateFactory();
            _projectileFactory = new ProjectileFactory();
            _itemDrop = new ItemDrop();
            _camera = new Point2D(0, 0);
            _centering = new Point2D(800f, 480f);
            _characters = new HashSet<Character>();
            _map = new Map();
            _projectiles = new HashSet<Projectile>();
            _effects = new HashSet<Effect>();
            _items = new HashSet<Item>();
            _spawner = new Spawner(new List<List<string>>());
            _drawGameObject = new DrawGameObject();
            _drawMap = new DrawMap();
            _drawStatusBoard = new DrawStatusBoard();
        }
        public void SetUp(string name, int theme, Saver saver, ref bool isStartGame, ref BuffManager buffManager)
        {
            // if isStartGame = false --> Option: Continue
            Console.WriteLine($"This marked new Set Up log");
            Console.WriteLine($"Is Start Game: {isStartGame}");
            _name = name;
            _saver = saver;
            _theme = theme;
            Console.WriteLine(theme);
            if(saver.Level <= 3 && theme == 2)
            {
                // Only allow theme 0 and 1 for level 1-3
                theme = SplashKit.Rnd(0, 2);
            } else if (_saver.Level == 7 || _saver.Level == 10)
            {
                _theme = 3;
            }
            Console.WriteLine($"Theme: {_theme}");
            _loader.LoadResource(_drawGameObject, _drawMap, _drawStatusBoard, _spawner, _theme);
            _map = new Map();
            
            Console.WriteLine($"Level: {_saver.Level}");
            _map.GenerateMap();
            _isFreeAll = false;
            _player = (Player)_playerFactory.Create(_name, 250, 0);
            if (isStartGame == false)
                // Continue playing
            {
                _loader.LoadSaveGame(ref _player, ref _saver, ref _playerFactory, ref _enemyFactory, ref buffManager
                                    , ref _weaponFactory, ref _items);
                EnemyFactory tempEF = (EnemyFactory)_enemyFactory;
                PlayerFactory tempPF = (PlayerFactory)_playerFactory;
                Console.WriteLine($"PlayerFactory: {tempPF.BonusHP} {tempPF.BonusEnergy} {tempPF.BonusArmor} {tempPF.BonusSpeed}");
                Console.WriteLine($"EnemyFactory: {tempEF.DecreaseHP} {tempEF.DecreaseSpeed} {tempEF.DecreaseDamage}");
                Console.WriteLine($"Player: {_player.Name} {_player.Health} {_player.Energy} {_player.Coin}");
                if(_saver.Level == 10)
                {
                    int rnd = SplashKit.Rnd(0, 3);
                    switch (rnd)
                    {
                        case 0:
                            _player.MaxHealth -= 80;
                            _player.HealthChanged(0);
                            break;
                        case 1:
                            _player.MaxEnergy -= 125;
                            _player.EnergyChanged(0);
                            break;
                        case 2:
                            _player.MaxArmor -= 50;
                            _player.ArmorChanged(0);
                            break;
                    }
                }
                
            } else
            {
                // Start new game
                Console.WriteLine($"Inside GameManager: Start Default Game");
                _loader.LoadDefault(_player, _saver);
                Item personalItem = _weaponFactory.Create(_player.PersonalItem, 0f, 0f);
                _player.Inventory.Add(personalItem, _player);
                _items.Add(personalItem);
                saver.IsAbleToContinue = true;
                isStartGame = false; // Set for later load
            }
            _spawner.SetUpRoom(_characters, _items, _map.Rooms, _enemyFactory, _gateFactory, _theme);
            
            _characters.Add(_player);
            _items.Add(_weaponFactory.Create("revolver", 450f, 0));
            foreach (Character character in _characters)
            {
                if (character.Type == CharacterType.RangeEnemy)
                {
                    RangeEnemy rangeEnemy = (RangeEnemy)character;
                    _items.Add(rangeEnemy.Inventory.GetItem);
                }
            }
            _isSetUp = true;
        }
        
        public void Update()
        {
            _inputHandler.HandleKeyboardInput(_player, _map, _projectiles,
                                        _items, _characters, _effects,
                                        _effectFactory, _projectileFactory);
            foreach (Character character in _characters)
            {
                character.CollisionWithProjectile(_projectiles, _map.Rooms);
                character.CollisionWithMeleeWeapon(_items, _map.Rooms);
                character.UpdateRoomNumber(_map.Rooms);
                if (!character.Exist)
                {
                    if(character.Type == CharacterType.RangeEnemy)
                    {
                        RangeEnemy rangeEnemy = (RangeEnemy)character;
                        _items.Remove(rangeEnemy.Inventory.GetItem);
                    }
                    for(int i = 0; i < 8; i++)
                        // Create rewards
                    {
                        int rndEnergy = SplashKit.Rnd(0, 4);
                        int rndCoin = SplashKit.Rnd(0, 2);
                        if(rndEnergy == 1)
                        {
                            _items.Add(_rewardFactory.Create("Energy Particle", character.X, character.Y));
                        }
                        if(rndCoin == 1)
                        {
                            _items.Add(_rewardFactory.Create("Coin", character.X, character.Y));
                        }
                    }
                    if(character.Type == CharacterType.Player)
                    {
                        _saver.IsLost = true;
                        _isNotLost = false;
                        _saver.SaveLost();
                    } else
                    {
                        _itemDrop.Drop(_weaponFactory, _items, character.Coordinate());
                    }
                        _characters.Remove(character);
                    
                }
                switch (character.Type)
                {
                    case CharacterType.Player:
                        character.Tick();
                        Player p = (Player)character;
                        if(_map.Rooms[p.RoomIndex].RoomNumber == 2 && !(_map.Rooms[p.RoomIndex].IsClear) && !_map.Rooms[p.RoomIndex].IsPlayerEnter){
                            p.X += 60;
                            _map.Rooms[p.RoomIndex].Lock();
                        }
                        p.Inventory.UpdateItemPosition(p);
                        p.RegenerateArmor();
                        if(p.Inventory.GetItem != null)
                        {
                            p.Inventory.GetItem.Angle = p.NearestEnemyAngle(p.NearestEnemy(_characters));
                        }
                        break;
                    case CharacterType.MeleeEnemy:
                        MeleeEnemy mEnemy = (MeleeEnemy)character;
                        mEnemy.FindPlayerNearby(_player, _map.Rooms);
                        if (mEnemy.IsAttack)
                        {
                            _effects.Add(_effectFactory.Create("scratch", mEnemy.NearestEnemy(_characters).X + SplashKit.Rnd(-16, 16),
                                mEnemy.NearestEnemy(_characters).Y + SplashKit.Rnd(-16, 8), mEnemy.FaceLeft));
                        }
                        break;
                    case CharacterType.RangeEnemy:
                        RangeEnemy rangeEnemy = (RangeEnemy)character;
                        rangeEnemy.Inventory.UpdateItemPosition(rangeEnemy);
                        rangeEnemy.FindPlayerNearby(_player, _map.Rooms);
                        RangeWeapon rangeWeapon = (RangeWeapon)rangeEnemy.Inventory.GetItem;
                        rangeWeapon.Display = true;
                        rangeWeapon.Angle = rangeEnemy.NearestEnemyAngle(rangeEnemy.NearestEnemy(_characters));
                        if (rangeEnemy.IsAttack)
                        {
                            rangeWeapon.UseBy(rangeEnemy, rangeEnemy.NearestEnemy(_characters), _effects,
                                    _effectFactory, _projectiles, _projectileFactory);
                        }
                        break;
                }
            }
            _map.UpdateMap(_characters);
            foreach (Item item in _items) 
            {
                if(item.Type == ItemType.Reward)
                {
                    Reward reward = (Reward)item;
                    reward.MoveTowardsPlayer(_player);
                    if (!reward.Exist)
                    {
                        if(reward.Name == "Coin")
                        {
                            _player.Coin += 1;
                        }
                        if(reward.Name == "Energy Particle")
                        {
                            _player.EnergyChanged(2);
                        }
                    }
                } else if(item.Type == ItemType.Gate)
                {
                    Gate gate = (Gate)item;
                    if (gate.Name == "OutGate" && gate.IsPlayerInteract)
                    {
                        _saver.SaveWin(_player, _playerFactory, _enemyFactory);
                        _isNotLost = true;
                    }
                }
            }
            _camera = _player.Coordinate() - _centering;

            foreach(Projectile projectile in _projectiles)
            {
                projectile.Move(_map.Rooms);
                if (projectile.Collided)
                {
                    if(projectile.Collision != "null")
                    {
                        _effects.Add(_effectFactory.Create(projectile.Collision, projectile.X, projectile.Y, false));
                    }
                    _projectiles.Remove(projectile);
                }
            }

            foreach (Effect effect in _effects)
            {
                effect.Tick();
                if (!effect.Exist)
                {
                    _effects.Remove(effect);
                }
            }

            foreach (Item item in _items)
            {
                if (!item.Exist)
                {
                    _items.Remove(item);
                }
            }

            SplashKit.SetCameraPosition(_camera.ToSplashKitPoint());
        }
        public void Draw()
        {
            _drawMap.Draw(_map);
            foreach (Item item in _items)
            {
                if(item.Type == ItemType.Gate)
                {
                    
                    _drawGameObject.DrawStaticItem(item);
                }
            }

            _drawGameObject.Draw(_characters);
            _drawGameObject.Draw(_projectiles);
            _drawGameObject.Draw(_effects);
            // Effect is not game Object but drawGameObject can draw it.
           _drawGameObject.Draw(_items, _player.NearestEnemyAngle(_player.NearestEnemy(_characters)));
            foreach (Item item in _items)
            {
                switch (item.Type)
                {
                    case ItemType.Potion:
                        Potion potion = (Potion)item;
                        if (potion.NearByPlayer(_player, _map.Rooms[0].TileSize))
                        {
                            _drawText.DrawH1(potion.Name, potion.X, potion.Y);
                        }
                        break;
                    case ItemType.RangeWeapon:
                        RangeWeapon rangeWeapon = (RangeWeapon)item;
                        if (rangeWeapon.NearByPlayer(_player, _map.Rooms[0].TileSize) && !rangeWeapon.InInventory)
                        {
                            _drawText.DrawH1(rangeWeapon.Name, rangeWeapon.X, rangeWeapon.Y);
                        }
                        break;
                    case ItemType.MeleeWeapon:
                        MeleeWeapon meleeWeapon = (MeleeWeapon)item;
                        if(meleeWeapon.NearByPlayer(_player, _map.Rooms[0].TileSize) && !meleeWeapon.InInventory)
                        {
                            _drawText.DrawH1(meleeWeapon.Name, meleeWeapon.X + meleeWeapon.Width / 4, meleeWeapon.Y);
                        }
                        break;
                    case ItemType.Gate:
                        Gate gate = (Gate)item;
                        if(gate.Name == "OutGate" && gate.NearByPlayer(_player, _map.Rooms[0].TileSize))
                        {
                            _drawText.DrawSuperH1("Go Inside", gate.X, gate.Y - 108);
                        }
                        break;
                }
            }
            
            foreach (Item item in _items)
            {
                if(item.InInventory && item.Display)
                {
                    _drawGameObject.DrawPrioritizedItem(item, item.Angle);
                }
                ;
            }
            _drawStatusBoard.Draw(_player, _saver);
        }
        public void UpdateBuff(BuffManager buffManager)
        {
            // Update player factory
            PlayerFactory playerFactory = (PlayerFactory)_playerFactory;
            playerFactory.BonusHP = buffManager.GetBuffIndex(0);
            playerFactory.BonusEnergy = buffManager.GetBuffIndex(1);
            playerFactory.BonusArmor = buffManager.GetBuffIndex(2);
            playerFactory.BonusSpeed = buffManager.GetBuffIndex(3);
            // Update enemy factory
            EnemyFactory enemyFactory = (EnemyFactory)_enemyFactory;
            enemyFactory.DecreaseHP = buffManager.GetBuffIndex(4);
            enemyFactory.DecreaseSpeed = buffManager.GetBuffIndex(5);
            _playerFactory = playerFactory;
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
            _isBuffUpdated = true;
        }
        public void FreeAll()
        {
            Console.WriteLine("Inside Game Manager: Free All");
            foreach(Item item in _items)
            {
                _items.Remove(item);
            }
            foreach (Character character in _characters)
            {
                _characters.Remove(character);
            }
            foreach (Projectile projectile in _projectiles)
            {
                _projectiles.Remove(projectile);
            }
            foreach (Effect effect in _effects)
            {
                _effects.Remove(effect);
            }
            _isNotLost = true;
            _isBuffUpdated = false;
            _isSetUp = false;
            _isFreeAll = true;
        }
        public Saver Saver
        {
            get { return _saver; }
        }
        public bool IsSetUp
        {
            get { return _isSetUp; }
            set { _isSetUp = value; }
        }
        public bool IsNotLost
        {
            get { return _isNotLost; }
        }
        public bool IsBuffUpdated
        {
            get { return _isBuffUpdated; }
        }
        public bool IsFreeAll
        {
            get { return _isFreeAll; }
        }
    }
}
