using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.GamePlay
{
    public enum TilePosition { Left, right, Middle, Top, Bottom, Custom}

    public class Platform : GameObject
    {
        protected bool isDisappearingPlatform;
        protected bool isMovingPlatform = false;
        float travelDistance, currentDistance;
        protected Vector2 speed = new Vector2(0,0);
        bool verticalMove;
        int direction;
        List<MovableObject> objectsOnTopOfThis = new List<MovableObject>();
        TilePosition tilePosition;
        Rectangle TextureSource;


        public Vector2 Speed { get { return speed; } }
        public string TextureSet { set { TextureFileName = value; } }

        public Platform(Rectangle positionAndSize, TilePosition tileposition) 
        {
            actualSize = new Rectangle(0,0,positionAndSize.Width, positionAndSize.Height);
            position.X = positionAndSize.X;
            position.Y = positionAndSize.Y;

            // TODO, only for testing purposes
            TextureFileName = "terrain_stone_simple";

            // TODO
            /*
            switch (tileposition) 
            {
                case TilePosition.Middle: TextureSource = new Rectangle(0,0,64,32); break;
                case TilePosition.Top: TextureSource = new Rectangle(64,0,64,32); break;
                case TilePosition.Custom: TextureSource = actualSize; break;
                default: TextureSource = new Rectangle(0, 0, 64, 32); break;
            } */
        }

        public Platform(Rectangle positionAndSize, string tilesetName, bool isMoving, bool verticalMove, float speed, int travelDistance)  :
            this (positionAndSize, TilePosition.Custom)
        {
            this.verticalMove = verticalMove;
            this.travelDistance = travelDistance;
            isMovingPlatform = isMoving;
            currentDistance = 0;
            direction = 1;

            if (verticalMove) this.speed.Y = speed;
            else this.speed.X = speed;
        }



        public override void Update(TimeSpan time)
        {
            // if this platform has its own movement update it
            if (isMovingPlatform) 
            {
                    position.Y += speed.Y * direction;

                    position.X += speed.X * direction;

                currentDistance += speed.X;
                currentDistance += speed.Y;

                // loop through all movable objects standing on this
                // and make their position follow platforms X movement
                //if (objectsOnTopOfThis.Count > 0) { 
                foreach(MovableObject O in objectsOnTopOfThis) 
                {
                    O.AdjustHorizontalPosition(direction, (int)speed.X);
                }

                // change distance if max travel was reached
                if (currentDistance >= travelDistance)
                {
                    direction = -direction;
                    currentDistance = 0;
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


        public void RegisterObjectOnTop(MovableObject O) 
        {
            objectsOnTopOfThis.Add(O);
        }
    }
}
