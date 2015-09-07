using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.StatusEffects;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Slime : Enemy, IInteractiveObject
    {
        public Slime(Vector2 startPos) : base (startPos)
        {
            TextureFileName = "slime_sprite";
            characterType = "slime";
            MaxHealth = 10;
            maxSpeedX = 1.5f;
            acceleration = 0.66f;

            detectionRange = 0;
            setParameters();

            AI_combatMovement = AI_CombatMovement.Patrol;
            AI_idleMovement = AI_IdleMovement.Patrol;

            actualSize = new Rectangle(0, 0, 70, 40);
        }

        public void CheckPlayerCollision(Player P)
        {
            if (P.BoundingBox.Intersects(BoundingBox))
            P.OnReceiveDamage(10, true, StatusEffect.createEffect(level, EffectType.Poison));
        }
    }
}
