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
        public Player(Vector2 startPosition) : base(startPosition) 
        {
            alignment = ObjectAlignment.Player;
            TextureFileName = "player_sprite";
            characterType = "player";
            this.actualSize = new Rectangle(0,0,60,90);

            maxSpeed = 5;
            acceleration = 0.24f;
            AttackSpeed = 400;

            facing = Direction.Right;
        }

        ////////////////////////////
        // player UI input methods
        ///////////////////////////

       
        // makes the player jump
        public void DoJump(GameButton G) 
        {
            if (touchesGround)
                velocity.Y = -24;
        }
    }
}
