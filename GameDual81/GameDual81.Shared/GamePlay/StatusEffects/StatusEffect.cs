using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay.StatusEffects
{
    public enum EffectType {
        Poison,
        Fragile,
        Shield,
        Regenerate,
        Weakness,
        BattleRage,
        GhostWalk,
        Stunned,
        LifeSteal
    }


    public abstract class StatusEffect
    {
        public int triggerPropability = 100;  // 100% chance by default
        public float duration { get; protected set; }
        protected float aliveTime = 0, effectInterval, lastTick;
        protected int effectStrenght;

        // effects might have animations
        protected Animation animation;
        protected Rectangle animationPosition;
        protected string TextureSourceFile;

        public static StatusEffect createEffect (int level, EffectType effectType, int effectDuration)
        {
            if (effectType == EffectType.Fragile) return new Fragile() { duration = effectDuration };
            if (effectType == EffectType.Poison) return new Poison() { duration = effectDuration, effectStrenght = 5 , effectInterval = 1000 };
            if (effectType == EffectType.Regenerate) return new Regenerate { duration = effectDuration, effectStrenght = 2, effectInterval = 1000 };
            if (effectType == EffectType.Shield) return new Shield() { duration = effectDuration };
            if (effectType == EffectType.Weakness) return new Weakness() { duration = effectDuration };
            if (effectType == EffectType.BattleRage) return new BattleRage() { duration = effectDuration };
            if (effectType == EffectType.GhostWalk) return new Ghostly() { duration = effectDuration };
            if (effectType == EffectType.LifeSteal) return new LifeSteal() { duration = effectDuration };
           
            return null;
        }

        public virtual bool CheckIfTimeLeft(TimeSpan time, Character C, CharacterStatuses CS) 
        {
            aliveTime += time.Milliseconds;
            lastTick += time.Milliseconds;

            if (animation != null)
            {
                animation.CheckIfDoneAndUpdate(time);
                animationPosition = C.BoundingBox;
            }

            if (lastTick >= effectInterval) 
            {
                DoIntervalEffect(C);
                lastTick = 0;
            }

            DoConstantEffect(CS);

            if (aliveTime >= duration) return false;

            return true;
        }

        public virtual void Draw(SpriteBatch S, TextureLoader T)
        {
            if (animation != null)
            S.Draw
                (T.GetTexture(TextureSourceFile),
                MyRectangle.AdjustExistingRectangle(animationPosition),
                animation.AnimationFrameToDraw,
                Color.White
                );
        }


        // function for overriding an existing statuseffect with a new one
        // technically we only reset the timer and duration
        public void ReplaceWithNewInstance(float newDuration)
        {
            aliveTime = 0;
            duration = newDuration;
        }

        public virtual void DoConstantEffect(CharacterStatuses CS) { }
        public virtual void DoIntervalEffect(Character C) { }
    }
    

    class Regenerate : StatusEffect 
    {
        public override void DoIntervalEffect(Character C)
        {
            AttackDetailObject heal = new AttackDetailObject()
            { healing = 3 };
            C.OnReceiveAttackOrEffect(heal);
        }
    }

    class Poison : StatusEffect
    {
        public Poison()
        {
            TextureSourceFile = "poison_effect";
            animation = new Animation(AnimationLists.GetAnimation("poison_effect"), true);
        }

        public override void DoIntervalEffect(Character C)
        {
            AttackDetailObject attack = new AttackDetailObject()
            { damage = effectStrenght, ignoresArmor = true };
            C.OnReceiveAttackOrEffect(attack);
        }
    }

    class Fragile : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.receiveDamageMod += 0.25f;
        }
    }

    class Weakness : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.meleeDamageMod -= 0.25f;
        }
    }
    
    class Shield : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.Shielded = true;
        }
    }
    
    class BattleRage : StatusEffect
    {
        public BattleRage()
        {
            TextureSourceFile = "fury_effect";
            animation = new Animation(AnimationLists.GetAnimation("furyEffect"), true);
        }

        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.meleeDamageMod += 0.2f;
            CS.moveSpeedMod += 0.2f;
        }
    }

    class Ghostly : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.CannotAttack = true;
            CS.Shielded = true;
            CS.moveSpeedMod += 0.6f;
            CS.gravityMod = 11;
        }
    }

    class LifeSteal : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.StealsLife = true;
        }
    }

}
