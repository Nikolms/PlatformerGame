using ThielynGame.AnimationFiles;
using ThielynGame.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ThielynGame.GamePlay
{
    public delegate void ActionButtonAction (ActionButton sender);
    public delegate void SimpleAction();

    public class ActionButton : BaseButton 
    {
        string textureName = "skill_buttons";

        public float maxCoolDown { get; set; }
        public float coolDownLeft { get; set; }

        Rectangle coolDownDrawPosition;
        Rectangle textureSource;
        public Rectangle TextureSource { set { textureSource = value; } }

        Animation coolDownAnimation = 
            new Animation(AnimationLists.GetAnimationFrames("yellow_flash"), true);

        public event ActionButtonAction onClick;

        public ActionButton()
        {
            coolDownLeft = 0;
        }

        public override bool CheckIfClicked(Vector2 inputLocation)
        {
            if (base.CheckIfClicked(inputLocation) && coolDownLeft <= 0)
                onClick(this);
            return true;
        }
        
        public void UpdateCooldowns(TimeSpan time)
        {
            coolDownLeft -= (float)time.TotalMilliseconds;

            if (coolDownAnimation != null) coolDownAnimation.CheckIfDoneAndUpdate(time);

            float modHeight = (coolDownLeft / maxCoolDown) * clickArea.Height;
            coolDownDrawPosition = clickArea;
            coolDownDrawPosition.Height = (int)modHeight;
        }

        public void Draw(SpriteBatch S, TextureLoader T) 
        {
            S.Draw(
                T.GetTexture(textureName),
                this.clickArea,
                textureSource,
                Color.White);

            if (coolDownLeft > 0)
            {
                S.Draw
                    (T.GetTexture("button_cooldown"),
                    coolDownDrawPosition,
                    coolDownAnimation.AnimationFrameToDraw,
                    Color.White
                    );
            }
        }
    }

    class LevelGUI
    {
        Player player;

        List<ActionButton> UIbuttons;
        Rectangle healthBar, healthBarBase;
        Rectangle manaBar;
        Vector2 AmmoInfoLocation;
        Vector2 mouseLocation;
        

        public LevelGUI(Player player) 
        {
            this.player = player;

            healthBarBase = new Rectangle(0,0,340,100);
            healthBar = new Rectangle(19,19, 0, 60);

            manaBar = new Rectangle(0,102, 0, 40);

            AmmoInfoLocation = new Vector2(740, 10);

            UIbuttons = new List<ActionButton>();
        }

        public void UpdateGUI(TimeSpan time)
        {
            foreach (ActionButton G in UIbuttons)
            {
                G.UpdateCooldowns(time);
            }
        }

        public void checkButtonClicks(List<Vector2> inputLocations) 
        {
            foreach (Vector2 input in inputLocations)
            {
                foreach (ActionButton B in UIbuttons)
                {
                    B.CheckIfClicked(input);
                }
            }
        }



        public void PlayerNonVisualInput(InputHandler input)
        {
            if (input.MeleeAttackInput)
                player.DoMeleeAttack();

            if (input.SKill_1_Input)
                player.DoSkillOne();
            if (input.Skill_2_Input)
                player.DoSkillTwo();
            if (input.Skill_3_Input)
                player.DoSkillThree();

            if (input.moveLeftInput) 
                player.DoMovementLeft();
            if (input.moveRightInput) 
                player.DoMovementRight();
            if (input.jumpInput) 
                player.DoJump();
            if (input.ExitGame_Input)
                ;
            if (input.Developer_Skip)
                player.ReachedEndOfLevel = true;
        }



        public void Draw(SpriteBatch S, TextureLoader T) 
        {
            healthBar.Width = player.CurrentHealth * 3;
            manaBar.Width = player.CurrentHealth * 2;

            S.Draw(T.GetTexture("healthbar_base"), MyRectangle.AdjustExistingRectangle(healthBarBase),
                Color.White);

            // draw healthbar
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(healthBar), Color.White);

            // draw manaBar
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(manaBar), Color.White);

            
            foreach (ActionButton B in UIbuttons) 
            {
                B.Draw(S,T);
            }
        }
    }
}
