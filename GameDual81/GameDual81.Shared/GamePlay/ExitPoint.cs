using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    class ExitPoint : GameObject, ICollisionObject
    {
        public bool levelComplete { get; private set; }

        public void CheckCollisionWithCharacter(Character C)
        {
            if (this.actualSize.Contains(C.BoundingBox)) 
                levelComplete = true;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S, AnimationFiles.TextureLoader T)
        {
            throw new NotImplementedException();
        }
    }
}
