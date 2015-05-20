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
        // a separate flag to keep track of levelgeneration and other
        // tasks that happen without screen switch
        bool isBusy;

        Texture2D levelBackGround_close, levelBackGround_mid, levelBackGround_deep;
        ObjectManager objectManager;
        TextureLoader GamePlayTextures;
        LevelGUI levelGUI;
        Player player;

        // this keeps track of current level.
        // increases everytime a level is completed
        int levelCounter;

        public GameScreen(Game1 game1) : base(game1)
        {
            isLoading = true;

            player = new Player(new Vector2(100,100));
            // Initialize all the "services"
            levelGUI = new LevelGUI(player);

            // create objectManager to hold and perform updates on gameobjects
            objectManager = new ObjectManager(player);

            // create the helper class to load textures from
            GamePlayTextures = new TextureLoader(content);

            // create the animationlists
            AnimationLists.Initialize();

            levelCounter = 0;

            // TODO should not be in constructor maybe
            StartANewLevel();
            Load();

            // TODO these objects are for testing, implement level contructors for modular endless levels
            Troll T = new Troll(new Vector2(600, 100));
            Troll T1 = new Troll(new Vector2(1200, 100));
            Troll T2 = new Troll(new Vector2(2000, 100));
            Troll T3 = new Troll(new Vector2(1600, 100));

            objectManager.AddGameObject(T);
        }

        async void StartANewLevel() 
        {
            // set the busy flag so that loading screen is displayed
            isBusy = true;

            //increase levelCounter everytime new level is started
            levelCounter++;
            objectManager.StartLevel(10, levelCounter);

            // wait for a while to prevent loading screen from flashing to fast
            await Task.Delay(TimeSpan.FromSeconds(1));

            // release flag when level has been created
            isBusy = false;
        }

        async void Load()
        {
            levelBackGround_close = content.Load<Texture2D>("cave_close_background");
            levelBackGround_mid = content.Load<Texture2D>("cave_mid_background");
            levelBackGround_deep = content.Load<Texture2D>("cave_deep_background");

            await Task.Delay(TimeSpan.FromSeconds(1));

            isLoading = false;
        }

        public override void HandleInput(InputHandler input)
        {
            levelGUI.checkButtonClicks(input.InputLocations);
            levelGUI.PlayerNonVisualInput(input);

            if (input.ExitGame_Input) 
                ExitScreen();
        }

        public override void Update(TimeSpan time)
        {
            // do not update game world while loading or creating new level
            if (!isBusy && !isLoading)
            objectManager.Update(time);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch s)
        {
            if (isBusy || isLoading) s.Draw(CommonAssets.LoadingBackGround, Vector2.Zero, Color.White);

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
