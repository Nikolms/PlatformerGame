using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Beast : Enemy
    {

        public Beast(Vector2 startPosition, int level) : base(startPosition, level)
        {
            TextureFileName = "naga_sprite";
            characterType = "beast";
            actualSize = new Rectangle(0,0,70,70);

            MaxHealth = 30 + level;
            acceleration = 0.66f;
            maxSpeedX = 3f;

            baseMeleeDamage = 10 + (level / 2);

            rangePrimaryAttack = 300;
            rangeSecondaryAttack = 0;
            detectionRange = 300;

            cooldownBetweenAttacks = 3000;
            cooldownPrimaryAttack = 3000;
            cooldownSecondaryAttack = 3000;

            setParameters();
        }

        protected override void DoPrimaryAttack()
        {
            startNewAction(BaseAction.CreateAction(ActionID.Charge, this));
            base.DoPrimaryAttack();
        }
    }
}
