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
        

        public Player(Vector2 startPosition, int level) : base(startPosition, level) 
        {
            alignment = ObjectAlignment.Player;
            TextureFileName = "test_character";
            characterType = "player";
            this.actualSize = new Rectangle(0,0,45,70);

            maxSpeedX = 5;
            acceleration = 0.75f;
            AttackSpeed = 100;
            MeleeReach = new Rectangle(0,0,70,70);

            facing = FacingDirection.Right;
            MaxHealth = 100;
            armor = GameSettings.ArmorUpgrade + 1;
            ReachedEndOfLevel = false;

            setParameters();
        }

        ////////////////////////////
        // player UI input methods
        ///////////////////////////

       
        // makes the player jump
        public void DoJump() 
        {
            int jumpPower;
            if (TouchesGround)
            {
                jumpPower = 13 + Math.Abs((int)velocity.X);
                velocity.Y = -jumpPower;
            }
        }
       
        public void DoMeleeAttack() 
        {
             startNewAction( BaseAction.CreateAction(ActionID.MeleeAttack, this));
        }

        public void ActionInput(ActionButton G)
        {
            if (currentAction != null)
            {
                G.coolDownLeft = 0;
                return;
            }

            startNewAction(BaseAction.CreateAction(G.skill_ID, this));
            G.coolDownLeft = currentAction.CoolDown;
            G.maxCoolDown = currentAction.CoolDown;
        }
        
        // this function is used to reset player for the start of level
        // sets velocity to 0 and clears negative effects etc
        public void ResetPlayerStatus() 
        {
            currentEffectsList.Clear();
            ReachedEndOfLevel = false;
        }

        public void LevelUP()
        {
            this.level++;
        }

        public override int GetMeleeDamage()
        {
            float damage = (19 + level) * statusModifiers.meleeDamageMod;
            return (int)damage;
        }

        public override int GetSpellPower()
        {
            float spellPower = (19 + level) * statusModifiers.spellPowerMod;
            return (int)spellPower;
        }
    }
}
