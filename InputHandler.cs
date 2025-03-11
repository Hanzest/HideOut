using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class InputHandler
    {
        // Store key states
        private Dictionary<KeyCode, bool> _keyStates = new Dictionary<KeyCode, bool>();

        public InputHandler()
        {
            // Initialize key states to false
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                _keyStates[key] = false;
            }
        }

        // Handle Keyboard Input
        public void HandleKeyboardInput(Player player, Map map, HashSet<Projectile> projectiles,
                                        HashSet<Item> items, HashSet<Character> characters,
                                        HashSet<Effect> effects,
                                        EffectFactory effectFactory, ProjectileFactory projectileFactory)
        {
            if (SplashKit.KeyDown(KeyCode.WKey)) player.Move(Direction.Up, map.Rooms, 96, 128);
            if (SplashKit.KeyDown(KeyCode.SKey)) player.Move(Direction.Down, map.Rooms, 96, 128);
            if (SplashKit.KeyDown(KeyCode.AKey)) player.Move(Direction.Left, map.Rooms, 96, 128);
            if (SplashKit.KeyDown(KeyCode.DKey)) player.Move(Direction.Right, map.Rooms, 96, 128);
            if (SplashKit.KeyTyped(KeyCode.JKey))
            {
                bool ok = false;
                foreach (Item item in items)
                {
                    if (item.NearByPlayer(player, map.Rooms[0].TileSize) &&
                        !item.InInventory)
                    {
                        switch (item.Type)
                        {
                            case ItemType.Potion:
                                Potion potion = (Potion)item;
                                potion.UseBy(player);
                                ok = true;
                                break;
                            case ItemType.RangeWeapon:
                                RangeWeapon rWeapon = (RangeWeapon)item;
                                rWeapon.UseBy(player, player.NearestEnemy(characters), effects,
                                    effectFactory, projectiles, projectileFactory);
                                player.Inventory.Add(rWeapon, player);
                                ok = true;
                                break;
                            case ItemType.MeleeWeapon:
                                MeleeWeapon mWeapon = (MeleeWeapon)item;
                                mWeapon.UseBy(player, player.NearestEnemy(characters), effects,
                                    effectFactory, projectiles, projectileFactory);
                                player.Inventory.Add(mWeapon, player);
                                ok = true;
                                break;
                        }
                        if (ok) break;
                    } else if (item.InInventory && player.Inventory.GetItem == item)
                    {
                        switch (item.Type)
                        {
                            case ItemType.RangeWeapon:
                                RangeWeapon weapon = (RangeWeapon)item;
                                weapon.UseBy(player, player.NearestEnemy(characters), effects,
                                    effectFactory, projectiles, projectileFactory);
                                break;
                            case ItemType.MeleeWeapon:
                                MeleeWeapon mWeapon = (MeleeWeapon)item;
                                mWeapon.UseBy(player, player.NearestEnemy(characters), effects,
                                    effectFactory, projectiles, projectileFactory);
                                break;
                        }
                    }
                }
                if (!ok)
                {
                    //projectiles.Add(new Projectile("bulletCollision", "pistol", player.X, player.Y, 
                    //                            16f, 25, false, 1800, 0));
                    //player.EnergyChanged(-10);
                }
                
            }
            if (SplashKit.KeyTyped(KeyCode.KKey))
            {
                player.Inventory.IncrementIndex();
 
            }
        }

        // Handle Mouse Input
        public void HandleMouseInput()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Console.WriteLine($"Left Click at {SplashKit.MousePosition()}");
            }
            if (SplashKit.MouseClicked(MouseButton.RightButton))
            {
                Console.WriteLine($"Right Click at {SplashKit.MousePosition()}");
            }
        }

        // Update Inputs (Call this every frame)
        public void HandleInput(Player player, Map map, HashSet<Projectile> projectiles,
                                HashSet<Item> items, HashSet<Character> characters,
                                HashSet<Effect> effects,
                                EffectFactory effectFactory, ProjectileFactory projectileFactory)
        {
            HandleKeyboardInput(player, map, projectiles, items, characters, effects, effectFactory, projectileFactory);
            HandleMouseInput();
        }
    }

}
