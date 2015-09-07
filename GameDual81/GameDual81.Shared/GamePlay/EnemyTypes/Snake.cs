using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Snake : Enemy
    {
        public Snake(Vector2 startPos) : base (startPos) 
        {
            TextureFileName = "naga_sprite";
            characterType = "snake";

            MaxHealth = 10;
            acceleration = 0.66f;
            maxSpeedX = 4;

            rangePrimaryAttack = 60;
            rangeSecondaryAttack = 0;
            detectionRange = 250;

            cooldownBetweenAttacks = 1750;
            cooldownPrimaryAttack = 1750;
            cooldownSecondaryAttack = 10000;

            actualSize = new Rectangle(0,0,80,90);

            setParameters();
        }

        protected override void DoPrimaryAttack()
        {
        }

        public override int GetMeleeDamage()
        {
            return 50;
        }
    }
}
