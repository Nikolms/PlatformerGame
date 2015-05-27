using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    class ExitPoint : GameObject, ICollisionObject
    {
        public bool LevelCompleted { get; private set; }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S, AnimationFiles.TextureLoader T)
        {
            throw new NotImplementedException();
        }

        public void CheckCollisionWithCharacter(Character C)
        {
            if (actualSize.Contains(C.BoundingBox)) 
                LevelCompleted = true;
        }
    }
}
