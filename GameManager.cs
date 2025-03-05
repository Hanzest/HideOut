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
            _drawMap = new DrawMap("Resource\\Themes");
            _drawText = new DrawText();
            _drawStatusBoard = new DrawStatusBoard();
            _inputHandler = new InputHandler();
            _playerFactory = new PlayerFactory();
            _enemyFactory = new EnemyFactory();
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
            _player = (Player)_playerFactory.Create("alchemist", 200, CharacterType.Player, 250, 0, 20f);
            _characters.Add(_player);
            _characters.Add(_enemyFactory.Create("spider", 300, CharacterType.Enemy, 300, 0, 3f));
            _characters.Add(_enemyFactory.Create("spider", 300, CharacterType.Enemy, 1000, 50, 5f));

            _drawGameObject.AddBitmap("alchemist", ClassType.Character);
            _drawGameObject.AddBitmap("spider", ClassType.Character);
            _drawGameObject.AddBitmap("bulletCollision", ClassType.Effect);
            _drawGameObject.AddBitmap("crawl", ClassType.Effect);
            _drawGameObject.AddBitmap("pistolBullet", ClassType.Projectile);
            _drawGameObject.AddBitmap("Health potion", ClassType.Item);
            _drawGameObject.AddBitmap("Energy potion", ClassType.Item);

            _items.Add(new Potion(ItemType.Potion, "Energy potion", false, 400f, 0f));
            
        }
        public void Update()
        {
            _inputHandler.HandleInput(_player, _maps[0], _projectiles,
                                        _items);
            Console.WriteLine(_maps[0].MapSize);
            foreach (Character character in _characters)
            {
                if(character.Type == CharacterType.Player)
                {
                    character.Tick();
                } else
                {
                    Enemy enemy = (Enemy)character;
                    enemy.FindPlayerNearby(_player, _maps[0].Rooms);
                    if (enemy.IsAttack)
                    {
                        _effects.Add(new Effect("crawl", 
                            _player.Coordinate().X + SplashKit.Rnd(-16, 16),
                            _player.Coordinate().Y + SplashKit.Rnd(-16, 8),
                            15, enemy.FaceLeft));
                    }
                }
            }
            foreach (Item item in _items) 
            {
                switch (item.Type)
                {
                    case ItemType.Potion:
                        Potion potion = (Potion)item;
                        break;
                }
            }
            _camera = _player.Coordinate() - _centering;


            foreach(Projectile projectile in _projectiles)
            {
                projectile.Move(_maps[0].Rooms);
                if (projectile.Collided)
                {
                    _effects.Add(new Effect(projectile.Collision, projectile.X, projectile.Y, 15, false));
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
           _drawGameObject.Draw(_items);
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
                }
            }
            _drawStatusBoard.Draw(_player);
        }
    }
}
