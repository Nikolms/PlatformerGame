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
    
    public abstract class PhysicsObjects : GameObject
    {
        protected float MAX_FALL_SPEED = 16f,
                        AIR_FRICTION = 1f,
                        GROUND_FRICTION = 0.5f;

        // some special objects might ignore gravity
        protected bool affectedByGravity = true;
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
                return new Rectangle((int)position.X, (int)position.Y + 10, actualSize.Width, actualSize.Height - 20);
            }
        }

        public Rectangle VerticalCollisionBox
        {
            get
            {
                return new Rectangle((int)position.X + 10, (int)position.Y, actualSize.Width - 20, actualSize.Height);
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