using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    class FloatingMine : Enemy
    {
        float changeMovementTimer, changeMovementInterval = 2500;
        Vector2 PatrolPoint;        // the point around which random movement happens
        int patrolRange = 200;            // the radius how far random movement can happen

        public FloatingMine(Vector2 StartPosition, int Level) : base(StartPosition, Level)
        {
            characterType = "dummy_large";
            TextureFileName = "TODO";
            actualSize = new Rectangle(0,0,65,65);

            detectionDistance = 150;
            releaseAggroDistance = 160;

            maxSpeedX = 4;
            maxSpeedY = 4;

            MaxHealth = 40;

            affectedByGravity = false;

            PatrolPoint = StartPosition;

            setParameters();
        }

        public override void AdjustVerticalPosition(int direction, int rate)
        {
            base.AdjustVerticalPosition(direction, rate);
            PatrolPoint.Y += direction * rate;
        }

        public override void AdjustHorizontalPosition(int direction, int rate)
        {
            base.AdjustHorizontalPosition(direction, rate);
            PatrolPoint.X += direction * rate;
        }

        protected override void DoCombatAI(TimeSpan time)
        {
            velocity = Vector2.Zero;
            if (currentAction == null)
            {
                MineExplodeAction M = new MineExplodeAction(this);
                M.completeTrigger = SelfDestruct;
                currentAction = M;
            }
        }

        protected override void DoDefaultAI(TimeSpan time)
        {
            changeMovementTimer += time.Milliseconds;

            if (changeMovementTimer > changeMovementInterval)
            {
                Vector2 destination = new Vector2();

                destination.X = Randomizer.Random.Next(patrolRange * 2 + 1) - patrolRange;
                destination.Y = Randomizer.Random.Next(patrolRange * 2 + 1) - patrolRange;

                destination += PatrolPoint;

                velocity = destination - position;
                velocity.Normalize();
                velocity *= maxSpeedX;

                changeMovementTimer = 0;
            }
        }

        protected override bool CheckPlayerDetection()
        {
            if (DistanceToPlayer() < detectionDistance)
                return true;

            return false;
        }

        public override int GetMeleeDamage()
        {
            return 1 + (int)(level / 10);
        }

        void SelfDestruct ()
        {
            IsDead = true;
        }
    }
    
    delegate void ActionCompleteMessage();

    class MineExplodeAction : CharacterAction
    {
        public ActionCompleteMessage completeTrigger;

        public MineExplodeAction(Character Actor) : base (Actor) 
        {
            animation = new Animation(
                AnimationLists.GetAnimation("floatingmine_explode"), false);
            animation.Start();

            preDuration = 500;
            postDuration = 0;
            totalDuration = 500;
        }

        protected override void OnComplete()
        {
            completeTrigger();
        }

        protected override void OnExecute()
        {
            MineExplosion M = new MineExplosion(actor.Position, actor.Alignment);
            M.Damage = actor.GetMeleeDamage();
            LevelManager L = new LevelManager();
            L.AddGameObject(M);
        }
    }

    class MineExplosion : GameObject, IHarmfulObject
    {
        public float Damage { get; set; } = 1;
        float aliveTimer, aliveDuration = 200;
        
        public MineExplosion(Vector2 Position, ObjectAlignment Alignment)
        {
            actualSize = new Rectangle(0,0,400,400);
            position = Position;
            alignment = Alignment;
        }

        public override void Update(TimeSpan time)
        {
            aliveTimer += time.Milliseconds;
            if (aliveTimer > aliveDuration)
                IsDead = true;

            base.Update(time);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            S.Draw(T.GetTexture("TODO"),MyRectangle.AdjustExistingRectangle(BoundingBox), Color.White);
        }

        public ObjectAlignment GetAlignment()
        {
            return alignment;
        }

        public Rectangle GetBoundingBox()
        {
            return BoundingBox;
        }

        public void HitAnObject(IDestroyableObject D)
        {
            // nothing
        }
    }
}
