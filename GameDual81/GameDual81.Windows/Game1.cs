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

        // th current screen that needs to update, either Menu, Loading or gameplay
        Screen currentScreen;

        InputHandler inputHandler;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            inputHandler = new InputHandler();
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(16);
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

            // initialize separate contentmanagers for menu and gamescreen
            MenuScreen.Initialize(new ContentManager(this.Services, "Content"));
            GameScreen.Initialize(new ContentManager(this.Services, "Content"));

            // register this instance of game1 class
            Screen.setHost = this;

            // calculate screenmultiplier
            screenMultiplierHeight = (float)GraphicsDevice.Viewport.Height /1080;
            screenMultiplierWidth = ((float)GraphicsDevice.Viewport.Width + 1)/ 1920;

            Debug.WriteLine(
                "width:  " + GraphicsDevice.Viewport.Width + 
                "\nHeight:  " + GraphicsDevice.Viewport.Height
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

            // Create a menuscreen as the entryscreen when game launches
            currentScreen = new MenuScreen();
            currentScreen.Load();
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
            // TODO: Add your update logic here
            // start by checking update
            inputHandler.Update();
            currentScreen.HandleInput(inputHandler);
            // once input is resolved, update all time based elements
            currentScreen.Update(gameTime.ElapsedGameTime);

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


        public void StartGameScreen(MenuButton B)
        {
            Screen nextScreen = new GameScreen();
            LoadingScreen loading = new LoadingScreen(Content.Load<Texture2D>("loadingbackground"), nextScreen, currentScreen);
            currentScreen = loading;
            currentScreen.Load();
        }
        public void GoToMenu() { }

        public void FinishScreenTransition(Screen nextScreen) 
        {
            currentScreen = nextScreen;
        }
    }
}
