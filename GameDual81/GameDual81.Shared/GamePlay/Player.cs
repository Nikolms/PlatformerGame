using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.GamePlay
{
    class Player : Character
    {
        public bool ReachedEndOfLevel { get; set; }

        public Player(Vector2 startPosition) : base(startPosition) 
        {
            alignment = ObjectAlignment.Player;
            TextureFileName = "player_sprite";
            characterType = "player";
            this.actualSize = new Rectangle(0,0,45,70);

            maxSpeedX = 5;
            acceleration = 0.66f;
            AttackSpeed = 400;

            facing = FacingDirection.Right;
            maxHealth = 100;
            armor = 15;
            ReachedEndOfLevel = false;

            setParameters();
        }

        ////////////////////////////
        // player UI input methods
        ///////////////////////////

       
        // makes the player jump
        public void DoJump(GameButton G) 
        {
            int jumpPower;
            if (touchesGround)
            {
                jumpPower = 13 + Math.Abs((int)velocity.X);
                velocity.Y = -jumpPower;
            }
        }

        // this function is used to reset player for the start of level
        // sets velocity to 0 and clears negative effects etc
        public void ResetPlayerStatus() 
        {
            ReachedEndOfLevel = false;
        }
    }
}
