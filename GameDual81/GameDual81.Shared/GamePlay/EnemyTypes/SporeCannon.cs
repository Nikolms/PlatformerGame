using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    class SporeCannon : Enemy
    {
        float attackCoolDown, attackInterval = 1500;

        public SporeCannon(Vector2 StartPosition, int Level) : base(StartPosition, Level)
        {
            actualSize = new Rectangle(0,0,60,60);

            maxSpeedX = 0;      // stationary enemy

            MaxHealth = 60;

            detectionDistance = 500;
            releaseAggroDistance = 520;

            baseRangeDamage = 3;

            setParameters();
        }

        // detect player regardless of facing
        protected override bool CheckPlayerDetection()
        {
            if (DistanceToPlayer() < detectionDistance)
                return true;

            return false;
        }

        protected override void DoDefaultAI(TimeSpan time)
        {
            // nothing
        }

        protected override void DoCombatAI(TimeSpan time)
        {
            attackCoolDown += time.Milliseconds;

            if (attackCoolDown > attackInterval)
            {
                attackCoolDown = 0;
                currentAction = new SporeAttackAction
                    (this);
            }
        }

        public override int GetRangedDamage()
        {
            return baseRangeDamage + (int) (level / 8);
        }
    }

    class SporeAttackAction : CharacterAction
    {

        public SporeAttackAction(Character Actor) : base(Actor)
        {
            List<FrameObject> animationFrames =
            AnimationLists.GetAnimation("cannonmonster_shoot");

            animation = new Animation(animationFrames, false);
            animation.Start();

            preDuration = 400;
            postDuration = 100;
            totalDuration = 500;
        }
        
        protected override void OnComplete()
        {
            // nothing
        }

        protected override void OnExecute()
        {
            bool negativeSwitch = false;
            LevelManager LevelM = new LevelManager();

            // Randomize x amount of projectile paths and add them to levelManager
            for (int x = 0; x < 16; x++)
            {
                Vector2 temp = new Vector2(
                    Randomizer.Random.Next(4, 8),
                    Randomizer.Random.Next(20, 28));

                temp.Y *= -1;
                if (negativeSwitch) temp.X *= -1;
                negativeSwitch = !negativeSwitch;
                

                SporeProjectile P = new SporeProjectile
                    (actor.Position, temp, actor.Alignment);
                P.Damage = actor.GetRangedDamage();
                LevelM.AddGameObject(P);
            }
        }
    }

    class SporeProjectile : PhysicsObjects, IHarmfulObject
    {
        public float Damage { get; set; }

        public SporeProjectile(Vector2 StartPosition, Vector2 Velocity, ObjectAlignment Alignment)
        {
            position = StartPosition;
            velocity = Velocity;
            alignment = Alignment;

            actualSize = new Rectangle(0,0,12,12);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(BoundingBox), actualSize, Color.White);
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
            IsDead = true;
        }

        public override void HandleObsticleCollision(CollisionDetailObject CC, Platform collidedWith)
        {
            IsDead = true;
        }
    }
}
