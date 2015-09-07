using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay.Actions
{
    public enum ActionType { MeleeAttack, RangedAttack, Charge, Spell }

    public abstract class BaseAction
    {
        protected Character actor;
        protected Animation animation;
        protected float totalDuration, preExecuteDuration, postExecutionDuration, elapsedTime;
        bool executeComplete;
        protected string textureFileName;
        public string actionAnimationName { get; protected set;}

        public static BaseAction CreateAction(ActionType type, Character actor) 
        {
            BaseAction A = null;
            if (type == ActionType.MeleeAttack) A = new MeleeAttack() 
                { preExecuteDuration = 150, postExecutionDuration = 100, actionAnimationName = "_melee"};

            if (type == ActionType.RangedAttack) A = new RangedAttack() 
                { preExecuteDuration = 250, postExecutionDuration = 100, actionAnimationName = "_ranged"};

            if (type == ActionType.Charge) A = new Charge() 
                { preExecuteDuration = 250, postExecutionDuration = 0, actionAnimationName = "_charge"};

            if (type == ActionType.Spell) A = new WhirlWind() 
                { preExecuteDuration = 750, postExecutionDuration = 100, actionAnimationName = "_spellcast"};

            A.totalDuration = A.preExecuteDuration + A.postExecutionDuration;
            A.actor = actor;
            return A;
        }
            
        public virtual bool UpdateAndCheckIfDone(TimeSpan time) 
        {
            elapsedTime += time.Milliseconds;

            if (animation != null)
            animation.CheckIfDoneAndUpdate(time);

            // do execute only the first time that timer goes past post execute
            if (elapsedTime >= preExecuteDuration && !executeComplete) 
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

        public abstract void DoPreExecution();
        public abstract void DoPostExecution();
        protected abstract void Execute();

        public abstract void Draw(SpriteBatch S, TextureLoader T);
    }

    class RangedAttack : BaseAction
    {
        public override void DoPreExecution()
        {
            // nothing
        }

        public override void DoPostExecution()
        {
            // do nothing
        }

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

    class IceBolt : BaseAction
    {
        public override void DoPreExecution()
        {
            // nothing special
        }

        public override void DoPostExecution()
        {
            // nothing special
        }

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

    class MeleeAttack : BaseAction
    {
        public override void DoPreExecution()
        {
            // nothing
        }

        public override void DoPostExecution()
        {
           // nothing
        }

        protected override void Execute()
        {
            // calculate the hitbox position
            Rectangle hitbox = actor.MeleeReach;
            if (actor.Facing == FacingDirection.Left) 
                hitbox.X = actor.BoundingBox.Center.X - hitbox.Width;

            if (actor.Facing == FacingDirection.Right) 
                hitbox.X = actor.BoundingBox.Center.X;

            hitbox.Y = actor.BoundingBox.Y - (hitbox.Height - actor.BoundingBox.Height);

            MeleeArea AE = new MeleeArea(postExecutionDuration, actor.GetMeleeDamage(), actor, hitbox);
            LevelManager L = new LevelManager();
            L.AddGameObject(AE);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            throw new NotImplementedException();
        }
    }

    class WhirlWind : BaseAction 
    {

        public override void DoPreExecution()
        {
            // nothing
        }

        public override void DoPostExecution()
        {
            // nothing
        }

        protected override void Execute()
        {
            Rectangle hitbox = new Rectangle(0,0,100,200);

            WhirlWindArea W = new WhirlWindArea(10000, actor.GetSpellPower(), actor, hitbox);
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
        public override void DoPreExecution()
        {
            actor.AddExternalSpeed(16 * (int)actor.Facing, 0);
        }

        public override void DoPostExecution()
        {
            // no post execution
        }

        protected override void Execute()
        {
            // TODO
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // TODO
        }
    }

    class Spell : BaseAction
    {
        public override void DoPreExecution()
        {
            throw new NotImplementedException();
        }

        public override void DoPostExecution()
        {
            throw new NotImplementedException();
        }

        protected override void Execute()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            throw new NotImplementedException();
        }
    }
}
