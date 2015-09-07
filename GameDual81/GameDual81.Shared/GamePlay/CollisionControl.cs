using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.GamePlay
{
    public struct collisionCorrection 
    {
        public int directionY,
            directionX,
            correctionDistanceY,
            correctionDistanceX;
    }

    class GroundCollisionControl
    {
        public static void CheckGroundCollision(PhysicsObjects collider, List<Platform> terrain) 
        {
            foreach (Platform P in terrain) 
            {
                if (collider.BoundingBox.Intersects(P.BoundingBox)) 
                {
                    // calculate the correction distances depending on entry direction
                    collisionCorrection CC = new collisionCorrection();
                    CC.correctionDistanceX = 0; CC.correctionDistanceY = 0;
                    CC.directionX = 0; CC.directionY = 0;

                    // Y - Axis
                    if (collider.VerticalCollisionBox.Intersects(P.BoundingBox))
                    {
                        // calculate collision correction based on colliders vertical direction
                        if (collider.VerticalCollisionBox.Y < P.BoundingBox.Bottom &&
                            collider.VerticalCollisionBox.Y > P.BoundingBox.Y)
                        {
                            CC.directionY = 1;
                            CC.correctionDistanceY = P.BoundingBox.Height - (collider.BoundingBox.Y - P.BoundingBox.Y);
                        }

                        if (collider.VerticalCollisionBox.Bottom < P.BoundingBox.Bottom &&
                            collider.VerticalCollisionBox.Bottom > P.BoundingBox.Y)
                        {
                            CC.directionY = -1;
                            CC.correctionDistanceY = collider.BoundingBox.Height - (P.BoundingBox.Y - collider.BoundingBox.Y);
                        }
                        CC.correctionDistanceY = Math.Abs(CC.correctionDistanceY);
                        collider.HandleGroundCollision(CC, P);
                    }

                    // reset the correction values after Y- axis is resolved
                    CC.correctionDistanceX = 0; CC.correctionDistanceY = 0;
                    CC.directionX = 0; CC.directionY = 0;

                    // X - Axis
                    if (collider.HorizontalCollisionBox.Intersects(P.BoundingBox))
                    {
                        // calculate collision correction based on colliders movement direction
                        if (collider.HorizontalCollisionBox.Left > P.BoundingBox.X &&
                            collider.HorizontalCollisionBox.Left < P.BoundingBox.Right)
                        {
                            CC.directionX = 1;
                            CC.correctionDistanceX = P.BoundingBox.Width - (collider.BoundingBox.X - P.BoundingBox.X);
                        }

                        if (collider.HorizontalCollisionBox.Right > P.BoundingBox.X &&
                            collider.HorizontalCollisionBox.Right < P.BoundingBox.Right)
                        {
                            CC.directionX = -1;
                            CC.correctionDistanceX = collider.BoundingBox.Width - (P.BoundingBox.X - collider.BoundingBox.X);
                            
                        }

                        CC.correctionDistanceX = Math.Abs(CC.correctionDistanceX);
                        collider.HandleGroundCollision(CC, P);
                    }
                    
                }
                
            }
        }

    }
}
