using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Archer : Enemy
    {
        public Archer(Vector2 startPos, int level) : base (startPos, level)
        {
            // TextureFileName = "troll_sprite";
            characterType = "archer";

            MaxHealth = 30 + level;
            maxSpeedX = 2;
            acceleration = 0.66f;

            detectionRange = 600;
            releaseDetectRange = 650;
            rangePrimaryAttack = 650;
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
            startNewAction(BaseAction.CreateAction(ActionID.RangedAttack, this));
            base.DoPrimaryAttack();
        }

        public override int GetRangedDamage()
        {
            return 19 + level;
        }
    }
}
