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
        ArcaneStorm,
        ArcaneSpray,
        ArcaneBolt,
        ArcaneLightning,
        BattleRage,        
        ChainSpear,
        Charge,
        ArcaneCloak,
        Heal,
        ShadowForm,        
        MeleeAttack,
        RangedAttack,
        Regenerate,
        LifeSteal
    }

    public enum MonsterSkills {

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

        public bool CanBeUsedInAir { get; protected set; }
        

        public static BaseAction CreateAction(ActionID type, Character actor) 
        {
            BaseAction A = null;
            if (type == ActionID.ArcaneStorm) A = new ArcaneStorm()
                { CoolDown = 3000, PreExecuteDuration = 0, PostExecutionDuration = 2000, actionAnimationName = "_spellcast"};

            if (type == ActionID.ArcaneBolt) A = new ArcaneBolt()
                { CoolDown = 3000, PreExecuteDuration = 750, PostExecutionDuration = 0, actionAnimationName = "_spellcast" };

            if (type == ActionID.ArcaneCloak) A = new ArcaneCloak()
                { CoolDown = 3000, PreExecuteDuration = 500, PostExecutionDuration = 0, actionAnimationName = "_spellcast" };

            if (type == ActionID.ArcaneLightning) A = new ArcaneLightning()
            { CoolDown = 3000, PreExecuteDuration = 500, PostExecutionDuration = 0, actionAnimationName = "_spellcast" };

            if (type == ActionID.BattleRage) A = new BattleRage()
                { CoolDown = 3000, PreExecuteDuration = 250, PostExecutionDuration = 0, actionAnimationName = "_charge" };
            
            if (type == ActionID.ChainSpear) A = new ChainSpear()
                { CoolDown = 3000, PreExecuteDuration = 0, PostExecutionDuration = 10000, actionAnimationName = "_melee" };

            if (type == ActionID.Charge) A = new Charge()
                { CoolDown = 3000, PreExecuteDuration = (int)actor.AttackSpeed, PostExecutionDuration = 400, actionAnimationName = "_charge" };
            
            if (type == ActionID.Heal) A = new Heal()
                { CoolDown = 3000, PreExecuteDuration = 2000, PostExecutionDuration = 0, actionAnimationName = "_spellcast"};

            if (type == ActionID.LifeSteal) A = new LifeSteal()
                { CoolDown = 3000, PreExecuteDuration = 0, PostExecutionDuration = 0, actionAnimationName = "_spellcast"};

            if (type == ActionID.MeleeAttack) A = new MeleeAttack()
                { CoolDown = 0, PreExecuteDuration = (int)actor.AttackSpeed, PostExecutionDuration = 100, actionAnimationName = "_melee" };
            
            if (type == ActionID.RangedAttack) A = new RangedAttack()
                { CoolDown = 0, PreExecuteDuration = (int)actor.AttackSpeed, PostExecutionDuration = 100, actionAnimationName = "_ranged" };
              
            if (type == ActionID.Regenerate) A = new Regenerate()
                {CoolDown = 3000, PreExecuteDuration = 1000, PostExecutionDuration = 0, actionAnimationName = "_spellcast" };
            
            if (type == ActionID.ShadowForm) A = new ShadowForm()
                {CoolDown = 3000, PreExecuteDuration = 0, PostExecutionDuration = 0, actionAnimationName = "_charge" };
            
            A.totalDuration = A.PreExecuteDuration + A.PostExecutionDuration;
            A.actor = actor;
            return A;
        }
            
        public virtual bool UpdateAndCheckIfDone(TimeSpan time) 
        {
            elapsedTime += time.Milliseconds;

            // recalculate total duration every frame. Important in case action
            // is not fixed duration
            totalDuration = PreExecuteDuration + PostExecutionDuration;

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

        // override this for actions that have other visual effect in addition to character animation
        public virtual void Draw(SpriteBatch S, TextureLoader T)
        {
        }
    }


    class ArcaneLightning : BaseAction
    {
        protected override void Execute()
        {
            Rectangle damageArea = new Rectangle
                (0 ,actor.BoundingBox.Y, 700,actor.BoundingBox.Height);

            if (actor.Facing == FacingDirection.Left)
                damageArea.X = actor.BoundingBox.Left - 695;
            if (actor.Facing == FacingDirection.Right)
                damageArea.X = actor.BoundingBox.Right;

            AttackDetailObject attack = new AttackDetailObject()
            { damage = actor.GetSpellPower() };

            AreaEffect Area = new AreaEffect(500, actor, damageArea, attack);

            Animation Anim = new Animation(AnimationFiles.AnimationLists.GetAnimation("fire_cloak_effect"), true);
            Area.SetAnimation(Anim, "fire_shield");

            LevelManager L = new LevelManager();
            L.AddGameObject(Area);
        }
    }

    class ArcaneStorm : BaseAction
    {
        protected override void Execute()
        {
            Rectangle damageArea = new Rectangle(0,0,1280, 768);
            AttackDetailObject attack = new AttackDetailObject()
            { damage = actor.GetSpellPower()  };

            AreaEffect Area = 
                new AreaEffect(2000, actor, damageArea, attack);

            Animation Anim = new Animation(AnimationFiles.AnimationLists.GetAnimation("fire_cloak_effect"), true);
            Area.SetAnimation(Anim, "fire_shield");

            LevelManager L = new LevelManager();
            L.AddGameObject(Area);
        }
    }

    class Heal : BaseAction
    {
        protected override void Execute()
        {
            AttackDetailObject Heal = new AttackDetailObject()
            { healing = 50 };
            actor.OnReceiveAttackOrEffect(Heal);
            actor.ClearStatuses();
        }
    }

    class ArcaneBolt : BaseAction
    {
        protected override void Execute()
        {
            Projectile P = Projectile.createProjectile(ProjetileType.IceBolt, actor);
            LevelManager L = new LevelManager();
            L.AddGameObject(P);
        }
    }

    class ChainSpear : BaseAction
    {
        Projectile P;

        protected override void Execute()
        {
            P = Projectile.createProjectile(ProjetileType.ChainSpear, actor);
            LevelManager M = new LevelManager();
            M.AddGameObject(P);
        }

        public override void DoPostExecution()
        {
            // when we react our projectile hit something, we set the duration to a very
            // low value, which causes the next loop to react that duration is over
            if (P.collidedWithSomething) PostExecutionDuration = 1;
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

            AttackDetailObject attack = new AttackDetailObject()
            { damage = actor.GetMeleeDamage()};

            MeleeArea Area = new MeleeArea(PostExecutionDuration, actor, hitbox, attack);
            LevelManager L = new LevelManager();
            L.AddGameObject(Area);
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
            AttackDetailObject attack = new AttackDetailObject()
            { damage = actor.GetMeleeDamage() * 2};

            AreaEffect Area = new AreaEffect(PostExecutionDuration, actor, actor.BoundingBox, attack);
            Area.FollowsActor = true;
            LevelManager L = new LevelManager();
            L.AddGameObject(Area);
        }
    }

    class Regenerate : BaseAction
    {
        protected override void Execute()
        {
            AttackDetailObject buff = new AttackDetailObject();
            buff.BuffEffects = new List<StatusEffect>();
            buff.BuffEffects.Add(
                StatusEffect.createEffect(actor.level, EffectType.Regenerate, 20000));

            actor.OnReceiveAttackOrEffect(buff);
        }
    }

    class ArcaneCloak : BaseAction
    {
        protected override void Execute()
        {
            Rectangle hitbox = actor.BoundingBox;
            hitbox.Width += 100; hitbox.Height += 20;

            AttackDetailObject attack = new AttackDetailObject()
            { damage = actor.GetSpellPower() / 2 };

            AreaEffect Area = new AreaEffect(8000, actor, hitbox, attack);
            Area.centerAroundActor = true;
            Area.FollowsActor = true;
            Area.SetInterval(1000);

            Animation A = new Animation(AnimationFiles.AnimationLists.GetAnimation("fire_cloak_effect"), true);
            Area.SetAnimation(A, "fire_shield");

            LevelManager L = new LevelManager();
            L.AddGameObject(Area);
        }
    }

    class BattleRage : BaseAction
    {
        protected override void Execute()
        {
            AttackDetailObject buff = new AttackDetailObject();
            buff.BuffEffects = new List<StatusEffect>();
            buff.BuffEffects.Add(
                StatusEffect.createEffect(actor.level, StatusEffects.EffectType.BattleRage, 7000));

            actor.OnReceiveAttackOrEffect(buff);
        }
    }

    class ShadowForm : BaseAction
    {
        protected override void Execute()
        {
            AttackDetailObject buff = new AttackDetailObject();
            buff.BuffEffects = new List<StatusEffect>();
            buff.BuffEffects.Add(
                StatusEffect.createEffect(actor.level, StatusEffects.EffectType.GhostWalk, 2000));

            actor.OnReceiveAttackOrEffect(buff);
        }
    }

    class LifeSteal : BaseAction
    {
        protected override void Execute()
        {
            AttackDetailObject effect = new AttackDetailObject();
            effect.BuffEffects.Add(
                StatusEffect.createEffect(actor.level, EffectType.LifeSteal, 7000)
                );
            actor.OnReceiveAttackOrEffect(effect);
        }
    }

}
