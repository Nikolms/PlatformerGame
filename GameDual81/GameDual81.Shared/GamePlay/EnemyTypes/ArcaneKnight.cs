using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class ArcaneKnight : Enemy
    {
        public ArcaneKnight(Vector2 startPosition, int level) : base(startPosition, level)
        {
            characterType = "arcaneknight";

            actualSize = new Rectangle(0,0,45,70);
            MeleeReach = new Rectangle(0,0,70,60);
            rangePrimaryAttack = 60;
            rangeSecondaryAttack = 500;
            AttackSpeed = 300;

            baseMeleeDamage = 10 + level;
            baseSpellPower = 20 + level;

            cooldownPrimaryAttack = 1500;
            cooldownSecondaryAttack = 10000;
            cooldownBetweenAttacks = 1000;

            MaxHealth = 20 + (2 * level);
            maxSpeedX = 3.5f;
            acceleration = 0.66f;

            detectionRange = 300;
            releaseDetectRange = 600;

            AI_combatMovement = AI_CombatMovement.MoveCloser;
            AI_idleMovement = AI_IdleMovement.Patrol;

            setParameters();
        }

        protected override void DoPrimaryAttack()
        {
            startNewAction(BaseAction.CreateAction(ActionID.MeleeAttack, this));
            base.DoPrimaryAttack();
        }

        protected override void DoSecondaryAttack()
        {
            startNewAction(BaseAction.CreateAction(ActionID.ArcaneCloak, this));
            base.DoSecondaryAttack();
        }
    }
}
