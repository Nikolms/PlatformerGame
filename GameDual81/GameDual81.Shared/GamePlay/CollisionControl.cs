using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.GamePlay
{
    public struct CollisionDetailObject 
    {
        public int directionY,
            directionX,
            correctionDistanceY,
            correctionDistanceX;
    }

    class CollisionControl
    {
        // TODO remove this function. Replaced by new ones
        /*
        public static void CheckGroundCollision(PhysicsObjects collider, List<Platform> terrain) 
        {
            foreach (Platform P in terrain) 
            {
                if (collider.BoundingBox.Intersects(P.BoundingBox)) 
                {
                    // calculate the correction distances depending on entry direction
                    CollisionDetailObject CC = new CollisionDetailObject();
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
                        collider.HandleObsticleCollision(CC, P);
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
                        collider.HandleObsticleCollision(CC, P);
                    }
                    
                }
                
            }
        }
         */
        public static CollisionDetailObject CheckObsticleY(PhysicsObjects collider, Rectangle Obsticle)
        {
            CollisionDetailObject CC = new CollisionDetailObject();
            CC.correctionDistanceX = 0; CC.correctionDistanceY = 0;
            CC.directionX = 0; CC.directionY = 0;

            if (!collider.VerticalCollisionBox.Intersects(Obsticle)) return CC;
            
                // calculate collision correction based on colliders vertical direction
                if (collider.VerticalCollisionBox.Y < Obsticle.Bottom &&
                    collider.VerticalCollisionBox.Y > Obsticle.Y)
                {
                    CC.directionY = 1;
                    CC.correctionDistanceY = Math.Abs(collider.BoundingBox.Y - Obsticle.Bottom);
                }

                if (collider.VerticalCollisionBox.Bottom < Obsticle.Bottom &&
                    collider.VerticalCollisionBox.Bottom > Obsticle.Y)
                {
                    CC.directionY = -1;
                    CC.correctionDistanceY = Math.Abs(Obsticle.Y - collider.BoundingBox.Bottom);
                }
                
                    if (CC.correctionDistanceY > Math.Abs(collider.TotalVelocity.Y))
                      CC.correctionDistanceY = (int)Math.Abs(collider.TotalVelocity.Y) + 1;

                return CC;
            
        }

        public static CollisionDetailObject CheckObsticleX (PhysicsObjects collider, Rectangle Obsticle)
        {
            CollisionDetailObject CC = new CollisionDetailObject();
            CC.correctionDistanceX = 0; CC.correctionDistanceY = 0;
            CC.directionX = 0; CC.directionY = 0;

            if (!collider.HorizontalCollisionBox.Intersects(Obsticle)) return CC;

            if (collider.HorizontalCollisionBox.Left > Obsticle.X &&
                collider.HorizontalCollisionBox.Left < Obsticle.Right)
            {
                CC.directionX = 1;
                CC.correctionDistanceX = Math.Abs(collider.BoundingBox.X - Obsticle.Right);
            }

            if (collider.HorizontalCollisionBox.Right > Obsticle.X &&
                collider.HorizontalCollisionBox.Right < Obsticle.Right)
            {
                CC.directionX = -1;
                CC.correctionDistanceX = Math.Abs(Obsticle.X - collider.BoundingBox.Right);
            }

            //if (CC.correctionDistanceX > Math.Abs(collider.TotalVelocity.X))
            //    CC.correctionDistanceX = (int)Math.Abs(collider.TotalVelocity.X) + 1;

            return CC;
        }

        public static void CheckObjectCollision(IHarmfulObject H, IDestroyableObject D)
        {
            if (H.GetAlignment() == D.GetAlignment()) return;

            if (H.GetBoundingBox().Intersects(D.GetBoundingBox()))
            {
                D.HitByHarmfulObject(H);
                H.HitAnObject(D);
            }
        }
    }
}
