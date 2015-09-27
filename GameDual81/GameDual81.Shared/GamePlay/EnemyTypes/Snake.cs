﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Snake : Enemy
    {
        public Snake(Vector2 startPos, int level) : base (startPos, level) 
        {
            TextureFileName = "naga_sprite";
            characterType = "snake";

            MaxHealth = 20 + (2* level);
            acceleration = 0.66f;
            maxSpeedX = 4;

            AttackSpeed = 500;

            MeleeReach = new Rectangle(0,0,100,90);
            rangePrimaryAttack = 60;
            rangeSecondaryAttack = 0;
            detectionRange = 250;

            cooldownBetweenAttacks = 1750;
            cooldownPrimaryAttack = 1750;
            cooldownSecondaryAttack = 500;

            actualSize = new Rectangle(0,0,80,90);

            setParameters();
        }

        protected override void DoPrimaryAttack()
        {
            this.startNewAction(BaseAction.CreateAction(ActionID.MeleeAttack, this));
        }

        public override int GetMeleeDamage()
        {
            return 50;
        }
    }
}
