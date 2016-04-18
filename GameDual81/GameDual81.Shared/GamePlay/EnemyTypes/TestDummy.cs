using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    // a dummy enemy to test level up scaling and skill interactions, NOT TO BE USED IN FINAL GAME
    class TestDummy : Enemy
    {

        public TestDummy(int level) : base(level)
        {
            actualSize = new Rectangle(0, 0, 80, 80);

            MaxHealth = 10 + level;
            TextureFileName = "TODO";
            characterType = "dummy";
            maxSpeedX = 0;

            setParameters();
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {            
            base.Draw(S, T);
        }
    }
}
