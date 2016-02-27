using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    class PlasmaWalker : Enemy
    {
        float attackCooldDown = 0, attackSpeed = 1500;


        public PlasmaWalker(Vector2 Position, int Level) : base (Position, Level)
        {
            detectionDistance = 300;
            releaseAggroDistance = 400;
            actualSize = new Rectangle(0,0,75,70);

            characterType = "cannonmonster";
            TextureFileName = "cannonmonster_sprite";

            acceleration = 1f;
            maxSpeedX = 4;
            elevationStep = 3;

            MaxHealth = 80;

            setParameters();
        }

        public override void Update(TimeSpan time)
        {
            attackCooldDown += time.Milliseconds;
            base.Update(time);
        }

        protected override void DoDefaultAI(TimeSpan time)
        {
            ConstantPatrol();
        }

        protected override void DoCombatAI(TimeSpan time)
        {
            if (!isFacingPlayer() && facing == FacingDirection.Left)
                facing = FacingDirection.Right;
            if (!isFacingPlayer() && facing == FacingDirection.Right)
                facing = FacingDirection.Left;

            if (attackCooldDown < attackSpeed) return;
            attackCooldDown = 0;

            if (currentAction == null)
            {
                PlasmaShootAction P = new PlasmaShootAction(this);
                P.projectileVelo = new Vector2((int)facing * 7, 0);
                P.Damage = 50;

                currentAction = P;
            }

        }
    }

    class PlasmaFlyer : Enemy
    {

        float changeMovementTimer, changeMovementInterval = 1000;
        float attackTimer, attackInterval = 2000;
        int minimumAttackRange = 100;
        int patrolRange = 600;
        Vector2 patrolPoint;


        public PlasmaFlyer(Vector2 StartPosition, int Level) : base(StartPosition, Level)
        {
            characterType = "dummy_medium";
            TextureFileName = "TODO";

            actualSize = new Rectangle(0,0,40,70);

            maxSpeedX = 3;
            maxSpeedY = 3;
            affectedByGravity = false;      // flying enemy

            primaryAttackRange = 400;

            detectionDistance = 700;
            releaseAggroDistance = 800;

            MaxHealth = 80;

            patrolPoint = StartPosition;

            setParameters();
        }

        public override void AdjustHorizontalPosition(int direction, int rate)
        {
            base.AdjustHorizontalPosition(direction, rate);
            patrolPoint.X += direction * rate;
        }

        public override void AdjustVerticalPosition(int direction, int rate)
        {
            base.AdjustVerticalPosition(direction, rate);
            patrolPoint.Y += direction * rate;
        }

        protected override void DoDefaultAI(TimeSpan time)
        {
            if (changeMovementTimer < changeMovementInterval + 1)
            changeMovementTimer += time.Milliseconds;

            if (changeMovementTimer > changeMovementInterval)
            {
                Vector2 destination = new Vector2();

                destination.X = Randomizer.Random.Next(patrolRange * 2 + 1) - patrolRange;
                destination.Y = Randomizer.Random.Next(patrolRange * 2 + 1) - patrolRange;

                destination += patrolPoint;

                velocity = destination - position;
                velocity.Normalize();
                velocity *= maxSpeedX;

                // adjust facing accordingly
                if (velocity.X < 0) facing = FacingDirection.Left;
                else facing = FacingDirection.Right;

                changeMovementTimer = 0;
            }
        }

        protected override void DoCombatAI(TimeSpan time)
        {
            attackTimer += time.Milliseconds;

            // no update if an action is being performed
            if (currentAction != null)
                return;

            // start shooting action if possible
            if (DistanceToPlayer() < primaryAttackRange && DistanceToPlayer() > minimumAttackRange
                && isFacingPlayer() && currentAction == null && attackTimer > attackInterval)
            {
                attackTimer = 0;

                Vector2 ProjectileDirection = new Vector2(playerPosition.X, playerPosition.Y);
                ProjectileDirection -= position;
                ProjectileDirection.Normalize();
                ProjectileDirection *= 7;

                PlasmaShootAction P = new PlasmaShootAction(this);
                P.projectileVelo = ProjectileDirection;
                P.Damage = 50;
                currentAction = P;
                return;
            }


            // Combat behaviour when in range but cannot attack
            if (DistanceToPlayer() < primaryAttackRange -50 && playerPosition.X < BoundingBox.Center.X)
            {
                velocity.X = 3.5f;
                velocity.Y = -2;
                facing = FacingDirection.Left;
            }
            if (DistanceToPlayer() < primaryAttackRange - 50 && playerPosition.X >= BoundingBox.Center.X)
            {
                velocity.X = -3.5f;
                velocity.Y = -2;
                facing = FacingDirection.Right;
            }

            // Combat behaviour when not in attack range
            if (DistanceToPlayer() > primaryAttackRange - 50)
            {
                // slowly modify movement vector towards player
                if (playerPosition.X < position.X && velocity.X > -maxSpeedX)
                    velocity.X -= 0.3f;
                if (playerPosition.Y < position.Y && velocity.Y > -maxSpeedY)
                    velocity.Y -= 0.3f;
                if (playerPosition.X > position.X && velocity.X < maxSpeedX)
                    velocity.X += 0.3f;
                if (playerPosition.Y > position.Y && velocity.Y < maxSpeedY)
                    velocity.Y += 0.3f;
            }
        }
    }

    class PlasmaShootAction : CharacterAction
    {
        public Vector2 projectileVelo;
        public int Damage = 10;

        public PlasmaShootAction(Character Actor) : base (Actor)
        {
            List<FrameObject> frames = AnimationLists.GetAnimation("cannonmonster_shoot");
            animation = new Animation(frames, false);
            animation.Start();

            preDuration = 400;
            postDuration = 200;
            totalDuration = preDuration + postDuration;
        }

        protected override void OnComplete()
        {
            // nothing
        }

        protected override void OnExecute()
        {
            Vector2 projectileStart = new Vector2(actor.BoundingBox.Center.X, actor.BoundingBox.Center.Y);
            PlasmaBall ball = new PlasmaBall(projectileStart, projectileVelo, actor.Alignment);
            ball.Damage = Damage;
            LevelManager L = new LevelManager();
            L.AddGameObject(ball);
        }
    }


    class PlasmaBall : PhysicsObjects, IHarmfulObject
    {
        public float Damage
        {
            get;

            set;
        }


        public PlasmaBall(Vector2 start, Vector2 direction, ObjectAlignment Alignment)
        {
            position = start;
            actualSize = new Rectangle(0,0,30,30);
            velocity = direction;
            affectedByGravity = false;
            alignment = Alignment;
        }


        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(BoundingBox), Color.White);
        }

        public Rectangle GetBoundingBox()
        {
            return BoundingBox;
        }

        public override void HandleObsticleCollision(CollisionDetailObject CC, Platform collidedWith)
        {
            IsDead = true;
        }

        public void HitAnObject(IDestroyableObject D)
        {
            IsDead = true;
        }

        public ObjectAlignment GetAlignment()
        {
            return alignment;
        }
    }
}
