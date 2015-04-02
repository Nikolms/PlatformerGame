using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.GamePlay
{
    public abstract class MovableObject : GameObject
    {
        protected float MAX_FALL_SPEED = 16f,
                        AIR_FRICTION = 1f,
                        GROUND_FRICTION = 0.08f;

        // some special objects might ignore gravity
        protected bool affectedByGravity = true;
        protected bool touchesGround;

        protected Direction facing = Direction.Right;
        public Direction Facing { get { return facing; } }

        float lastUpdate;
        protected float acceleration, maxSpeed;
        protected Vector2 velocity;
        public Vector2 Velocity { get { return velocity; } }


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
            // touches ground needs to be reset by collsion every frame
            touchesGround = false;

            ApplyGravity(time);

            position += velocity;
        }

        public virtual void ApplyGravity(TimeSpan time) 
        {
            if (affectedByGravity)
            velocity.Y += 2;

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
                velocity.Y = collidedWith.Speed.Y;
                collidedWith.RegisterObjectOnTop(this);
            }

            AdjustHorizontalPosition(CC.directionX, CC.correctionDistanceX);
            AdjustVerticalPosition(CC.directionY, CC.correctionDistanceY);

        }

    }
}