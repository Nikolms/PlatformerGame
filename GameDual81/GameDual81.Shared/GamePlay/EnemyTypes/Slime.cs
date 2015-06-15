using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Slime : Enemy, IPlayerInterAct
    {
        public Slime(Vector2 startPos) : base (startPos)
        {
            TextureFileName = "troll_sprite";
            characterType = "troll";
            maxHealth = 10;
            maxSpeedX = 5;
            acceleration = 0.66f;

            detectionRange = 10;
            setParameters();

            actualSize = new Rectangle(0, 0, 70, 50);
        }

        public void CheckPlayerCollision(Player P)
        {
            if (P.BoundingBox.Intersects(BoundingBox))
            P.HitByAttack(10);
        }
    }
}
