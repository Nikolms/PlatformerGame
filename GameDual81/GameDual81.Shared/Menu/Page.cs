using ThielynGame.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ThielynGame.Menu
{
    abstract class Page
    {
        public static MenuScreen containingScreen { get; set; }
        protected List<BaseButton> buttons = new List<BaseButton>();

        // iterate all buttons on the page for every input available
        public virtual void checkButtonClick(List<Vector2> inputLocations) 
        {
            foreach (Vector2 input in inputLocations)
            {
                foreach (BaseButton B in buttons)
                {
                    B.CheckIfClicked(input);
                }
            }
        }

        public virtual void Draw(SpriteBatch S) 
        {
            //draw all the buttons that belong to the screen
            foreach (BaseButton B in buttons) 
            {
                B.Draw(S);
            }
        }
    }
}
