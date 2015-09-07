using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay.StatusEffects
{
    public enum EffectType { Poison, Fragile, Shield, Regenerate}

    public abstract class StatusEffect
    {
        protected float duration, aliveTime = 0, effectInterval, lastTick;
        protected int effectStrenght;

        public static StatusEffect createEffect (int level, EffectType effectType)
        {
            if (effectType == EffectType.Fragile) return new Fragile() { duration = 10000 };
            if (effectType == EffectType.Poison) return new Poison() { duration = 5000, effectStrenght = 5 , effectInterval = 1000 };
            if (effectType == EffectType.Regenerate) return new Regenerate {duration = 60000, effectStrenght = 3, effectInterval = 2000 };
            if (effectType == EffectType.Shield) return new Shield() { duration = 6000 };

            return null;
        }

        public virtual bool CheckIfTimeLeft(TimeSpan time, Character C, CharacterStatuses CS) 
        {
            aliveTime += time.Milliseconds;
            lastTick += time.Milliseconds;

            if (lastTick >= effectInterval) 
            {
                DoIntervalEffect(C);
                lastTick = 0;
            }

            DoConstantEffect(CS);

            if (aliveTime >= duration) return false;

            return true;
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
            CS.Fragile = 1.25f;
        }

        public override void DoIntervalEffect(Character C)
        {
            // nothing
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

}
