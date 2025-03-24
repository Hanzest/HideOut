using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SplashKitSDK;

namespace HideOut
{
    public class DrawGameState
    {
        private GameStateManager _gameStateManager;
        private DrawText _drawText;
        private InputHandler _inputHandler;
        private Point2D _position;
        private DrawingComponent _drawingComponent;
        private GameManager _gameManager;
        private Saver _saver;
        private float _mouseX;
        private float _mouseY;
           // Character Choosing
        private List<Bitmap> _bitmaps = new List<Bitmap>();
        private CharacterLoader _characterLoader;
        private int _characterIndex;
        private bool _isStartGame;
        // Game Play
        private int _theme;
        // Buff Purchase
        private BuffManager _buffManager;
        private List<Bitmap> _buffBitmaps = new List<Bitmap>();
        private BuffLoader _buffLoader;
        private int _buffIndex1;
        private int _buffIndex2;
        private Bitmap _bmpCoin;
        // Win Game
        private bool _isWinGame;
        public DrawGameState()
        {
            _characterIndex = 0;
            _isStartGame = false;
            _isWinGame = false;
            _theme = SplashKit.Rnd(0, 3);
            _bitmaps = new List<Bitmap>();
            _buffBitmaps = new List<Bitmap>();
            _characterLoader = new CharacterLoader();
            _buffLoader = new BuffLoader();
            _buffManager = new BuffManager();
            _inputHandler = new InputHandler();
            _gameStateManager = new GameStateManager();
            _position = new Point2D(0, 0);
            _drawText = new DrawText();
            _saver = new Saver();
            _drawingComponent = new DrawingComponent();
            _gameManager = new GameManager();
            _characterLoader.Load(_bitmaps);
            _buffLoader.Load(_buffBitmaps);
            _buffIndex1 = SplashKit.Rnd(0, _buffBitmaps.Count);
            _buffIndex2 = SplashKit.Rnd(0, _buffBitmaps.Count);
            _bmpCoin = new Bitmap("Coin", "Resource\\Icons\\Coin.png");
        }

        public void Draw()
        {
            SplashKit.SetCameraPosition(_position.ToSplashKitPoint());
            _mouseX = SplashKit.MouseX();
            _mouseY = SplashKit.MouseY();
            GameState currentState = _gameStateManager.GetState();
            switch (currentState)
            {
                case GameState.MainMenu:
                    DrawMainMenu();
                    break;
                case GameState.GameInstruction:
                    DrawGameInstruction();
                    break;
                case GameState.ChoosingCharacter:
                    DrawChoosingCharacter();
                    break;
                case GameState.NotAbleToContinue:
                    DrawNotAbleToContinue();
                    break;
                case GameState.DuringStage:
                    DrawDuringStage();
                    break;
                case GameState.BuffPurchase:
                    DrawBuffPurchase();
                    break;
                case GameState.LoseGame:
                    DrawGameOver();
                    break;
                case GameState.WinGame:
                    DrawWinGame();
                    break;
            }
        }
        public void UpdateGameState()
        {
            
            _inputHandler.HandleMouseInput(_gameStateManager, ref _characterIndex, _bitmaps.Count, _buffManager, ref _buffIndex1, ref _buffIndex2, ref _isStartGame, _saver);
        }
        public void DrawMainMenu()
        {
            _isWinGame = false;
            _saver.IsAbleToContinue = true;
            _drawingComponent.DrawRectangle(Color.RGBColor(117, 124, 106), Color.RGBColor(0, 123, 53), 450f, -50f, 740, 250);
            _drawText.DrawMontserratH1Custom("Hide Out", 800, 50, Color.RGBColor(255, 255, 255));
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1),
                                                    650f, 600f, 340, 75);
            _drawText.DrawMontserratH3Custom("Start Game", 800, 610, Color.RGBColor(0, 0, 0));

            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1),
                                                650f, 700f, 340, 75);
            _drawText.DrawMontserratH3Custom("Continue", 805, 710, Color.RGBColor(0, 0, 0));
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1),
                                                650f, 800f, 340, 75);
            _drawText.DrawMontserratH3Custom("Instructions", 810, 810, Color.RGBColor(0, 0, 0));
        }

        public void DrawGameInstruction()
        {
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1), 50, 50, 165, 75);
            _drawText.DrawMontserratH3Custom("Back", 120, 60, Color.RGBColor(0, 0, 0));
        }
        public void DrawDuringStage()
        {
            if (!_gameManager.IsBuffUpdated)
            {
                _gameManager.UpdateBuff(_buffManager);
            }
            else if (!_gameManager.IsSetUp)
            {
                _theme = SplashKit.Rnd(0, 3);
                _gameManager.SetUp(_bitmaps[_characterIndex].Name, _theme, _saver, ref _isStartGame, ref _buffManager);
            } else 
            {
                _gameManager.Update();
                
                if(_gameManager.Saver.IsAbleToContinue == false)
                {
                    _gameStateManager.SetState(GameState.NotAbleToContinue);
                }
                if (_gameManager.IsNotLost == false)
                // player is lost
                {
                    _gameStateManager.SetState(GameState.LoseGame);
                }
                else if (_gameManager.Saver.IsSaved == true)
                {
                    if (_gameManager.Saver.Level <= 10)
                    {
                        _saver.IsSaved = false;
                        _gameStateManager.SetState(GameState.BuffPurchase);
                    }
                    else
                    {
                        _gameStateManager.SetState(GameState.WinGame);
                    }
                }
                else
                {
                    _gameManager.Draw();
                }
            }
        }
        public void DrawChoosingCharacter()
        {
            _isStartGame = true;
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1), 50, 50, 165, 75);
            _drawText.DrawMontserratH3Custom("Back", 120, 60, Color.RGBColor(0, 0, 0));
            // Back Btn
            int cardX = 600;
            int cardY = 220;
            int cardWidth = 400;
            int cardHeight = 600;
            _drawingComponent.DrawRectangle(Color.White, Color.RGBColor(0, 134, 96),
                                             cardX, cardY, cardWidth, cardHeight);
            // Big Card
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(0, 216, 68), Color.RGBColor(0, 118, 38), cardX + 25, cardY + cardHeight - 100, cardWidth - 50, 75);
            _drawText.DrawMontserratH3Custom("Start Game", 780, cardY + cardHeight - 90, Color.RGBColor(0, 0, 0));
            // Start Game Button
            Bitmap currentBmp = _bitmaps[_characterIndex];

            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(76, 144, 155), Color.RGBColor(56, 108, 112),
                50, 450, 250, 75);
            _drawText.DrawMontserratH3Custom("Previous", 160, 460, Color.Black);
            // Previous Button
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(76, 144, 155), Color.RGBColor(56, 108, 112),
                1380, 450, 170, 75);
            _drawText.DrawMontserratH3Custom("Next", 1450, 460, Color.Black);
            // Next Button
            int x = 630;
            int y = 350;
            int headerY = 250;
            int headerX = 790;
            switch (currentBmp.Name)
            {
                //case "alchemist":
                //    return new Player(name, 150, 200, 60, CharacterType.Player, x, y, 10f, 96, 128);
                //case "wizard":
                //    return new Player(name, 100, 300, 40, CharacterType.Player, x, y, 10f, 96, 128);
                //case "cowboy":
                //    return new Player(name, 125, 150, 65, CharacterType.Player, x, y, 12f, 96, 128);
                case "alchemist":
                    _drawText.DrawMontserratH2Custom("Alchemist", headerX, headerY, Color.RGBColor(0, 0, 0));
                    _drawText.DrawMontserratH4LeftAlign("HP: 150", x, y, Color.Black);
                    _drawText.DrawMontserratH4LeftAlign("Energy: 200", x, y + 50, Color.Black);
                    _drawText.DrawMontserratH4LeftAlign("Armor: 60", x, y + 100, Color.Black);
                    _drawText.DrawMontserratH4LeftAlign("Speed: 10", x, y + 150, Color.Black);
                    break;
                case "wizard":
                    _drawText.DrawMontserratH2Custom("Wizard", headerX, headerY, Color.RGBColor(0, 0, 0));
                    _drawText.DrawMontserratH4LeftAlign("HP: 100", x, y, Color.Black);
                    _drawText.DrawMontserratH4LeftAlign("Energy: 300", x, y + 50, Color.Black);
                    _drawText.DrawMontserratH4LeftAlign("Armor: 40", x, y + 100, Color.Black);
                    _drawText.DrawMontserratH4LeftAlign("Speed: 10", x, y + 150, Color.Black);
                    break;
                case "cowboy":
                    _drawText.DrawMontserratH2Custom("Cowboy", headerX, headerY, Color.RGBColor(0, 0, 0));
                    _drawText.DrawMontserratH4LeftAlign("HP: 125", x, y, Color.Black);
                    _drawText.DrawMontserratH4LeftAlign("Energy: 150", x, y + 50, Color.Black);
                    _drawText.DrawMontserratH4LeftAlign("Armor: 65", x, y + 100, Color.Black);
                    _drawText.DrawMontserratH4LeftAlign("Speed: 12", x, y + 150, Color.Black);
                    break;
            }
            currentBmp.Draw(800 - currentBmp.Width/2, cardY + cardHeight - 250);
        }
        public void DrawNotAbleToContinue()
        {
            if (!_gameManager.IsFreeAll)
            {
                _gameManager.FreeAll();
                _saver.SaveLost();
            }
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1), 50, 50, 165, 75);
            _drawText.DrawMontserratH3Custom("Back", 120, 60, Color.RGBColor(0, 0, 0));

            _drawingComponent.DrawRectangle(Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), 400, 450, 800, 150);
            _drawText.DrawMontserratH1Custom("No Save Found", 725, 465, Color.RGBColor(0, 0, 0));
        }
        public void DrawBuffPurchase()
        {
            if (!_saver.IsSaved)
                // if the gameManager information is saved
            {
                _saver = _gameManager.Saver;
                _buffIndex1 = SplashKit.Rnd(0, _buffBitmaps.Count);
                _buffIndex2 = SplashKit.Rnd(0, _buffBitmaps.Count);
                _gameManager.FreeAll();
                // clear data of game Manager, set local saver to false
                _saver.IsSaved = true;
            }
            int cnt = 0;
            while ((_buffIndex1 == _buffIndex2 || _buffManager.GetBuffIndex(_buffIndex1) == 2 || _buffManager.GetBuffIndex(_buffIndex2) == 2) && cnt < 1000)
            {
                cnt++;
                _buffIndex1 = SplashKit.Rnd(0, _buffBitmaps.Count);
                _buffIndex2 = SplashKit.Rnd(0, _buffBitmaps.Count);
            }
            if (cnt >= 1000)
            {
                _gameStateManager.SetState(GameState.DuringStage);
            }
            Bitmap buff1 = _buffBitmaps[_buffIndex1];
            Bitmap buff2 = _buffBitmaps[_buffIndex2];
            string buff1Name = _buffManager.GetBuffString(_buffIndex1);
            string buff2Name = _buffManager.GetBuffString(_buffIndex2);
            int cardX1 = 300;
            int cardY1 = 120;
            int cardX2 = 900;
            int cardY2 = 120;
            int cardWidth = 400;
            int cardHeight = 600;
            _drawingComponent.DrawRectangle(Color.White, Color.RGBColor(0, 134, 96),
                                             cardX1, cardY1, cardWidth, cardHeight);
            // Big Card 1
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(0, 216, 68), Color.RGBColor(0, 118, 38), cardX1 + 25, cardY1 + cardHeight - 100, cardWidth - 50, 75);
            _drawText.DrawMontserratH3Custom("Choose", cardX1 + cardWidth / 2 - 20, cardY1 + cardHeight - 90, Color.RGBColor(0, 0, 0));
            // Buy Button 1
            _drawingComponent.DrawRectangle(Color.White, Color.RGBColor(0, 134, 96),
                                             cardX2, cardY2, cardWidth, cardHeight);
            // Big Card 2
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(0, 216, 68), Color.RGBColor(0, 118, 38), cardX2 + 25, cardY2 + cardHeight - 100, cardWidth - 50, 75);
            _drawText.DrawMontserratH3Custom("Choose", cardX2 + cardWidth / 2 - 20, cardY2 + cardHeight - 90, Color.RGBColor(0, 0, 0));
            // Buy Button 2
            if (4 <= _buffIndex1 && _buffIndex1 <= 6)
            {
                _drawText.DrawMontserratH4Custom("Enemy Debuff", cardX1 + cardWidth / 2 - 30, cardY1 + 300, Color.Black);
            }
            if (4 <= _buffIndex2 && _buffIndex2 <= 6)
            {
                _drawText.DrawMontserratH4Custom("Enemy Debuff", cardX2 + cardWidth / 2 - 30, cardY2 + 300, Color.Black);
            }
            buff1.DrawScale(cardX1 + cardWidth / 2 - 10, cardY1 + 135, 10, 10);
            buff2.DrawScale(cardX2 + cardWidth / 2 - 10, cardY2 + 135, 10, 10);
            _drawText.DrawMontserratH4LeftAlign(buff1Name, cardX1 + 25, cardY1 + 375, Color.Black);
            _drawText.DrawMontserratH4LeftAlign($"Level: {_buffManager.GetBuffIndex(_buffIndex1)} (Max: 2)", cardX1 + 25, cardY1 + 425, Color.Black);

            _drawText.DrawMontserratH4LeftAlign(buff2Name, cardX2 + 25, cardY1 + 375, Color.Black);
            _drawText.DrawMontserratH4LeftAlign($"Level: {_buffManager.GetBuffIndex(_buffIndex2)} (Max: 2)", cardX2 + 25, cardY1 + 425, Color.Black);

            _drawText.DrawH1($"{_saver.Coin}", 1300 + 50, 40 + 68);
            _bmpCoin.Draw(1300, 40);
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(0, 216, 68), Color.RGBColor(0, 118, 38), 550, 750, 500, 150);
            _drawText.DrawMontserratH4Custom("Heal 20HP for 50 Coins", 785, 775, Color.Black);
            _drawText.DrawMontserratH4Custom($"Current HP: {_saver.Health} / {_saver.MaxHealth}", 800, 835, Color.Black);
        }   
        public void DrawGameOver()
        {
            if (!_gameManager.IsNotLost)
            {
                _buffIndex1 = SplashKit.Rnd(0, _buffBitmaps.Count);
                _buffIndex2 = SplashKit.Rnd(0, _buffBitmaps.Count);
                _gameManager.FreeAll();
                _gameManager = new GameManager();
                _saver.SaveLost();
            }
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1), 50, 50, 165, 75);
            _drawText.DrawMontserratH3Custom("Back", 120, 60, Color.RGBColor(0, 0, 0));


            _drawingComponent.DrawRectangle(Color.RGBColor(117, 124, 106), Color.RGBColor(0, 123, 53), 500, 450, 600, 150);
            _drawText.DrawMontserratH1Custom("Game Over", 735, 465, Color.RGBColor(0, 0, 0));
        }
        public void DrawWinGame()
        {
            if (!_isWinGame)
            {
                _buffIndex1 = SplashKit.Rnd(0, _buffBitmaps.Count);
                _buffIndex2 = SplashKit.Rnd(0, _buffBitmaps.Count);
                _gameManager.FreeAll();
                _gameManager = new GameManager();
                _saver = _gameManager.Saver;
                _saver.SaveFinish();
                _isWinGame = true;
            }
            
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1), 50, 50, 165, 75);
            _drawText.DrawMontserratH3Custom("Back", 120, 60, Color.RGBColor(0, 0, 0));


            _drawingComponent.DrawRectangle(Color.RGBColor(117, 124, 106), Color.RGBColor(0, 123, 53), 500, 450, 600, 150);
            _drawText.DrawMontserratH1Custom("You Win!", 745, 465, Color.RGBColor(0, 0, 0));
        }
    }
}
