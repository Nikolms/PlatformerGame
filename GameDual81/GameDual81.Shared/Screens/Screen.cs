using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.Screens
{
    
    public abstract class Screen
    {
        // set this to true is screen is done loading
        public bool isLoading { get; protected set; }

        protected ContentManager content;


        public Game1 ContainingClass { get; protected set; }

        public Screen(Game1 game1) 
        {
            content = new ContentManager(game1.Services,"Content");
            ContainingClass = game1;
        }

        // all screens should handle input
          public abstract void HandleInput(InputHandler inputhandler);

        // all screens need an update logic
          public abstract void Update(TimeSpan time);

        // all screen need a draw logic
          public abstract void Draw(SpriteBatch s);

        // all screen need a method that handles exiting the screen
          public abstract void ExitScreen();
    }
}
