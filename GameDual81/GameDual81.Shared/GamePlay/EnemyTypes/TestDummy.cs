using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class TestDummy : Enemy
    {

        public TestDummy(Vector2 startPosition, int level) : base(startPosition, level)
        {
            actualSize = new Rectangle(0, 0, 80, 80);

            MaxHealth = 10;
            TextureFileName = "TODO";
            characterType = "dummy";
            maxSpeedX = 2;

            setParameters();
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {            
            base.Draw(S, T);
        }
    }
}
