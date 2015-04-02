using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame
{
    // helper class to adjust sizes and positions to screen resolution
    class MyRectangle
    {
        // transforms dimension using the screenmultiplier and created a new rectangle
        public static Rectangle AdjustSizeCustomRectangle(int x, int y, int width, int height) 
        {
            float modX = (float)x * Game1.screenMultiplierWidth;
            float modY = (float)y * Game1.screenMultiplierHeight;
            float modW = (float)width * Game1.screenMultiplierWidth;
            float modH = (float)height * Game1.screenMultiplierHeight;

            return new Rectangle(
                (int)modX, 
                (int)modY, 
                (int)modW, 
                (int)modH);
        }

        public static Rectangle AdjustExistingRectangle(Rectangle R) 
        {
            float modX = (float)R.X * Game1.screenMultiplierWidth;
            float modY = (float)R.Y * Game1.screenMultiplierHeight;
            float modW = (float)R.Width * Game1.screenMultiplierWidth;
            float modH = (float)R.Height * Game1.screenMultiplierHeight;

            return new Rectangle(
                    (int) modX,
                    (int) modY,
                    (int) modW,
                    (int) modH
                    );
        }
    }
}
