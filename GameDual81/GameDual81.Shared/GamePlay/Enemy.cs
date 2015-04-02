using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.GamePlay
{
    public enum AIstate { Patrol, Idle, Combat, Guard}

    abstract class Enemy : Character
    {
        // these variable are static and contain external information that all enemies
        // need to make decisions on what to do next
        public static List<Platform> AIrelevantTerrain;
        public static Point playerLocation;

        protected bool hasDetectedPlayer;
        protected bool primaryRanged;

        AIstate currentAIState;
        bool collidedAlongXAxis;

        protected float detectionRange;

        public Enemy(Vector2 startPosition) : base(startPosition) 
        {
            // default facing for all monsters
            facing = Direction.Left;
            alignment = ObjectAlignment.Enemy;
        }

        
        public override void Update(TimeSpan time)
        {
            // if enemy is not performing any action it should evaluate
            // what it should do during this frame
            if (currentAction == null) 
            {
                SetAIBehaviour();
                UpdateAI();
            }

            base.Update(time);

            collidedAlongXAxis = false;
        }

        public override void HandleGroundCollision(collisionCorrection CC, Platform collidedWith)
        {
            if (CC.correctionDistanceX > 0) 
                collidedAlongXAxis = true;

            base.HandleGroundCollision(CC, collidedWith);
            
        }

        //////////////////////////////////////
        // This is the AI section basically
        //-----------------------------------------------
        void SetAIBehaviour() 
        {
            if (Math.Abs(playerLocation.X - position.X) < detectionRange &&
                Math.Abs(playerLocation.Y - position.Y) < 90)
                hasDetectedPlayer = true;
            else
                hasDetectedPlayer = false;

            if (hasDetectedPlayer) 
            {
                if (primaryRanged) currentAIState = AIstate.Guard;

                else currentAIState = AIstate.Guard;
            }

            // if enemy has not detected player, it patrols along its platform
            else 
                currentAIState = AIstate.Patrol;
        }

        void UpdateAI() 
        {
            switch (currentAIState) 
            {
                    /////////////////
                case AIstate.Patrol:

                    // Do movement in the direction watching
                    if (facing == Direction.Left)
                    {
                        if (IsNextStepSafe() && !collidedAlongXAxis)
                            DoMovementLeft();

                        else
                            DoMovementRight();
                        
                    }
                    else
                    if (facing == Direction.Right)
                    {
                        if (IsNextStepSafe() && !collidedAlongXAxis)
                            DoMovementRight();

                        else
                            DoMovementLeft();
                    }
                    
                    break;

                    ///////////////
                case AIstate.Guard:
                    if (playerLocation.X < BoundingBox.Center.X)
                        facing = Direction.Left;
                    if (playerLocation.X > BoundingBox.Center.X)
                        facing = Direction.Right;

                    break;

                    //////////////
                case AIstate.Combat: break;
            }
        }

        bool IsNextStepSafe()
        {
            // not safe by default unless a terrain piece changes this later in the loop
            bool isSafeFlag = false;
            Vector2 referencePoint = new Vector2(0, 0);

            //if (velocity.X == 0) isSafeFlag = true;

            // set the point we want to check for terrain based on facing
            if (facing == Direction.Left)
            {
                referencePoint.X = VerticalCollisionBox.Left - 5;
                referencePoint.Y = VerticalCollisionBox.Bottom + 5;
            }

            if (facing == Direction.Right)
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

    }
}
