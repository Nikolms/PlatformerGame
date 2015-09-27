using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Troll : Enemy
    {
        public Troll(Vector2 startLocation, int level) : base(startLocation, level) 
        {
            TextureFileName = "troll_sprite";
            characterType = "troll";
            actualSize = new Rectangle(0,0,80,120);
            maxSpeedX = 2f;
            acceleration = 0.66f;

            MaxHealth = 2;
            detectionRange = 300;
            rangePrimaryAttack = 60;
            rangeSecondaryAttack = 60;

            MeleeReach = new Rectangle(0,0,100,100);
            AttackSpeed = 1000;

            cooldownPrimaryAttack = 4000;
            cooldownSecondaryAttack = 2000;
            cooldownBetweenAttacks = 1500;

            AI_combatMovement = AI_CombatMovement.MoveCloser;
            AI_idleMovement = AI_IdleMovement.Patrol;


            // finalize the calculated  parameters
            setParameters();
        }

        protected override void DoPrimaryAttack()
        {
            startNewAction(BaseAction.CreateAction(ActionID.MeleeAttack, this));
        }

        protected override void DoSecondaryAttack()
        {
            
        }
    }
}
