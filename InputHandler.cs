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
                                        HashSet<Item> items)
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
                    if(item.NearByPlayer(player, map.Rooms[0].TileSize) &&
                        !item.InInventory)
                    {
                        switch (item.Type)
                        {
                            case ItemType.Potion:
                                Potion potion = (Potion)item;
                                potion.Use(player);
                                ok = true;
                                break;
                        }
                    }
                }
                if (!ok)
                {
                    projectiles.Add(new Projectile("bulletCollision", "pistol", player.X, player.Y, 
                                                16f, 25, false, 1800, 0));
                    player.EnergyChanged(-10);
                }
                
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
                                HashSet<Item> items)
        {
            HandleKeyboardInput(player, map, projectiles, items);
            HandleMouseInput();
        }
    }

}
