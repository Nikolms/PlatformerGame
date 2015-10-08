using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.StatusEffects;

namespace ThielynGame.GamePlay.EnemyTypes
{
    class Slime : Enemy, IInteractiveObject
    {
        public Slime(Vector2 startPos, int level) : base (startPos, level)
        {
            TextureFileName = "slime_sprite";
            characterType = "slime";
            MaxHealth = 15 + (5 * level);
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
            {
                AttackDetailObject attack = new AttackDetailObject();
                attack.BuffEffects = new List<StatusEffect>();
                attack.BuffEffects.Add(
                    StatusEffect.createEffect(level, EffectType.Poison, 5000));
                attack.damage = 10 + (level / 4);
            }
        }
    }
}
