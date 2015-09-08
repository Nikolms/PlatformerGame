using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Archer : Enemy
    {
        public Archer(Vector2 startPos) : base (startPos)
        {
            TextureFileName = "troll_sprite";
            characterType = "archer";

            MaxHealth = 10;
            maxSpeedX = 2;
            acceleration = 0.66f;

            detectionRange = 600;
            rangePrimaryAttack = 600;
            rangeSecondaryAttack = 0;

            cooldownBetweenAttacks = 2000;
            cooldownPrimaryAttack = 2000;
            cooldownSecondaryAttack = 10000;

            actualSize = new Rectangle(0,0,45,70);

            AI_combatMovement = AI_CombatMovement.Guard;
            AI_idleMovement = AI_IdleMovement.Patrol;

            setParameters();
        }

        protected override void DoPrimaryAttack()
        {
            
        }

        public override int GetRangedDamage()
        {
            return 20;
        }
    }
}
