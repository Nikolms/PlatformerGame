using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Troll : Enemy
    {
        public Troll(Vector2 startLocation) : base(startLocation) 
        {
            TextureFileName = "troll_sprite";
            characterType = "troll";
            actualSize = new Rectangle(0,0,80,120);
            maxSpeed = 1.5f;
            acceleration = 0.24f;

            maxHealth = 2;
            detectionRange = 300;

            // finalize the calculated  parameters
            setParameters();
        }
    }
}
