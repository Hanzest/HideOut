using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private float _mouseX;
        private float _mouseY;

        public DrawGameState()
        {
            _inputHandler = new InputHandler();
            _gameStateManager = new GameStateManager();
            _position = new Point2D(0, 0);
            _drawText = new DrawText();
            _drawingComponent = new DrawingComponent();
            _gameManager = new GameManager();
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
                case GameState.DuringStage:
                    DrawDuringStage();
                    break;
            }
        }
        public void UpdateGameState()
        {
            _inputHandler.HandleMouseInput(_gameStateManager);
        }
        public void DrawMainMenu()
        {
            _drawingComponent.DrawRectangle(Color.RGBColor(117, 124, 106), Color.RGBColor(0, 123, 53), 450f, -50f, 740, 250);
            _drawText.DrawMontserratH1Custom("Hide Out", 800, 50, Color.RGBColor(255, 255, 255));
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1),
                                                    650f, 600f, 340, 75);
            _drawText.DrawMontserratH3Custom("Start Game", 800, 610, Color.RGBColor(0, 0, 0));

            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1),
                                                650f, 700f, 340, 75);
            _drawText.DrawMontserratH3Custom("Instructions", 810, 710, Color.RGBColor(0, 0, 0));
        }

        public void DrawGameInstruction()
        {
            _drawingComponent.DrawHoveringRectangle(_mouseX, _mouseY, Color.RGBColor(117, 124, 106), Color.RGBColor(165, 255, 1), Color.RGBColor(105, 195, 1), 50, 50, 165, 75);
            _drawText.DrawMontserratH3Custom("Back", 120, 60, Color.RGBColor(0, 0, 0));
        }
        public void DrawDuringStage()
        {
            if (!_gameManager.IsSetUp)
            {
                _gameManager.SetUp();
            } else
            {
                _gameManager.Update();
                _gameManager.Draw();
            }
        }
        public void DrawChoosingCharacter()
            // Previous [ Card ] Next
            //         [ Select ]
        {

        }

    }
}
