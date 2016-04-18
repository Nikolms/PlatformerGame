using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;

namespace ThielynGame.GamePlay
{
    class Coin : MovableObject, IInteractiveObject
    {
        int Value = 1;

        public Coin()
        {
            actualSize = new Rectangle(0,0,50,50);
        }

        public void CheckPlayerCollision(Player P)
        {
            if (BoundingBox.Intersects(P.BoundingBox))
                IsDead = true;
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            S.Draw(T.GetTexture("world_item_texture"), MyRectangle.AdjustExistingRectangle(BoundingBox),
                new Rectangle(104,1,50,50), Color.White);
        }
    }
}
