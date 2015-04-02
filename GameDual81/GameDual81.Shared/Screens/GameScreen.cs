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
        static ContentManager content;
        Texture2D levelBackGround_close, levelBackGround_mid, levelBackGround_deep;
        ObjectManager objectManager;
        TextureLoader GamePlayTextures;
        LevelGUI levelGUI;
        Player player;

        // this keeps track of current level.
        // increases everytime a level is completed
        int levelCounter;

        public GameScreen() 
        {
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

            // TODO these objects are for testing, implement level contructors for modular endless levels
            Troll T = new Troll(new Vector2(600, 100));
            Troll T1 = new Troll(new Vector2(1200, 100));
            Troll T2 = new Troll(new Vector2(2000, 100));
            Troll T3 = new Troll(new Vector2(1600, 100));

            ObjectManager O1 = new ObjectManager();
            ObjectManager O2 = new ObjectManager();
            ObjectManager O3 = new ObjectManager();
            ObjectManager O4 = new ObjectManager();

            O1.AddGameObject(T);
            //O1.AddGameObject(T1);
            //O1.AddGameObject(T2);
            //O1.AddGameObject(T3);
        }

        public void StartANewLevel() 
        {
            //increase levelCounter everytime new level is started
            levelCounter++;

            objectManager.clearManager();
            objectManager.StartLevel(10,2, levelCounter);
        }


        public static void Initialize(ContentManager CM) 
        {
            content = CM;
        }

        public override void Load()
        {
            levelBackGround_close = content.Load<Texture2D>("cave_close_background");
            levelBackGround_mid = content.Load<Texture2D>("cave_mid_background");
            levelBackGround_deep = content.Load<Texture2D>("cave_deep_background");
        }

        public override void HandleInput(InputHandler input)
        {
            levelGUI.checkButtonClicks(input.InputLocations);
            levelGUI.PlayerNonVisualInput(input);
        }

        public override void Update(TimeSpan time)
        {
            objectManager.Update(time);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch s)
        {
            s.Draw(levelBackGround_deep, MyRectangle.AdjustSizeCustomRectangle(0,0,1280,768), Color.White);
            s.Draw(levelBackGround_mid, MyRectangle.AdjustSizeCustomRectangle(0, 0, 1280, 768), Color.White);
            s.Draw(levelBackGround_close, MyRectangle.AdjustSizeCustomRectangle(0, 0, 1280, 768), Color.White);

            objectManager.Draw(s, GamePlayTextures);

            // UI should be drawn last so that it is always visible
            levelGUI.Draw(s, GamePlayTextures);
        }

        public override void ExitScreen()
        {
            objectManager.clearManager();
            content.Unload();
        }
    }
}
