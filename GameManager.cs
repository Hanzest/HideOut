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
        private DrawGameObject _drawGameObject;
        private DrawMap _drawMap;
        //private DrawProjectile _drawProjectile;
        //private DrawItem _drawItem;
        private DrawText _drawText;
        private DrawStatusBoard _drawStatusBoard;
        private InputHandler _inputHandler;
        private ICharacterFactory _playerFactory;
        private ICharacterFactory _enemyFactory;
        private IItemFactory _rangeWeaponFactory;
        private IItemFactory _meleeWeaponFactory;
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
        public GameManager()
        {
            _drawGameObject = new DrawGameObject(
                "Resource\\Characters\\", "Resource\\Effects\\",
                "Resource\\Items\\", "Resource\\Projectiles\\"
                );
            _drawMap = new DrawMap("Resource\\Themes\\");
            _drawText = new DrawText();
            _drawStatusBoard = new DrawStatusBoard("Resource\\Icons\\");
            _inputHandler = new InputHandler();
            _playerFactory = new PlayerFactory();
            _enemyFactory = new EnemyFactory();
            _effectFactory = new EffectFactory();
            _rangeWeaponFactory = new RangeWeaponFactory();
            _meleeWeaponFactory = new MeleeWeaponFactory();
            _projectileFactory = new ProjectileFactory();
            _camera = new Point2D(0, 0);
            _centering = new Point2D(800f, 480f);
            _characters = new HashSet<Character>();
            _maps = new List<Map>();
            _projectiles = new HashSet<Projectile>();
            _effects = new HashSet<Effect>();
            _items = new HashSet<Item>();
        }
        public void SetUp()
        {
            _maps.Add(new Map());
            _maps[0].GenerateMap();
            //_characters.Add(_enemyFactory.Create("mantis", 200, 50));
            //_characters.Add(_enemyFactory.Create("mantis", 200, 50));
            //_characters.Add(_enemyFactory.Create("mantis", 200, 50));
            //_characters.Add(_enemyFactory.Create("mantis", 200, 50));
            _drawGameObject.AddBitmap("alchemist", ClassType.Character);
            _drawGameObject.AddBitmap("spider", ClassType.Character);
            _drawGameObject.AddBitmap("mantis", ClassType.Character);
            _drawGameObject.AddBitmap("zombie", ClassType.Character);
            _drawGameObject.AddBitmap("bulletCollision", ClassType.Effect);
            _drawGameObject.AddBitmap("scratch", ClassType.Effect);
            _drawGameObject.AddBitmap("pistolBullet", ClassType.Projectile);
            _drawGameObject.AddBitmap("shotgunBullet", ClassType.Projectile);
            _drawGameObject.AddBitmap("Health potion", ClassType.Item);
            _drawGameObject.AddBitmap("Energy potion", ClassType.Item);
            _drawGameObject.AddBitmap("Iron Sword", ClassType.Item);
            _drawGameObject.AddBitmap("paleslash", ClassType.Effect);
            _drawGameObject.AddBitmap("revolver", ClassType.Item);
            _drawGameObject.AddBitmap("sniper", ClassType.Item);
            _drawGameObject.AddBitmap("sniperBullet", ClassType.Projectile);
            _drawGameObject.AddBitmap("rifle", ClassType.Item);
            _drawGameObject.AddBitmap("rifleBullet", ClassType.Projectile);  
            _drawGameObject.AddBitmap("Light Saber", ClassType.Item);
            _drawGameObject.AddBitmap("slash", ClassType.Effect);
            _drawGameObject.AddBitmap("sawed-off shotgun", ClassType.Item);

            _items.Add(new Potion(ItemType.Potion, "Energy potion", false, 400f, 0f));
            _items.Add(_meleeWeaponFactory.Create("Iron Sword", 600f, 0));
            _items.Add(_rangeWeaponFactory.Create("sawed-off shotgun", 800f, 0));
            //_items.Add(_meleeWeaponFactory.Create("Iron Sword", 1000f, 0));
            _items.Add(_rangeWeaponFactory.Create("sniper", 300f, 0));
            _items.Add(_meleeWeaponFactory.Create("Light Saber", 1000f, 0));
            //_items.Add(new RangeWeapon(ItemType.RangeWeapon, "sawed-off shotgun", "shotgunBullet", "null", 1000f, 0f,
            //                        20, 0f, 200));
            //_items.Add(new RangeWeapon(ItemType.RangeWeapon, "sawed-off shotgun", "shotgunBullet", "null", 1200f, 0f,
            //                        20, 0f, 200));

            _player = (Player)_playerFactory.Create("alchemist", 250, 0);
            _characters.Add(_player);
            _characters.Add(_enemyFactory.Create("zombie", 2000, 0));
            foreach (Character character in _characters)
            {
                if (character.Type == CharacterType.RangeEnemy)
                {
                    RangeEnemy rangeEnemy = (RangeEnemy)character;
                    _items.Add(rangeEnemy.Inventory.GetItem);
                    Console.WriteLine(rangeEnemy.Inventory.GetItem.Name);
                    Console.WriteLine(rangeEnemy.Inventory.GetItem.InInventory);
                    Console.WriteLine(rangeEnemy.Inventory.GetItem.Display);
                }
            }
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
                    _characters.Remove(character);
                }
                switch (character.Type)
                {
                    case CharacterType.Player:
                        character.Tick();
                        Player p = (Player)character;
                        if(_maps[0].Rooms[p.RoomNumber].RoomNumber == 2 && !(_maps[0].Rooms[p.RoomNumber].IsClear) && !_maps[0].Rooms[p.RoomNumber].IsPlayerEnter){
                            p.X += 60;
                            _maps[0].Rooms[p.RoomNumber].Lock();
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
                switch (item.Type)
                {
                    case ItemType.Potion:
                        Potion potion = (Potion)item;
                        break;
                    // I just remove from here
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
                }
            }
            _drawStatusBoard.Draw(_player);
            foreach (Item item in _items)
            {
                if(item.InInventory && item.Display)
                {
                    _drawGameObject.DrawPrioritizedItem(item, item.Angle);
                }
                ;
            }
            
        }
    }
}
