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
        protected bool touchesGround;

        protected FacingDirection facing = FacingDirection.Right;
        public FacingDirection Facing { get { return facing; } }

        
        //float lastUpdate;
        protected float acceleration;
        protected Vector2 externalSpeed;


        public Rectangle HorizontalCollisionBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y + 8, actualSize.Width, actualSize.Height - 16);
            }
        }

        public Rectangle VerticalCollisionBox
        {
            get
            {
                return new Rectangle((int)position.X + 8, (int)position.Y, actualSize.Width - 16, actualSize.Height);
            }
        }
        
        public override void Update (TimeSpan time) 
        {
            // touches ground needs to be reset by collision every frame
            touchesGround = false;

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

        public virtual void HandleGroundCollision(collisionCorrection CC, Platform collidedWith)
        {
            // if the object hits its "head" zero its vertical velocity
            if (CC.directionY > 0) velocity.Y = 0;
            

            // if object was moved towards negative Y, it must have touched ground
            // also register ourselves with the platform we are standing on, and set our falling speed to
            // match potential movement
            if (CC.directionY < 0)
            {
                touchesGround = true;
                collidedWith.RegisterObjectOnTop(this);
            }

            AdjustHorizontalPosition(CC.directionX, CC.correctionDistanceX);
            AdjustVerticalPosition(CC.directionY, CC.correctionDistanceY);

        }

        public void AddExternalSpeed(float X, float Y) 
        {
            externalSpeed.X = X;
            externalSpeed.Y = Y;

        }

    }
}