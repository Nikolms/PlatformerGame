using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;
using ThielynGame.GamePlay.StatusEffects;

namespace ThielynGame.GamePlay.Actions
{
    public enum ActionID {
        MeleeAttack,
        RangedAttack,
        Charge,
        IceBolt,
        Regenerate,
        FireCloak,
        BattleRage,
        GhostWalk
    }

    public abstract class BaseAction
    {
        // functionality variables
        protected Character actor;

        protected Animation animation;
        public string actionAnimationName { get; protected set; }
        protected string textureFileName;

        protected float totalDuration, elapsedTime;
        bool executeComplete;
        

        // game mechanic variables
        public int CoolDown { get; protected set; }
        public int PreExecuteDuration { get; protected set; }
        public int PostExecutionDuration { get; protected set; }
        public int EffectDuration { get; protected set; }
        public float DamageModifier { get; protected set; }
        public float EffectStrength { get; protected set; }
        public float SecondaryEffectStrenght { get; protected set; }


        public static BaseAction CreateAction(ActionID type, Character actor) 
        {
            BaseAction A = null;
            if (type == ActionID.RangedAttack) A = new RangedAttack()
                { CoolDown = 0, PreExecuteDuration = (int)actor.AttackSpeed, PostExecutionDuration = 0, actionAnimationName = "_melee" };
              
            if (type == ActionID.MeleeAttack) A = new MeleeAttack() 
                {CoolDown = 0, PreExecuteDuration = (int)actor.AttackSpeed, PostExecutionDuration = 100, actionAnimationName = "_melee"};
            
            if (type == ActionID.Charge) A = new Charge() 
                {CoolDown = 6000, PreExecuteDuration = (int)actor.AttackSpeed, PostExecutionDuration = 400, actionAnimationName = "_charge"};

            if (type == ActionID.IceBolt) A = new IceBolt()
                {CoolDown = 3000, PreExecuteDuration = 1000, PostExecutionDuration = 0, actionAnimationName = "_spellcast" };

            if (type == ActionID.Regenerate) A = new Regenerate()
                {CoolDown = 25000, PreExecuteDuration = 1000, PostExecutionDuration = 0, actionAnimationName = "_spellcast" };

            if (type == ActionID.FireCloak) A = new FireCloak()
                {CoolDown = 5000, PreExecuteDuration = 500, PostExecutionDuration = 0, actionAnimationName = "_spellcast" };

            if (type == ActionID.BattleRage) A = new BattleRage()
                {CoolDown = 15000, PreExecuteDuration = 250, PostExecutionDuration = 0, actionAnimationName = "_charge" };

            if (type == ActionID.GhostWalk) A = new GhostWalk()
                {CoolDown = 20000, PreExecuteDuration = 0, PostExecutionDuration = 0, actionAnimationName = "_charge" };
            
            A.totalDuration = A.PreExecuteDuration + A.PostExecutionDuration;
            A.actor = actor;
            return A;
        }
            
        public virtual bool UpdateAndCheckIfDone(TimeSpan time) 
        {
            elapsedTime += time.Milliseconds;

            if (animation != null)
            animation.CheckIfDoneAndUpdate(time);

            // do execute only the first time that timer goes past post execute
            if (elapsedTime >= PreExecuteDuration && !executeComplete) 
            {
                executeComplete = true;
                Execute();
            }

            if (executeComplete) DoPostExecution();
            else DoPreExecution();

            if (elapsedTime >= totalDuration) 
            {
                return true;
            }

            return false;
        }

        public virtual void DoPreExecution() { }
        public virtual void DoPostExecution() { }
        protected abstract void Execute();

        public abstract void Draw(SpriteBatch S, TextureLoader T);
    }

    class IceBolt : BaseAction
    {
        protected override void Execute()
        {
            Projectile P = Projectile.createProjectile(ProjetileType.IceBolt, actor);
            LevelManager L = new LevelManager();
            L.AddGameObject(P);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            //nothing
        }
    }

    class RangedAttack : BaseAction
    {
        protected override void Execute()
        {
            Projectile P = Projectile.createProjectile(ProjetileType.Arrow, actor);
            LevelManager L = new LevelManager();
            L.AddGameObject(P);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // nothing
        }
    }

    class MeleeAttack : BaseAction
    {
        protected override void Execute()
        {
            // calculate the hitbox position
            Rectangle hitbox = actor.MeleeReach;
            if (actor.Facing == FacingDirection.Left) 
                hitbox.X = actor.BoundingBox.Center.X - hitbox.Width;

            if (actor.Facing == FacingDirection.Right) 
                hitbox.X = actor.BoundingBox.Center.X;

            hitbox.Y = actor.BoundingBox.Y - (hitbox.Height - actor.BoundingBox.Height);

            MeleeArea AE = new MeleeArea(PostExecutionDuration, actor.GetMeleeDamage(), actor, hitbox);
            LevelManager L = new LevelManager();
            L.AddGameObject(AE);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // nothing
        }
    }

    class WhirlWind : BaseAction 
    {
        protected override void Execute()
        {
            Rectangle hitbox = new Rectangle(0,0,100,200);

            IntervalDamageArea W = new IntervalDamageArea(1000,10000, actor.GetSpellPower(), actor, hitbox);
            LevelManager L = new LevelManager();
            L.AddGameObject(W);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            throw new NotImplementedException();
        }
    }

    class Charge : BaseAction
    {
        public override void DoPostExecution()
        {
            actor.AddExternalSpeed(18 * (int)actor.Facing, 0);
        }

        protected override void Execute()
        {
            float damage = actor.GetMeleeDamage() * 2;
            AreaEffect Area = new AreaEffect(PostExecutionDuration, (int)damage, actor, actor.BoundingBox);
            Area.FollowsActor = true;
            LevelManager L = new LevelManager();
            L.AddGameObject(Area);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // TODO
        }
    }

    class Regenerate : BaseAction
    {
        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // nothing
        }

        protected override void Execute()
        {
            actor.OnReceiveEffect(StatusEffect.createEffect
                (actor.level, EffectType.Regenerate, 20000));
        }
    }

    class FireCloak : BaseAction
    {
        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // nothing
        }

        protected override void Execute()
        {
            Rectangle hitbox = actor.BoundingBox;
            hitbox.Width += 100; hitbox.Height += 20;

            IntervalDamageArea W = new IntervalDamageArea(1000, 8000, actor.GetSpellPower(), actor, hitbox);

            Animation A = new Animation(AnimationFiles.AnimationLists.GetAnimation("fire_cloak_effect"), true);
            W.SetAnimation(A, "fire_shield");

            LevelManager L = new LevelManager();
            L.AddGameObject(W);
        }
    }

    class BattleRage : BaseAction
    {
        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // nothing
        }

        protected override void Execute()
        {
            actor.OnReceiveEffect(StatusEffect.createEffect(actor.level, StatusEffects.EffectType.BattleRage, 7000));
        }
    }

    class GhostWalk : BaseAction
    {
        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // nothing
        }

        protected override void Execute()
        {
            actor.ClearStatuses();
            actor.OnReceiveEffect(StatusEffect.createEffect(actor.level, EffectType.GhostWalk, 3000));
        }
    }

}
