using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.GamePlay
{
    // this class contains base functionalities for non terrain objects
    // these objects have more complex physics than terrain objects
    
    public abstract class MovableObject : GameObject
    {
        protected float MAX_FALL_SPEED = 16f,
                        AIR_FRICTION = 1f,
                        GROUND_FRICTION = 0.5f;

        // rulebreaking flags
        protected bool affectedByGravity = true;
        protected bool ignoresTerrain = false;

        // flag that is used for the objects to know when they are touching ground
        public bool TouchesGround { get; protected set; }

        protected FacingDirection facing = FacingDirection.Right;
        public FacingDirection Facing { get { return facing; } }

        
        //float lastUpdate;
        protected float acceleration;
        protected Vector2 externalSpeed;

        public Vector2 TotalVelocity
        { get { return velocity + externalSpeed; } }


        public Rectangle HorizontalCollisionBox
        {
            get
            {
                Rectangle R = BoundingBox;
                R.Y += 10;
                R.Height -= 20;
                return R;
            }
        }

        public Rectangle VerticalCollisionBox
        {
            get
            {
                Rectangle R = BoundingBox;
                R.X += 10;
                R.Width -= 20;
                return R;                
            }
        }
        
        public override void Update (TimeSpan time) 
        {
            // touches ground needs to be reset by collision every frame
            TouchesGround = false;

            ApplyGravity(time);

            position += velocity;
            position += externalSpeed;

            externalSpeed.X = 0;
            externalSpeed.Y = 0;
        }

        public virtual void ApplyGravity(TimeSpan time) 
        {
            if (affectedByGravity)
            velocity.Y += 1;

            if (velocity.Y > MAX_FALL_SPEED) velocity.Y = MAX_FALL_SPEED;
        }

        public virtual void HandleObsticleCollision(CollisionDetailObject CC, Platform collidedWith)
        {
            // if the object hits its "head" zero its vertical velocity
            if (CC.directionY > 0) velocity.Y = 0;
            if (CC.directionY > 0) velocity.X = 0;

            // cap the correction distance at current velocity in that direction
            //if (CC.correctionDistanceY > Math.Abs(velocity.Y)) CC.correctionDistanceY = (int)Math.Abs(velocity.Y) + 1;
            

            // if object was moved towards negative Y, it must have touched ground
            // also register ourselves with the platform we are standing on, and set our falling speed to
            // match potential movement
            if (CC.directionY < 0)
            {
                TouchesGround = true;
                collidedWith.RegisterObjectOnTop(this);
                velocity.Y = 0;
            }
            
            AdjustHorizontalPosition(CC.directionX, CC.correctionDistanceX);
            AdjustVerticalPosition(CC.directionY, CC.correctionDistanceY);
        }

        public void AddExternalSpeed(float X, float Y) 
        {
            externalSpeed.X = X;
            externalSpeed.Y = Y;

        }

        public void ApplyForce(float X, float Y) 
        {
            velocity.Y += Y;
            velocity.X += X;
        }

    }
}