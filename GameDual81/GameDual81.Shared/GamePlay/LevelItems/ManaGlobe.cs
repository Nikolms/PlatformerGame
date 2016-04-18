using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;

namespace ThielynGame.GamePlay
{
    class ManaGlobe : MovableObject, IInteractiveObject
    {
        int manaValue = 20;

        public ManaGlobe()
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
                new Rectangle(200,1,40,40),
                Color.White);
        }
    }
}
