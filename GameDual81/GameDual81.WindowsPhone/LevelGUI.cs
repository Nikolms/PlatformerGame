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
    public delegate void ActionButtonAction(ActionButton sender);
    public delegate void SimpleAction();

    // simple button for continuous actions and no cooldowns (run jump)
    public class SimpleGameButton : BaseButton
    {
        string textureName = "TODO";
        public Rectangle textureSource { get; set; }

        public event SimpleAction OnClick;

        public override bool CheckIfClicked(Vector2 inputLocation)
        {
            if (base.CheckIfClicked(inputLocation))
                OnClick();
            return false;
        }
        
    }

    // button class that can handle cooldowns for player skills etc
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

        List<BaseButton> UIbuttons;   // this list has all basic game buttons
        List<ActionButton> SkillButtons;  // this list contains all buttons that need to handle cooldowns
        Rectangle healthBar, healthBarBase;
        Rectangle energyBar;

        bool MoveLeftInput, MoveRightInput, JumpInput;


        public LevelGUI(Player player)
        {
            this.player = player;

            healthBarBase = new Rectangle(0, 0, 340, 100);
            healthBar = new Rectangle(19, 19, 0, 60);
            energyBar = new Rectangle(0, 102, 0, 40);

            UIbuttons = new List<BaseButton>();

            SimpleGameButton Jump = new SimpleGameButton();
            SimpleGameButton MoveLeft = new SimpleGameButton();
            SimpleGameButton MoveRight = new SimpleGameButton();

            Jump.OnClick += SetJumpInput;
            MoveLeft.OnClick += SetMoveLeftInput;
            MoveRight.OnClick += SetMoveRightInput;

            Jump.PositionAndSize = MyRectangle.AdjustSizeCustomRectangle (0, 0, 500, 450);
            MoveLeft.PositionAndSize = MyRectangle.AdjustSizeCustomRectangle(0,0,250, 768);
            MoveRight.PositionAndSize = MyRectangle.AdjustSizeCustomRectangle(250,0,250,768);

            UIbuttons.Add(Jump);
            UIbuttons.Add(MoveLeft);
            UIbuttons.Add(MoveRight);

        }

        public void UpdateGUI(TimeSpan time)
        {
            MoveLeftInput = false;
            MoveRightInput = false;
            JumpInput = false;
        }

        public void checkButtonClicks(List<Vector2> inputLocations)
        {
            foreach (Vector2 input in inputLocations)
            {
                foreach (BaseButton B in UIbuttons)
                {
                    B.CheckIfClicked(input);
                }
            }
        }



        public void PlayerNonVisualInput(InputHandler input)
        {
            if (MoveLeftInput) player.DoMovementLeft();
            if (MoveRightInput) player.DoMovementRight();

            if (JumpInput) player.DoJump();
        }



        public void Draw(SpriteBatch S, TextureLoader T)
        {
            healthBar.Width = player.CurrentHealth * 3;
            energyBar.Width = player.currentMana * 2;

            S.Draw(T.GetTexture("healthbar_base"), MyRectangle.AdjustExistingRectangle(healthBarBase),
                Color.White);

            // draw healthbar
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(healthBar), Color.White);

            // draw energybar
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(energyBar), Color.White);

            foreach (BaseButton B in UIbuttons)
            {
                B.Draw(S);
            }
        }


        // BUTTON METHODS
        void SetJumpInput()
        {
            JumpInput = true;
        }
        void SetMoveLeftInput()
        {
            MoveLeftInput = true;
        }
        void SetMoveRightInput()
        {
            MoveRightInput = true;
        }
    }
}
