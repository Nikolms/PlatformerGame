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
        Slow,
        Haste,
        Weakness,
        Empower,
        Might,
        BattleRage,
        GhostWalk
    }


    public abstract class StatusEffect
    {
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
            if (effectType == EffectType.Haste) return new Haste() { duration = effectDuration };
            if (effectType == EffectType.Slow) return new Slow() { duration = effectDuration };
            if (effectType == EffectType.Weakness) return new Weakness() { duration = effectDuration };
            if (effectType == EffectType.Empower) return new Empowered() { duration = effectDuration };
            if (effectType == EffectType.Might) return new Might() { duration = effectDuration };
            if (effectType == EffectType.BattleRage) return new BattleRage() { duration = effectDuration };
            if (effectType == EffectType.GhostWalk) return new Ghostly() { duration = effectDuration };

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

        public abstract void DoConstantEffect(CharacterStatuses CS);
        public abstract void DoIntervalEffect(Character C);
    }


    class Regenerate : StatusEffect 
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            // nothing
        }

        public override void DoIntervalEffect(Character C)
        {
            C.OnReceiveHeal(effectStrenght);
        }
    }

    class Poison : StatusEffect
    {
        public Poison()
        {
            TextureSourceFile = "poison_effect";
            animation = new Animation(AnimationLists.GetAnimation("poison_effect"), true);
        }

        public override void DoConstantEffect(CharacterStatuses CS)
        {
            // nothing
        }

        public override void DoIntervalEffect(Character C)
        {
            C.OnReceiveDamage(effectStrenght, true, null);
        }
    }

    class Fragile : StatusEffect
    {

        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.receiveDamageMod += 0.25f;
        }

        public override void DoIntervalEffect(Character C)
        {
            // nothing
        }
    }

    class Slow : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.moveSpeedMod -= 0.2f;
        }

        public override void DoIntervalEffect(Character C)
        {
            // NOTHING
        }
    }

    class Weakness : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.meleeDamageMod -= 0.25f;
        }

        public override void DoIntervalEffect(Character C)
        {
            // NOTHING
        }
    }

    class Haste : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.moveSpeedMod += 0.2f;
        }

        public override void DoIntervalEffect(Character C)
        {
            // NOTHING
        }
    }

    class Shield : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.Shielded = true;
        }

        public override void DoIntervalEffect(Character C)
        {
            // nothing
        }
    }

    class Empowered : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.spellPowerMod += 0.3f;
        }

        public override void DoIntervalEffect(Character C)
        {
            // NOTHING
        }
    }

    class Might : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.meleeDamageMod += 0.2f;
        }

        public override void DoIntervalEffect(Character C)
        {
            // NOTHING
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
            CS.moveSpeedMod += 0.15f;
        }

        public override void DoIntervalEffect(Character C)
        {
            // nothing
        }
    }

    class Ghostly : StatusEffect
    {
        public override void DoConstantEffect(CharacterStatuses CS)
        {
            CS.Shielded = true;
            CS.moveSpeedMod += 0.6f;
        }

        public override void DoIntervalEffect(Character C)
        {
            // NOTHING
        }
    }

}
