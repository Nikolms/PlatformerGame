using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    class Chest : MovableObject, IDestroyableObject
    {
        int currentHealth = 3;

        public Chest()
        {
            actualSize = new Rectangle(0,0,100,100);
            teamID = TeamID.Enemy;
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            S.Draw(T.GetTexture("world_item_texture"), MyRectangle.AdjustExistingRectangle(BoundingBox),
                new Rectangle(1,1,100,100), Color.White);
        }

        public TeamID GetTeamID()
        {
            return this.teamID;
        }

        public Rectangle GetBoundingBox()
        {
            return BoundingBox;
        }

        public void HitByHarmfulObject(IHarmfulObject O)
        {
            currentHealth -= 1;
            if (currentHealth <= 0) IsDead = true;
        }
    }
}
