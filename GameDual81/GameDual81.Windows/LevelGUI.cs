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
using ThielynGame.GamePlay.Actions;

namespace ThielynGame.GamePlay
{
    public delegate void ActionButtonAction (ActionButton sender);
    public delegate void SimpleAction();

    public class ActionButton : BaseButton 
    {
        string textureName = "skill_buttons";

        public float maxCoolDown { get; set; }
        public float coolDownLeft { get; set; }

        public ActionID skill_ID { get; set; }

        Rectangle coolDownDrawPosition;
        Rectangle textureSource;
        public Rectangle TextureSource { set { textureSource = value; } }

        Animation coolDownAnimation = 
            new Animation(AnimationLists.GetAnimation("yellow_flash"), true);

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



        public LevelGUI(Player player) 
        {
            this.player = player;

            healthBarBase = new Rectangle(0,0,340,100);
            healthBar = new Rectangle(19,19, 0, 60);

            UIbuttons = new List<ActionButton>();
            
            ActionButton SkillOne = new ActionButton();
            ActionButton SkillTwo = new ActionButton();
            ActionButton SkillThree = new ActionButton();
            
            SkillOne.PositionAndSize = new Rectangle(10, 70,100, 100);
            SkillTwo.PositionAndSize = new Rectangle(10, 240, 100, 100);
            SkillThree.PositionAndSize = new Rectangle(10, 410, 100, 100);

            SkillOne.TextureSource = SkillData.GetSkillData(GameSettings.Skill1).imageSourcePosition;
            SkillTwo.TextureSource = SkillData.GetSkillData(GameSettings.Skill2).imageSourcePosition;
            SkillThree.TextureSource = SkillData.GetSkillData(GameSettings.Skill3).imageSourcePosition;

            SkillOne.onClick += player.ActionInput;
            SkillTwo.onClick += player.ActionInput;
            SkillThree.onClick += player.ActionInput;

            SkillOne.skill_ID = GameSettings.Skill1;
            SkillTwo.skill_ID = GameSettings.Skill2;
            SkillThree.skill_ID = GameSettings.Skill3;

            UIbuttons.Add(SkillOne);
            UIbuttons.Add(SkillTwo);
            UIbuttons.Add(SkillThree);
                
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
            if (input.moveLeftInput) 
                player.DoMovementLeft();
            if (input.moveRightInput) 
                player.DoMovementRight();
            if (input.jumpInput) 
                player.DoJump();
            if (input.MeleeAttackInput)
                player.DoMeleeAttack();
            if (input.ExitGame_Input)
                ;
            if (input.Developer_Skip)
                player.ReachedEndOfLevel = true;

        }



        public void Draw(SpriteBatch S, TextureLoader T) 
        {
            healthBar.Width = player.CurrentHealth * 3;

            S.Draw(T.GetTexture("healthbar_base"), MyRectangle.AdjustExistingRectangle(healthBarBase),
                Color.White);

            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(healthBar), Color.White);
                
            foreach (ActionButton B in UIbuttons) 
            {
                B.Draw(S,T);
            }
        }
    }
}
