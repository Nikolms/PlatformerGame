using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.Menu
{
    public delegate void MenuActionEvent(MenuButton sender);

    public class MenuButton : BaseButton
    {
        public string Text { get; set; }
        public string addOnText { get; set; }

        Vector2 textPosition;
        public event MenuActionEvent onClick;

        public override Rectangle PositionAndSize
        {
            set
            {
                base.PositionAndSize = value;
                // override this property set also set the displaytext position relative
                // to the button position
                textPosition.X = clickArea.X;
                textPosition.Y = clickArea.Y;
            }
        }


        public MenuButton(string displaytext) 
        {
            Text = displaytext;
        }

        public override bool CheckIfClicked(Vector2 inputLocation)
        {
            if (base.CheckIfClicked(inputLocation))
                onClick(this);

            return true;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S)
        {
            string actualText = Text + addOnText;
            // draw the background
            S.Draw(CommonAssets.menuButtonBackground, clickArea, Color.White);
            // draw the string on top
            S.DrawString(CommonAssets.menuFont, actualText, textPosition, Color.Brown);
        }

        // use this method to adjust text position within the button area
        public void SetTextPadding(int x, int y) 
        {
            textPosition.X += x * Game1.screenMultiplierWidth;
            textPosition.Y += y * Game1.screenMultiplierHeight;
        }
    }
}
