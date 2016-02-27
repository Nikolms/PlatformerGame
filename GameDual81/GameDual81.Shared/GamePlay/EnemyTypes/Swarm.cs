using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    // this enemy consist of 2 classes. the main class is an invisible point for the actual models to swarm around
    class Swarm : Enemy
    {
        public int SwarmCount;    // value used to track how many objects are currently swarming around this

        public Swarm(Vector2 startPosition, int Level) : base(startPosition, Level)
        {
            actualSize = new Rectangle(0,0,0,0); // the swarmNode is only a point
            maxSpeedX = 2;
            maxSpeedY = 2;

            characterType = "dummy_small";
            TextureFileName = "TODO";

            detectionDistance = 600;
            releaseAggroDistance = 1200;

            affectedByGravity = false;          // this enemy flies

             for (int x = 0; x < 10; x++)
            {
                SwarmModel S = new SwarmModel(startPosition, level, this);
                LevelManager L = new LevelManager();
                L.AddGameObject(S);
            } 

            SwarmCount = 10;
        }

        protected override void DoDefaultAI(TimeSpan time)
        {
            // do nothing
        }

        protected override void DoCombatAI(TimeSpan time)
        {
            // slowly modify movement vector towards player
            if (playerPosition.X < position.X && velocity.X > -maxSpeedX)
                velocity.X -= 0.3f ;
            if (playerPosition.Y < position.Y && velocity.Y > -maxSpeedY)
                velocity.Y -= 0.3f;
            if (playerPosition.X > position.X && velocity.X < maxSpeedX)
                velocity.X += 0.3f;
            if (playerPosition.Y > position.Y && velocity.Y < maxSpeedY)
                velocity.Y += 0.3f;
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            if (SwarmCount <= 0) IsDead = true;
        }

        protected override void setStateAndFacing()
        {
            characterState = CharacterState.Idle;
        }

        protected override bool CheckPlayerDetection()
        {
            if (DistanceToPlayer() < detectionDistance)
                return true;
            return false;
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // No graphics for this object
        }

        public override void HitByHarmfulObject(IHarmfulObject O)
        {
            // this object cannot be hit or dealt damage
            // is is removed if the swarmcount reaches 0
        }

        public override void HandleObsticleCollision(CollisionDetailObject CC, Platform collidedWith)
        {
            // no groundCollision
        }
    }

    class SwarmModel : Enemy, IHarmfulObject
    {
        Swarm swarmNode;        // the node object this moves around
        Vector2 moveDestination = new Vector2(0,0);
        int swarmRadius = 100;               // how far from swarmNode a destination should be randomized
        int changeDestinationCounter;       // counter to trigger destination changes
        int dealDamageTimer, dealDamageInterval = 250;

        public float Damage { get { return GetMeleeDamage(); } set { } }
        

        public SwarmModel(Vector2 startPosition, int Level, Swarm SwarmNode) : base(startPosition, Level)
        {
            actualSize = new Rectangle(0, 0, 20, 20); 
            maxSpeedX = 3;

            characterType = "dummy_small";
            TextureFileName = "TODO";

            affectedByGravity = false;          // this enemy flies
            swarmNode = SwarmNode;

            MaxHealth = 30;

            baseMeleeDamage = 1;

            setParameters();
        }

        protected override void setStateAndFacing()
        {
            characterState = CharacterState.Idle;
        }

        protected override void DoCombatAI(TimeSpan time)
        {
            dealDamageTimer += time.Milliseconds;
            if (dealDamageTimer > dealDamageInterval)
                baseMeleeDamage = 1;
            DoSwarming();
        }
        protected override void DoDefaultAI(TimeSpan time)
        {
            DoSwarming();
        }

        void DoSwarming()
        {
            changeDestinationCounter++;

            if (changeDestinationCounter > 19)
            {
                changeDestinationCounter = 0;       // reset counter

                moveDestination.X = swarmNode.Position.X - swarmRadius + random.Next(swarmRadius * 2);
                moveDestination.Y = swarmNode.Position.Y - swarmRadius + random.Next(swarmRadius * 2);

                velocity = moveDestination - position;
                velocity.Normalize();
                velocity *= maxSpeedX;
            }
        }

        public override void HitByHarmfulObject(IHarmfulObject O)
        {
            base.HitByHarmfulObject(O);
            // everytime a swarm model dies, reduce the count on the node
            if (IsDead) swarmNode.SwarmCount -= 1;
        }

        public void HitAnObject(IDestroyableObject D)
        {
            // set damage to 0 after hitting something
            baseMeleeDamage = 0;
            dealDamageTimer = 0;
        }

        public override int GetMeleeDamage()
        {
            return baseMeleeDamage;
        }
    }
}
