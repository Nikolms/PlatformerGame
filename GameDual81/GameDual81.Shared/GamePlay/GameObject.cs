using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.GamePlay
{
    public enum TeamID { Player, Enemy, Neutral }
    public enum FacingDirection {Left = -1, Right = 1}
    public enum AllDirections { Left, Up, Right, Down}

    public abstract class GameObject
    {
        public TeamID teamID { get; set; }

        protected Rectangle actualSize;
        protected Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }

        public int X { set { position.X = value; } }
        public int Y { set { position.Y = value; } }

        
        protected Vector2 velocity;
        protected float maxSpeedY, maxSpeedX;
        public Vector2 Velocity { get { return velocity; } }

        // the texturefile that should be loaded for the object
        // objects dont store their own textures, just the filename
        public string TextureFileName { get; protected set; }

        public bool IsDead { get; protected set; }

        
        // TODO add logic to determine if near screen or not
        public bool IsNearScreen () 
        {
            return true;
        }

        // return a rectangle based on current location and actual size
        // center the boundbox on position
        public Rectangle BoundingBox 
        {
            get 
            {
                return new Rectangle(
                    (int)position.X - actualSize.Width / 2,
                    (int)position.Y - actualSize.Height / 2,
                    actualSize.Width,
                    actualSize.Height);
            }
        }

        public virtual void Update(TimeSpan time)
        {
        }

        public abstract void Draw(SpriteBatch S, TextureLoader T);
       

        #region methods used to reposition objects related to camera or collision correction

        public virtual void AdjustHorizontalPosition(int direction, int rate)
        {
            position.X += rate * (int)direction;
        }
        public virtual void AdjustVerticalPosition(int direction, int rate) 
        {
            position.Y += rate * (int)direction;
        }

        #endregion
    }
}
