using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    class ExitPoint : GameObject, IPlayerInterAct
    {

        public ExitPoint(Rectangle SizeAndPosition) 
        {
            actualSize = SizeAndPosition;
            position.X = SizeAndPosition.X;
            position.Y = SizeAndPosition.Y;
        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S, AnimationFiles.TextureLoader T)
        {
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(BoundingBox), Color.White);
        }


        public void CheckPlayerCollision(Player P)
        {
            if (BoundingBox.Contains(P.BoundingBox)) 
                P.ReachedEndOfLevel = true;
        }
    }
}
