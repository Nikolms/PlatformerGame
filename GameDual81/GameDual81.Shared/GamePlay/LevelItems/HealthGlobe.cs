using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;

namespace ThielynGame.GamePlay
{
    class HealthGlobe : MovableObject, IInteractiveObject
    {
        int healValue = 20;

        public HealthGlobe()
        {
            actualSize = new Rectangle(0,0,40,40);
        }

        public void CheckPlayerCollision(Player P)
        {
            if (BoundingBox.Intersects(P.BoundingBox))
            IsDead = true;
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            S.Draw(T.GetTexture("world_item_texture"), 
                MyRectangle.AdjustExistingRectangle(BoundingBox), 
                new Rectangle(157,1,40,40),
                Color.White);
        }
    }
}
