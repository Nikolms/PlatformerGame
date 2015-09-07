using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.GamePlay.Actions;
using ThielynGame.GamePlay.StatusEffects;

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
            acceleration = 0.75f;
            AttackSpeed = 100;
            MeleeReach = new Rectangle(0,0,70,70);

            facing = FacingDirection.Right;
            MaxHealth = 100;
            armor = GameSettings.ArmorUpgrade + 0;
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
            if (TouchesGround)
            {
                jumpPower = 13 + Math.Abs((int)velocity.X);
                velocity.Y = -jumpPower;
            }
        }

        public void DoSkillSlotOne(GameButton G) 
        {
            if (currentAction == null)
                startNewAction(BaseAction.CreateAction(ActionType.Charge, this));
        }
        public void DoSkillSlotTwo(GameButton G) 
        {
        }
        public void DoSkillSlotThree(GameButton G) 
        {
        }

        public void DoMeleeAttack(GameButton G) 
        {
             startNewAction( BaseAction.CreateAction(ActionType.MeleeAttack, this));
        }

        // this function is used to reset player for the start of level
        // sets velocity to 0 and clears negative effects etc
        public void ResetPlayerStatus() 
        {
            ReachedEndOfLevel = false;
        }
    }
}
