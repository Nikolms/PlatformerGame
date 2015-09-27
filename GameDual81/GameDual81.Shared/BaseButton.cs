using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame
{
    // this baseclass provides the basic functionality so that every button can be aware if it was clicked or not
    // and also gives the draw method contract
    public class BaseButton
    {
        // general parameters
        public Rectangle imageSourceLocation { get; set; }

        protected Rectangle clickArea;
                
        public virtual Rectangle PositionAndSize 
        { 
            // sizebox is scaled to fit screen resolution
            set { clickArea = MyRectangle.AdjustExistingRectangle(value); } 
        }

        // this methid evaluates if a input location matches the buttons location
        public virtual bool CheckIfClicked(Vector2 inputLocation) 
        {
            if (clickArea.Contains(inputLocation))
            return true;

            return false;
        }

        public virtual void Draw(SpriteBatch S) 
        {
        }
    }
}
