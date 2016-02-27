using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;

namespace ThielynGame.GamePlay
{
    class ResourceNode : GameObject, IDestroyableObject
    {

        int currentHealth;

        public ResourceNode(Vector2 Position)
        {
            alignment = ObjectAlignment.Enemy;
            position = Position;
            this.actualSize = new Rectangle(0,0,60,60);
            TextureFileName = "resourcenode";
            currentHealth = 5;
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            S.Draw(T.GetTexture(TextureFileName), MyRectangle.AdjustExistingRectangle(BoundingBox), Color.White);
        }

        public void HitByHarmfulObject(IHarmfulObject O)
        {
            currentHealth -= 1;

            if (currentHealth <= 0)
                IsDead = true;
        }

        public Rectangle GetBoundingBox()
        {
            return BoundingBox;
        }

        public ObjectAlignment GetAlignment()
        {
            return alignment;
        }
    }
}
