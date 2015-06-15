using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.GamePlay
{
    public class Platform : GameObject
    {
        protected bool isDisappearingPlatform = false;

        protected bool isMovingPlatform = false;
        float travelDistance, currentDistance;
        int currentDirection;

        // list of objects that standing on this platform
        List<PhysicsObjects> objectsOnTopOfThis = new List<PhysicsObjects>();

        public string TextureSet { set { TextureFileName = value; } }

        public Platform(Rectangle positionAndSize) 
        {
            actualSize = new Rectangle(0,0,positionAndSize.Width, positionAndSize.Height);
            position.X = positionAndSize.X;
            position.Y = positionAndSize.Y;

            // TODO, only for testing purposes
            TextureFileName = "terrain_stone";
        }

        public Platform(Rectangle positionAndSize, string tilesetName, Vector2 velocity, float travelDistance)  :
            this (positionAndSize)
        {
            maxSpeedX = velocity.X; maxSpeedY = velocity.Y;
            isMovingPlatform = true;
            this.travelDistance = travelDistance;
            currentDistance = 0;
        }



        public override void Update(TimeSpan time)
        {
            // if this platform has its own movement update it
            if (isMovingPlatform) 
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
            }
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
    }
}
