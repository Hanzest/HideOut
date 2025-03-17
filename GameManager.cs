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
        private bool _isSetUp;
        private DrawGameObject _drawGameObject;
        private DrawMap _drawMap;
        private DrawText _drawText;
        private DrawStatusBoard _drawStatusBoard;
        private InputHandler _inputHandler;
        private ICharacterFactory _playerFactory;
        private ICharacterFactory _enemyFactory;
        private IItemFactory _rangeWeaponFactory;
        private IItemFactory _meleeWeaponFactory;
        private IItemFactory _rewardFactory;
        private IItemFactory _gateFactory;
        private EffectFactory _effectFactory;
        private ProjectileFactory _projectileFactory;
        private Player _player;
        private Point2D _camera;
        private Point2D _centering;
        private List<Map> _maps;
        private HashSet<Character> _characters;
        private HashSet<Projectile> _projectiles;
        private HashSet<Effect> _effects;
        private HashSet<Item> _items;
        private Spawner _spawner;
        private Loader _loader;
        public GameManager()
        {
            _isSetUp = false;
            _loader = new Loader();
            _drawText = new DrawText();
            _inputHandler = new InputHandler();
            _playerFactory = new PlayerFactory();
            _enemyFactory = new EnemyFactory();
            _effectFactory = new EffectFactory();
            _rangeWeaponFactory = new RangeWeaponFactory();
            _meleeWeaponFactory = new MeleeWeaponFactory();
            _rewardFactory = new RewardFactory();
            _gateFactory = new GateFactory();
            _projectileFactory = new ProjectileFactory();
            _camera = new Point2D(0, 0);
            _centering = new Point2D(800f, 480f);
            _characters = new HashSet<Character>();
            _maps = new List<Map>();
            _projectiles = new HashSet<Projectile>();
            _effects = new HashSet<Effect>();
            _items = new HashSet<Item>();
            _spawner = new Spawner(new List<List<string>>());
            _drawGameObject = new DrawGameObject();
            _drawMap = new DrawMap();
            _drawStatusBoard = new DrawStatusBoard();
        }
        public void SetUp()
        {
            _loader.LoadResource(_drawGameObject, _drawMap, _drawStatusBoard, _spawner);
            _maps.Add(new Map());
            _maps[0].GenerateMap();
            _spawner.SetUpRoom(_characters, _items, _maps[0].Rooms, _enemyFactory, _gateFactory, 1);
            _player = (Player)_playerFactory.Create("alchemist", 250, 0);

            _items.Add(new Potion(ItemType.Potion, "Energy potion", false, 400f, 0f));
            _items.Add(_meleeWeaponFactory.Create("Iron Sword", 600f, 0));
            _items.Add(_rangeWeaponFactory.Create("sawed-off shotgun", 800f, 0));
            _items.Add(_rangeWeaponFactory.Create("rifle", 300f, 0));
            _items.Add(_meleeWeaponFactory.Create("Light Saber", 1000f, 0));
//            _player = (Player)_playerFactory.Create("alchemist", 250, 0);

            _characters.Add(_player);
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
            _inputHandler.HandleInput(_player, _maps[0], _projectiles,
                                        _items, _characters, _effects,
                                        _effectFactory, _projectileFactory);
            foreach (Character character in _characters)
            {
                character.CollisionWithProjectile(_projectiles, _maps[0].Rooms);
                character.CollisionWithMeleeWeapon(_items, _maps[0].Rooms);
                character.UpdateRoomNumber(_maps[0].Rooms);
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
                        int rndEnergy = SplashKit.Rnd(0, 2);
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
                    _characters.Remove(character);
                    
                }
                switch (character.Type)
                {
                    case CharacterType.Player:
                        character.Tick();
                        Player p = (Player)character;
                        if(_maps[0].Rooms[p.RoomIndex].RoomNumber == 2 && !(_maps[0].Rooms[p.RoomIndex].IsClear) && !_maps[0].Rooms[p.RoomIndex].IsPlayerEnter){
                            p.X += 60;
                            _maps[0].Rooms[p.RoomIndex].Lock();
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
                        mEnemy.FindPlayerNearby(_player, _maps[0].Rooms);
                        if (mEnemy.IsAttack)
                        {
                            _effects.Add(_effectFactory.Create("scratch", mEnemy.NearestEnemy(_characters).X + SplashKit.Rnd(-16, 16),
                                mEnemy.NearestEnemy(_characters).Y + SplashKit.Rnd(-16, 8), mEnemy.FaceLeft));
                        }
                        break;
                    case CharacterType.RangeEnemy:
                        RangeEnemy rangeEnemy = (RangeEnemy)character;
                        rangeEnemy.Inventory.UpdateItemPosition(rangeEnemy);
                        rangeEnemy.FindPlayerNearby(_player, _maps[0].Rooms);
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
            _maps[0].UpdateMap(_characters);
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
                }
            }
            _camera = _player.Coordinate() - _centering;

            foreach(Projectile projectile in _projectiles)
            {
                projectile.Move(_maps[0].Rooms);
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
            _drawMap.Draw(_maps[0]);
            
            foreach(Item item in _items)
            {
                if(item.Type == ItemType.Gate)
                {
                    _drawGameObject.DrawStaticItem(item);
                }
            }

            _drawGameObject.Draw(_characters);
            _drawGameObject.Draw(_projectiles);
            _drawGameObject.Draw(_effects);
           _drawGameObject.Draw(_items, _player.NearestEnemyAngle(_player.NearestEnemy(_characters)));
            foreach (Item item in _items)
            {
                switch (item.Type)
                {
                    case ItemType.Potion:
                        Potion potion = (Potion)item;
                        if (potion.NearByPlayer(_player, _maps[0].Rooms[0].TileSize))
                        {
                            _drawText.DrawH1(potion.Name, potion.X, potion.Y);
                        }
                        break;
                    case ItemType.RangeWeapon:
                        RangeWeapon rangeWeapon = (RangeWeapon)item;
                        if (rangeWeapon.NearByPlayer(_player, _maps[0].Rooms[0].TileSize) && !rangeWeapon.InInventory)
                        {
                            _drawText.DrawH1(rangeWeapon.Name, rangeWeapon.X, rangeWeapon.Y);
                        }
                        break;
                    case ItemType.MeleeWeapon:
                        MeleeWeapon meleeWeapon = (MeleeWeapon)item;
                        if(meleeWeapon.NearByPlayer(_player, _maps[0].Rooms[0].TileSize) && !meleeWeapon.InInventory)
                        {
                            _drawText.DrawH1(meleeWeapon.Name, meleeWeapon.X + meleeWeapon.Width / 4, meleeWeapon.Y);
                        }
                        break;
                    case ItemType.Gate:
                        Gate gate = (Gate)item;
                        if(gate.Name == "OutGate" && gate.NearByPlayer(_player, _maps[0].Rooms[0].TileSize))
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
            _drawStatusBoard.Draw(_player);
        }
        public bool IsSetUp
        {
            get { return _isSetUp; }
        }
    }
}
