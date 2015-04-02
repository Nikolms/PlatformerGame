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
        public bool hasLoaded { get; protected set; }

        protected static Game1 _host;
        public static Game1 setHost { set { _host = value; } }
        // only an instance has public accessor to host instance
        public Game1 host { get { return _host; } }


        // method for loading assets
          public abstract void Load();

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
