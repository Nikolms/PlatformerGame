using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThielynGame.Screens
{
    class LoadingScreen : Screen
    {
        Texture2D background;

        // store the instance of the screen that needs to unload
        // and the screen that needs to prepare for display
        Screen previousScreen, nextScreen;

        public LoadingScreen(Texture2D background, Screen screenToLoad, Screen previous) 
        {
            this.background = background;
            nextScreen = screenToLoad;
            previousScreen = previous;
            // make sure any new instance knows it needs to load
            hasLoaded = false;
        }

        public override void Load()
        {
            Task t = new Task(LoadingProcess);
            t.Start();
        }
        // this method wraps the actual loading and unloading steps
        void LoadingProcess() 
        {
            // unload old screen first
            previousScreen.ExitScreen();
            // then load the new screen
            nextScreen.Load();
            // set the loading flag true when done
            hasLoaded = true;
        }

        public override void Update(TimeSpan time)
        {
            // no update to be done in this screen
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(background, MyRectangle.AdjustSizeCustomRectangle(0, 0, 1280, 768), Color.White);

            // once loading has completed we tell game1 to switch screen
            // we do this call in draw in order to draw a last frame of loading screen before
            // switching
            if (hasLoaded) 
            {
                host.FinishScreenTransition(nextScreen);
            }
        }

        public override void ExitScreen()
        {
            throw new NotImplementedException();
        }

        public override void HandleInput(InputHandler inputhandler)
        {
            // no input in this screen
        }
    }
}
