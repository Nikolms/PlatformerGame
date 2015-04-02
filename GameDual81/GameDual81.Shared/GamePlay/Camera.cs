using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.GamePlay
{
    class Camera
    {
        public static void FocusCameraOnPlayer(List<GameObject> objectsToMove, Player P) 
        {
            bool needAdjustment = false;

            int directionY = 1,
                directionX = 1,
                offSetX = 0,
                offSetY = 0;

            // if player is not in the middle of x axis
            if (P.BoundingBox.X != 610) 
            {
                offSetX = 610 - P.BoundingBox.X;

                // divide with the abs value to regain -1 or 1 for direction values
                //directionX = offSetX / Math.Abs(offSetX);

                // set flag if a change is needed
                needAdjustment = true;
            }

            // same here as above but for Y axis
            if ( P.BoundingBox.Y != 339)
            {
                offSetY = 339 - P.BoundingBox.Y;
                
                //directionY = offSetY / Math.Abs(offSetY);

                needAdjustment = true;
            }

                // Move all game objects including player by the amount needed
                // so that player is in middle of screen again
            if (needAdjustment) 
            {

                foreach (GameObject G in objectsToMove) 
                {
                    G.AdjustHorizontalPosition(directionX, offSetX);
                    G.AdjustVerticalPosition(directionY, offSetY);
                }

                // Also Move the player
                P.AdjustHorizontalPosition(directionX, offSetX);
                P.AdjustVerticalPosition(directionY, offSetY);
            }
            
        }
    }
}
