using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    class Leaper : Enemy, IHarmfulObject
    {
        public float Damage
        {
            get;
            set;
        }
        int jumpPower = 12;
        int jumpSpeed = 3;
        int minimumJumpDistance = 150;
        float jumpInterval = 500, jumpTimer = 0;

        public Leaper(Vector2 StartPosition, int Level) : base(StartPosition, Level)
        {
            characterType = "dummy_small";
            TextureFileName = "TODO";

            actualSize = new Rectangle(0,0,20,20);
            maxSpeedX = 0;      //does not move other than jumping
            MaxHealth = 20;
            elevationStep = 3;

            detectionDistance = 0;
            releaseAggroDistance = 1;
           
            jumpTimer = Randomizer.Random.Next(500);   // randomize startvalue to make big packs of these to look more natural

            setParameters();
        }

        public override void Update(TimeSpan time)
        {
            if (jumpTimer < jumpInterval + 1)
            jumpTimer += time.Milliseconds;

            base.Update(time);
        }

        public override void DoMovementLeft()
        {
            // cannot alter movement while in air
            if (!TouchesGround) return;
            base.DoMovementLeft();
        }
        public override void DoMovementRight()
        {
            // cannot alter movemeent while in air
            if (!TouchesGround) return;
            base.DoMovementRight();
        }

        protected override void DoDefaultAI(TimeSpan time)
        {
            if (!TouchesGround)
                return;

            CheckNextStep(jumpPower * jumpSpeed * 2, facing);

            if (nextStepFall || nextStepWall)
            {
                ChangeFacing();
                return;
            }

            CheckNextStep(3, facing);

            if (nextStepWall || nextStepFall)
            {
                ChangeFacing();
                return;          // return if there is an obsticle after the second check
            }  

            if (jumpTimer > jumpInterval)
            {
                velocity.X += jumpSpeed * (int)facing;
                velocity.Y = -jumpPower;
                jumpTimer = 0;
            }

            else if (TouchesGround)
                velocity.X = 0;
        }
        
        protected override void DoCombatAI(TimeSpan time)
        {
            // has no separate behavior in combat
            DoDefaultAI(time);
        }

        public void HitAnObject(IDestroyableObject D)
        {
            // nothing
        }
    }
}
