using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.GamePlay
{
    public enum AI_IdleMovement { Patrol, Guard}
    public enum AI_CombatMovement { MoveCloser, Guard, Patrol}

    abstract class Enemy : Character
    {
        // these variable are static and contain external information that all enemies
        // need to make decisions on what to do next
        public static List<Platform> AIrelevantTerrain;
        public static Point playerLocation;

        protected bool hasDetectedPlayer;

        protected AI_IdleMovement AI_idleMovement;
        protected AI_CombatMovement AI_combatMovement;

        bool collidedAlongXAxis;

        protected float detectionRange = 100;
        protected float releaseDetectRange = 150;
        protected float rangePrimaryAttack;
        protected float rangeSecondaryAttack;
        protected float rangeThirdAttack;

        bool canDoAttackPrimary;
        bool canDoAttackSecondary;
        bool canDoThirdAttack;

        float timerPrimaryAttack;
        float timerSecondaryAttack;
        float timerThirdAttack;

        protected float cooldownPrimaryAttack;
        protected float cooldownSecondaryAttack;
        protected float cooldownThirdAttack;
        
        // this cooldown is used to prevent AI from performing both attacks too fast after each other
        float timerAttackAction;
        protected float cooldownBetweenAttacks;

        public Enemy(Vector2 startPosition, int level) : base(startPosition, level) 
        {
            // default facing for all monsters
            facing = FacingDirection.Left;
            alignment = ObjectAlignment.Enemy;
        }

        
        public override void Update(TimeSpan time)
        {
            // advance all cooldown counters
            timerPrimaryAttack -= time.Milliseconds;
            timerSecondaryAttack -= time.Milliseconds;
            timerAttackAction -= time.Milliseconds;

            // if enemy is not performing any action it should evaluate
            // what it should do during this frame
            if (currentAction == null) 
            {
                checkAIRanges();
                //UpdateAI();
            }

            base.Update(time);

            collidedAlongXAxis = false;
        }

        public override void HandleObsticleCollision(CollisionDetailObject CC, Platform collidedWith)
        {
            if (CC.correctionDistanceX > 0) 
                collidedAlongXAxis = true;

            base.HandleObsticleCollision(CC, collidedWith);
            
        }

        //////////////////////////////////////
        // This is the AI section basically
        //-----------------------------------------------
        void checkAIRanges() 
        {
            // if enemy has not detected player at start of the frame, we check for possible detection
            if (!hasDetectedPlayer)
            {
                // check detection to left
                if (BoundingBox.Center.X - playerLocation.X <= detectionRange && facing == FacingDirection.Left)
                    hasDetectedPlayer = true;

                //check detection to right
                if (playerLocation.X - BoundingBox.Center.X <= detectionRange && facing == FacingDirection.Right)
                    hasDetectedPlayer = true;

                if (Math.Abs(playerLocation.Y - BoundingBox.Center.Y) > 60)
                    hasDetectedPlayer = false;
            }

            // check if enemy has lost detection every frame
            // detection release is only checked if the enemy had already detected player
            if (hasDetectedPlayer)
            {
                if (Math.Abs(playerLocation.X - position.X) > releaseDetectRange)
                    hasDetectedPlayer = false;

                // check primary attack range
                if (Math.Abs(playerLocation.X - BoundingBox.Center.X) <= rangePrimaryAttack)
                    canDoAttackPrimary = true;

                // check secondary attack range
                if (Math.Abs(playerLocation.X - BoundingBox.Center.X) <= rangeSecondaryAttack)
                    canDoAttackSecondary = true;
            }

            if (hasDetectedPlayer)
            {
                facing = playerDirection();
                DoCombatAI();
            }
            if (!hasDetectedPlayer) DoNeutralAI();

            // clear attack range checks before next frame
            canDoAttackPrimary = false;
            canDoAttackSecondary = false;
        }

        void DoCombatAI() 
        {
            if (timerAttackAction < 0)
            {
                // first resolve any attacks that can be made
                if (canDoAttackPrimary && timerPrimaryAttack < 0)
                {
                    DoPrimaryAttack();
                    return;
                }
                if (canDoAttackSecondary && timerSecondaryAttack < 0)
                {
                    DoSecondaryAttack();
                    return;
                }
            }

            // if no attacks can be made perform combat movement
            switch (AI_combatMovement) 
            {
                case AI_CombatMovement.Guard: break;

                case AI_CombatMovement.MoveCloser:
                    MoveTowardsPlayer();
                    break;

                case AI_CombatMovement.Patrol:
                    DoPatrol();
                    break;
            }
        }

        void DoNeutralAI()
        {
            switch (AI_idleMovement) 
            {
                case AI_IdleMovement.Guard: break;
                case AI_IdleMovement.Patrol:
                    DoPatrol();
                    break;
            }
        }

        void DoPatrol() 
        {
            // Do movement in the direction watching
            if (facing == FacingDirection.Left)
            {
                if (IsNextStepSafe() && !collidedAlongXAxis)
                    DoMovementLeft();

                else
                    DoMovementRight();

            }
            else
                if (facing == FacingDirection.Right)
                {
                    if (IsNextStepSafe() && !collidedAlongXAxis)
                        DoMovementRight();

                    else
                        DoMovementLeft();
                }
        }

        void MoveTowardsPlayer() 
        {
            // determine which direction to reach player
            if (playerDirection() == FacingDirection.Left) 
            {
                facing = FacingDirection.Left;
                if (IsNextStepSafe()) DoMovementLeft();
            }

            if (playerDirection() == FacingDirection.Right) 
            {
                facing = FacingDirection.Right;
                if (IsNextStepSafe()) DoMovementRight();
            }
        }

        protected virtual void DoPrimaryAttack() 
        {
            timerPrimaryAttack = cooldownPrimaryAttack;
            timerAttackAction = cooldownBetweenAttacks;
        }

        protected virtual void DoSecondaryAttack() 
        {
            timerSecondaryAttack = cooldownSecondaryAttack;
            timerAttackAction = cooldownBetweenAttacks;
        }

        protected virtual void DoThirdAttack()
        {
            timerAttackAction = cooldownBetweenAttacks;
            timerThirdAttack = cooldownThirdAttack;
        }

        // this method checks if edges in current movement direction are too high to climb back up again
        bool IsNextStepSafe()
        {
            // not safe by default unless a terrain piece changes this later in the loop
            bool isSafeFlag = false;
            Vector2 referencePoint = new Vector2(0, 0);

            //if (velocity.X == 0) isSafeFlag = true;

            // set the point we want to check for terrain based on facing
            if (facing == FacingDirection.Left)
            {
                referencePoint.X = VerticalCollisionBox.Left - 5;
                referencePoint.Y = VerticalCollisionBox.Bottom + 5;
            }

            if (facing == FacingDirection.Right)
            {
                referencePoint.X = VerticalCollisionBox.Right + 5;
                referencePoint.Y = VerticalCollisionBox.Bottom + 5;
            }

            // iterate all terrainpieces and see if any of them contain our reference point
            // if yes then there is safe ground to move on in the next frame
            foreach (Platform P in AIrelevantTerrain)
            {
                if (P.BoundingBox.Contains(referencePoint))
                    isSafeFlag = true;
            }

            return isSafeFlag;
        }

        // this method calculates in which direction player is from current position
        FacingDirection playerDirection() 
        {
            FacingDirection direction = FacingDirection.Left;

            if (playerLocation.X < BoundingBox.Center.X) direction = FacingDirection.Left;

            if (playerLocation.X > BoundingBox.Center.X) direction = FacingDirection.Right;

            return direction;
        }



    }
}
