using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.GamePlay.Object_Components;

namespace ThielynGame.GamePlay
{
    public class Platform : GameObject, IObsticle
    {
        // list of objects that standing on this platform
        protected List<PhysicsObjects> objectsOnTopOfThis = new List<PhysicsObjects>();

        public string TextureSet { set { TextureFileName = value; } }

        // platforms dont center boundingbox
        public new Rectangle BoundingBox 
            {
                get {
                return new Rectangle((int)position.X, (int)position.Y,
                    actualSize.Width, actualSize.Height);
                    }
            }

        public Platform(Rectangle positionAndSize)
        {
            actualSize = new Rectangle(0, 0, positionAndSize.Width, positionAndSize.Height);
            position.X = positionAndSize.X;
            position.Y = positionAndSize.Y;

            // TODO, only for testing purposes
            TextureFileName = "terrain_black";
        }
        
        public override void Update(TimeSpan time)
        {
            // unregister all objects every frame. Objects must renew their collision
            // on later frames
            objectsOnTopOfThis.Clear();
        }
        
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S, TextureLoader T)
        {
            S.Draw(
                T.GetTexture(TextureFileName),
                MyRectangle.AdjustExistingRectangle(BoundingBox),
                Color.White);
        }


        public void RegisterObjectOnTop(PhysicsObjects O)
        {
            objectsOnTopOfThis.Add(O);
        }

        public void CheckObsticleCollision(PhysicsObjects P)
        {
            // checks simple collision
            if (P.BoundingBox.Intersects(BoundingBox))
            {
                // if collision is detected, request detailed calculations
                // and send it to collider
                CollisionDetailObject C =
                    CollisionControl.CheckObsticleY(P, BoundingBox);
                if (C.directionY < 0) RegisterObjectOnTop(P);
                P.HandleObsticleCollision(C, this);
                // repeat for X axis
                C = CollisionControl.CheckObsticleX(P, BoundingBox);
                P.HandleObsticleCollision(C, this);
            }
        }
    }

    public class MovingPlatform : Platform
    {

        float travelDistance, currentDistance;
        int currentDirection;

        public Vector2 modifiedVelocity
        { get; set; }

        public MovingPlatform(Rectangle positionAndSize, Vector2 velocity, float TravelDistance) 
            : base(positionAndSize)
        {
            maxSpeedX = velocity.X; maxSpeedY = velocity.Y;
            travelDistance = TravelDistance;
            currentDistance = 0;
        }

        public override void Update(TimeSpan time)
        {
                // switch direction if max distance or zero distance is reached
                if (currentDistance <= 0)
                {
                    currentDirection = 1;
                    velocity.X = maxSpeedX; velocity.Y = maxSpeedY;
                }

                if (currentDistance >= travelDistance)
                {
                    currentDirection = -1;
                    velocity.X = -maxSpeedX; velocity.Y = -maxSpeedY;
                }

                // multiply velo with direction, to get negative or positive value depending on direction
                position += velocity;

                // update to current travel distance by velocity lenght
                currentDistance += velocity.Length() * currentDirection;

                foreach (PhysicsObjects O in objectsOnTopOfThis)
                {
                    O.AddExternalSpeed(velocity.X, velocity.Y);
                }

            base.Update(time);
        }

    }
}
