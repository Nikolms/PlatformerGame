using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ThielynGame.Screens;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Diagnostics;
using ThielynGame.Menu;
using Microsoft.Xna.Framework.Input;
using System;
using Windows.UI.Xaml;

namespace ThielynGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        

        // multiplier used to scale things to screenresolution
        public static float screenMultiplierWidth, screenMultiplierHeight;

        // the current screen that needs to update, either Menu, Loading or gameplay
        Screen currentScreen;

        InputHandler inputHandler;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            inputHandler = new InputHandler();
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(16);

            GameSettings.ArmorUpgrade = 0;
            GameSettings.MeleeUpgrade = 0;
            GameSettings.RangedUpgrade = 0;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // calculate screenmultiplier
            screenMultiplierHeight = (float)GraphicsDevice.Viewport.Height /768;
            screenMultiplierWidth = (float)GraphicsDevice.Viewport.Width / 1280;

            Debug.WriteLine(
                "width:  " + GraphicsDevice.Viewport.Width + 
                "\nHeight:  " + GraphicsDevice.Viewport.Height +
                "\nWidthMulti:  " + screenMultiplierWidth
                );

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            CommonAssets.menuButtonBackground = Content.Load<Texture2D>("menubuttonbackground");
            CommonAssets.menuFont = Content.Load<SpriteFont>("menufont");
            CommonAssets.LoadingBackGround = Content.Load<Texture2D>("loadingbackground");

            // Create a menuscreen as the entryscreen when game launches
            //currentScreen = new MenuScreen(this);
            GoToMenuScreen();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            // check for any input values
            inputHandler.Update();

            // update current screen
            currentScreen.Update(gameTime.ElapsedGameTime, inputHandler);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            currentScreen.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void GoToGameScreen(MenuButton B)
        {
            GameScreen game = new GameScreen(this);
            currentScreen = game;
        }

        public void GoToMenuScreen() 
        {
            MenuScreen menu = new MenuScreen(this);
            currentScreen = menu;
        }

        public void ExitApplication(MenuButton G) 
        {
            Application.Current.Exit();
        }

    }
}
