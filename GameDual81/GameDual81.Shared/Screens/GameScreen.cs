using ThielynGame.AnimationFiles;
using ThielynGame.GamePlay;
using ThielynGame.LevelGenerator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using ThielynGame.GamePlay.EnemyTypes;

namespace ThielynGame.Screens
{
    public enum GameState { Playing, FinishedLevel, Popup}

    class GameScreen : Screen
    {

        Texture2D levelBackGround_close, levelBackGround_mid, levelBackGround_deep;
        LevelManager objectManager;
        TextureLoader GamePlayTextures;
        LevelGUI levelGUI;

        public GameScreen(Game1 game1) : base(game1)
        {
            isLoading = true;

            // create the player instance that is used by GUI
            // and levelmanager
            Player player = new Player(Vector2.Zero);

            // create objectManager to hold and perform updates on gameobjects
            objectManager = new LevelManager();
            objectManager.Initialize(player);

            // create the GUI
            levelGUI = new LevelGUI(player);

            // create the helper class to load textures from
            GamePlayTextures = new TextureLoader(content);

            // create the animationlists
            AnimationLists.Initialize();

            Task T = new Task(Load);
            T.Start();
        }

        async void Load()
        {
            levelBackGround_close = content.Load<Texture2D>("cave_close_background");
            levelBackGround_mid = content.Load<Texture2D>("cave_mid_background");
            levelBackGround_deep = content.Load<Texture2D>("cave_deep_background");

            await Task.Delay(TimeSpan.FromSeconds(1));

            isLoading = false;
        }

        public override void Update(TimeSpan time, InputHandler input)
        {
            // check following flags if gameworld is ready for updates
            // if still loading abort function
            if (isLoading) return;
            if (objectManager.IsCreatingLevel) return;

            // read input information
            levelGUI.checkButtonClicks(input.InputLocations);
            levelGUI.PlayerNonVisualInput(input);

            if (input.ExitGame_Input)
                ExitScreen();

            // update all game objects
            objectManager.Update(time);

            if (objectManager.gameOver) ExitScreen(); 
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch s)
        {
            if (objectManager.IsCreatingLevel || isLoading) s.Draw(CommonAssets.LoadingBackGround, Vector2.Zero, Color.White);

            else
            {

                s.Draw(levelBackGround_deep, MyRectangle.AdjustSizeCustomRectangle(0, 0, 1280, 768), Color.White);
                s.Draw(levelBackGround_mid, MyRectangle.AdjustSizeCustomRectangle(0, 0, 1280, 768), Color.White);
                s.Draw(levelBackGround_close, MyRectangle.AdjustSizeCustomRectangle(0, 0, 1280, 768), Color.White);

                objectManager.Draw(s, GamePlayTextures);

                // UI should be drawn last so that it is always visible
                levelGUI.Draw(s, GamePlayTextures);
            }
        }

        public override void ExitScreen()
        {
            content.Unload();
            ContainingClass.GoToMenuScreen();
        }
    }
}
