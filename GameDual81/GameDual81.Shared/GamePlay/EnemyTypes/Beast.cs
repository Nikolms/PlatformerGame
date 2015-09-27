using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Beast : Enemy
    {

        public Beast(Vector2 startPosition, int level) : base(startPosition, level)
        {
            TextureFileName = "naga_sprite";
            characterType = "snake";
            actualSize = new Rectangle(0,0,70,70);

            MaxHealth = 30 + level;

        }
    }
}
