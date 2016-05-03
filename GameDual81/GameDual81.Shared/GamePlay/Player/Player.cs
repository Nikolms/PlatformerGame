using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;
using System.Diagnostics;

namespace ThielynGame.GamePlay
{
    class Player : Character
    {
        public bool ReachedEndOfLevel { get; set; }

        // GamePlay variables
        int rangedAttackSpeed = 99;
        int MeleeAttackSpeed = 150;
        float RangedCoolDown = 0;
        float MeleeCoolDown = 0;

        public int maxMana { get; set; }
        public int currentMana { get; protected set; }


        public Player() { 
            teamID = TeamID.Player;
            TextureFileName = "player_sprite";
            characterType = "player";
            this.actualSize = new Rectangle(0, 0, 40, 70);

            maxSpeedX = 5;
            acceleration = 0.75f;

            facing = FacingDirection.Right;
            MaxHealth = 100;
            armor = GameSettings.ArmorUpgrade + 1;
            ReachedEndOfLevel = false;
            

            setParameters();
        }

        /////////////////////////////////
        ///  overrides
        /////////////////////////////////

        protected override void setParameters()
        {
            base.setParameters();
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);
        }

        protected override void setStateAndFacing()
        {
            base.setStateAndFacing();
        }

        protected override void setCommonStateAnimation()
        {
            // the player combat stance overrides normal state animation
            if (characterState == CharacterState.PlayerCombat)
            {
                List<FrameObject> framelist = AnimationLists.GetAnimationFrames("player_combat");
                animation = new Animation(framelist, true);
                characterState = CharacterState.Idle;
                return;
            }

            base.setCommonStateAnimation();
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            base.Draw(S, T);
        }



        ////////////////////////////
        // player UI input methods
        ///////////////////////////


        // makes the player jump
        public void DoJump()
        {
            if (TouchesGround && currentAction == null)
                velocity.Y = -19;
        }

        public bool DoMeleeAttack()
        {
            return PlayerActionStart(new PlayerMeleeAction(this));
        }

        public bool DoSkillOne() {
            return PlayerActionStart(new PlayerArcaneMissileAction(this));
        }
        public bool DoSkillTwo() {
            return PlayerActionStart(new PlayerChargeAction(this));
        }
        public bool DoSkillThree() {
            return false;
        }


        public bool PlayerActionStart(CharacterAction A)
        {
            if (currentAction != null && !TouchesGround)
            return false;

            currentAction = A;
            return true;
        }



        // this function is used to reset player for the start of level
        // sets velocity to 0 and clears negative effects etc
        public void ResetPlayerStatus()
        {
            ReachedEndOfLevel = false;
        }

        public override int GetMeleeDamage()
        {
            return 20;
        }

        public override int GetRangedDamage()
        {
            return 10;
        }

        public override int GetSpellDamage()
        {
            return 10;
        }
    }
}
