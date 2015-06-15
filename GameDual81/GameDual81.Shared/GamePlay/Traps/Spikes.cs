using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay.Traps
{
    public enum TravelingAxis { Horizontal, Vertical}

    class Spikes : GameObject, IPlayerInterAct
    {
        int level;
        bool MovingTrap = false;
        float travelDistance, currentDistance;
        int currentDirection;

        public Spikes(int level, Rectangle size) 
        {
            actualSize = size;
            position.X = size.X;
            position.Y = size.Y;
            this.level = level;
        }
        // constructor for moving spikes
        public Spikes(int level, Rectangle size, Vector2 speed, float travelDistance) : this (level, size)
        {
            MovingTrap = true;
            currentDistance = 0;
            this.travelDistance = travelDistance;
            velocity = speed;
        }

        public override void Update(TimeSpan time)
        {
            if (MovingTrap) 
            {
                // switch direction if max distance or zero distance is reached
                if (currentDistance <= 0) currentDirection = 1;
                if (currentDistance >= travelDistance) currentDirection = -1;

                // multiply velo with direction, to get negative or positive value depending on direction
                position += velocity * currentDirection;

                // update to current travel distance by velocity lenght
                currentDistance += velocity.Length() * currentDirection;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S, AnimationFiles.TextureLoader T)
        {
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(BoundingBox), Color.White);
        }
        

        public void CheckPlayerCollision(Player P)
        {
            if (!P.BoundingBox.Intersects(this.BoundingBox)) return;

            P.HitByAttack(10 + level);
            P.KnockBack(AllDirections.Up, 17);
        }
    }
}
