﻿using System;
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
                            case ItemType.Gate:
                                Gate gate = (Gate)item;
                                if(gate.Name == "OutGate")
                                {
                                    gate.Interact(player);
                                    ok = true;
                                    break;
                                }
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
            }
            if (SplashKit.KeyTyped(KeyCode.KKey))
            {
                player.Inventory.IncrementIndex();
 
            }
        }

        // Handle Mouse Input
        public void HandleMouseInput(GameStateManager gameStateManager, ref int characterIndex, int characterMaxIndex
                                   , BuffManager buffManager, ref int buffIndex1, ref int buffIndex2)
        {
            float mouseX = SplashKit.MouseX();
            float mouseY = SplashKit.MouseY();
            bool isLeftClick = SplashKit.MouseClicked(MouseButton.LeftButton);
            bool isRightClick = SplashKit.MouseClicked(MouseButton.RightButton);
            switch (gameStateManager.GetState())
            {
                case GameState.MainMenu:
                    if (isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 650, 990, 700, 775))
                    {
                        gameStateManager.SetState(GameState.GameInstruction);
                    }   else if(isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 650, 990, 600, 675))
                    {
                        gameStateManager.SetState(GameState.ChoosingCharacter);
                    }
                    break;
                case GameState.GameInstruction:
                    if (isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 50, 215, 50, 125))
                    {
                        gameStateManager.SetState(GameState.MainMenu);
                    }
                    break;
                case GameState.ChoosingCharacter:
                    if (isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 50, 215, 50, 125))
                    {
                        gameStateManager.SetState(GameState.MainMenu);
                    } else if (isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 50, 300, 450, 525))
                    {
                        characterIndex--;
                        characterIndex %= characterMaxIndex;
                        if(characterIndex < 0)
                        {
                            characterIndex = characterMaxIndex - 1;
                        }
                    } else if (isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 1380, 1550, 450, 525))
                    {
                        characterIndex++;
                        characterIndex %= characterMaxIndex;
                    } else if (isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 575, 1025, 720, 820))
                    {
                        gameStateManager.SetState(GameState.DuringStage);
                    }
                        break;
                case GameState.BuffPurchase:
                   if (isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 325, 675, 720, 820))
                    {
                        buffManager.UpgradeBuff(buffIndex1);
                        gameStateManager.SetState(GameState.DuringStage);
                    } else if (isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 925, 1275, 720, 820))
                    {
                        buffManager.UpgradeBuff(buffIndex2);
                        gameStateManager.SetState(GameState.DuringStage);
                    }
                    break;
                case GameState.LoseGame:
                    if (isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 50, 215, 50, 125))
                    {
                        gameStateManager.SetState(GameState.MainMenu);
                    }
                    break;
                case GameState.WinGame:
                    if (isLeftClick && PositionValidation.PointInRectangle(mouseX, mouseY, 50, 215, 50, 125))
                    {
                        gameStateManager.SetState(GameState.MainMenu);
                    }
                    break;
            }
        }

        // Update Inputs (Call this every frame)
        public void HandleInput(Player player, Map map, HashSet<Projectile> projectiles,
                                HashSet<Item> items, HashSet<Character> characters,
                                HashSet<Effect> effects,
                                EffectFactory effectFactory, ProjectileFactory projectileFactory)
        {
            HandleKeyboardInput(player, map, projectiles, items, characters, effects, effectFactory, projectileFactory);
            // HandleMouseInput();
        }
    }

}
