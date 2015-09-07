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
    public delegate void GameButtonAction (GameButton sender);

    public class GameButton : BaseButton 
    {
        string textureName = "skill_buttons";

        Rectangle textureSource;
        public Rectangle TextureSource { set { textureSource = value; } }

        public event GameButtonAction onClick;

        public override bool CheckIfClicked(Vector2 inputLocation)
        {
            if (base.CheckIfClicked(inputLocation))
                onClick(this);
            return true;
        }

        public void Draw(SpriteBatch S, TextureLoader T) 
        {
            S.Draw(
                T.GetTexture(textureName),
                this.clickArea,
                textureSource,
                Color.White);
        }
    }


    class LevelGUI
    {
        Player player;

        List<GameButton> UIbuttons;
        Rectangle healthBar;



        public LevelGUI(Player player) 
        {
            this.player = player;

            healthBar = new Rectangle(10,10, 0, 60);

            UIbuttons = new List<GameButton>();

            GameButton Jump = new GameButton();
            GameButton Melee = new GameButton();
            GameButton SkillOne = new GameButton();
            GameButton SkillTwo = new GameButton();
            GameButton SkillThree = new GameButton();

            Jump.PositionAndSize = new Rectangle(1170, 658, 100, 100);
            Melee.PositionAndSize = new Rectangle(10, 658, 100, 100);
            SkillOne.PositionAndSize = new Rectangle(10, 70,100, 100);
            SkillTwo.PositionAndSize = new Rectangle(10, 240, 100, 100);
            SkillThree.PositionAndSize = new Rectangle(10, 410, 100, 100);

            Jump.TextureSource = new Rectangle(0,0,100,100);
            Melee.TextureSource = new Rectangle(100,0,100,100);
            SkillOne.TextureSource = new Rectangle(100, 0, 100, 100);
            SkillTwo.TextureSource = new Rectangle(100, 0, 100, 100);
            SkillThree.TextureSource = new Rectangle(100, 0, 100, 100);
           
            Jump.onClick += player.DoJump;
            Melee.onClick += player.DoMeleeAttack;
            SkillOne.onClick += player.DoSkillSlotOne;
            SkillTwo.onClick += player.DoSkillSlotTwo;
            SkillThree.onClick += player.DoSkillSlotThree;

            UIbuttons.Add(Jump);
            UIbuttons.Add(Melee);
            UIbuttons.Add(SkillOne);
            UIbuttons.Add(SkillTwo);
            UIbuttons.Add(SkillThree);
                
        }



        public void checkButtonClicks(List<Vector2> inputLocations) 
        {
            foreach (Vector2 input in inputLocations)
            {
                foreach (GameButton B in UIbuttons)
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
                player.DoJump(null);
            if (input.MeleeAttackInput)
                player.DoMeleeAttack(null);
            if (input.ExitGame_Input)
                ;

        }



        public void Draw(SpriteBatch S, TextureLoader T) 
        {
            healthBar.Width = player.CurrentHealth * 3;

            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(healthBar), Color.White);
                
            foreach (GameButton B in UIbuttons) 
            {
                B.Draw(S,T);
            }
        }
    }
}
