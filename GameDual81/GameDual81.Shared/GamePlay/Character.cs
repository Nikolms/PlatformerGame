using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ThielynGame.GamePlay
{
   public enum CharacterState { Idle, Run, Jump, Fall, PlayerCombat }

  
    //base class for player and enemies
     public abstract class Character : MovableObject, IDestroyableObject
    {
        #region fields and properties
        protected FacingDirection previousFacing;
        protected CharacterState characterState, previousState;

        public string characterType { get; protected set; }

        protected Animation animation;

        protected bool hasTakenDamage;
        protected float hurtImmunityTimer;

        protected int baseMeleeDamage, baseRangeDamage, baseSpellDamage;
        protected int armor;

        public int MaxHealth { get; protected set; }
        public int CurrentHealth { get; protected set; }

        public Rectangle MeleeReach { get; protected set; }
        
        protected CharacterAction currentAction;

        static protected Random random = new Random();
        #endregion

        public Character() 
        {
            TextureFileName = "test_character";
        }

         //this method is to be used by child classes to set parameters that are unknown at the time of
         // calling base constructor
        protected virtual void setParameters () 
        {
            CurrentHealth = MaxHealth;
        }

        public override void Update(TimeSpan time)
        {
            hurtImmunityTimer -= time.Milliseconds;

            if (hurtImmunityTimer <= 0)
                hasTakenDamage = false;


            if (currentAction != null)
            {
                currentAction.Update(time);
                if (currentAction.actionComplete)
                    currentAction = null;
            }

            else {

                setStateAndFacing();
                setCommonStateAnimation();
            }

            animation.CheckIfDoneAndUpdate(time);


            // apply friction
            if (TouchesGround)
            {
                if (Math.Abs(velocity.X) < GROUND_FRICTION)
                    velocity.X = 0;

                if (velocity.X < 0) velocity.X += GROUND_FRICTION;
                if (velocity.X > 0) velocity.X -= GROUND_FRICTION;
            }

            base.Update(time);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S, TextureLoader T)
        {
            // check to avoid null references
            if (currentAction == null && animation == null) return;
            
            Rectangle SourceBox = new Rectangle(0,0,0,0);   // rectangle to determine area from spritesheet

            // if there is an action draw all its animations
            if (currentAction != null)
                SourceBox = currentAction.char_animation.AnimationFrameToDraw;
            else
                SourceBox = animation.AnimationFrameToDraw;

            Rectangle RenderBox = SourceBox;                // rectangle to determine area on screen to draw in

            RenderBox.X = BoundingBox.X - ((SourceBox.Width - BoundingBox.Width) / 2);  // center image relative to boundbox
            RenderBox.Y = BoundingBox.Y - (SourceBox.Height - BoundingBox.Height);      // the bottom of image is same a bottom of boundBox
            
            

                if (facing == FacingDirection.Left)
                S.Draw(
                    T.GetTexture(TextureFileName),
                    MyRectangle.AdjustExistingRectangle(RenderBox),
                    SourceBox,
                    Color.White
                    );

            if (facing == FacingDirection.Right)
                S.Draw(T.GetTexture(TextureFileName),
                    MyRectangle.AdjustExistingRectangle(RenderBox),
                    SourceBox,
                    Color.White, 0,
                    Vector2.Zero,
                    Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally,
                    0);
        }

         // determines characters current state depending on different parameters, such as speed and direction or touches ground
        protected virtual void setStateAndFacing()
        {
            previousState = characterState;

            if (TouchesGround)
            {
                if (velocity.X == 0)
                    characterState = CharacterState.Idle;

                else
                    characterState = CharacterState.Run;
            }
            else
            {
                if (velocity.Y <= 1)
                    characterState = CharacterState.Jump;
                if (velocity.Y > 1)
                    characterState = CharacterState.Fall; 
            }
        }

        // creates an animation for the character based on what state character is in
        protected virtual void setCommonStateAnimation() 
         {
            // if state has not changed there is no need to change current animation
            // if there is no active animation it must be recreated
             if (characterState == previousState && animation != null)
                return;

             AnimationLists A = new AnimationLists();
             List<FrameObject> framelist = new List<FrameObject>();

             switch (characterState) 
             {
                 case CharacterState.Run:
                        framelist = AnimationLists.GetAnimationFrames(characterType + "_run");
                     break;

                 case CharacterState.Idle:
                         framelist = A.getAnimation(characterType + "_idle");
                     break;

                 case CharacterState.Jump:
                         framelist = A.getAnimation(characterType + "_jump");
                     break;

                case CharacterState.Fall:
                        framelist = A.getAnimation(characterType + "_fall");
                    break;
             }

            animation = new Animation(framelist, true);
            animation.Start();
         }
                


         // accelerates character towards left
         public virtual void DoMovementLeft()
         {
            // if character has a velocity in opposite direction, halt the movement
            // and do not do anything else this frame
            if (velocity.X > 0)
            {
                velocity.X = 0;
                return;
            }

            // if there was no opposite movement, increase speed in wanted direction
            velocity.X -= acceleration; // left has a negative X

            // dont allow to exceed max speed
            if (velocity.X < -maxSpeedX)
                velocity.X = -maxSpeedX;

            // change facing on move commands that actually increase movement
            previousFacing = facing;
            facing = FacingDirection.Left;
         }
        
        //accelerates character towards right
         public virtual void DoMovementRight()
         {
            // if character has a velocity in opposite direction, halt the movement
            // and do not do anything else this frame
            if (velocity.X < 0)
            {
                velocity.X = 0;
                return;
            }

            // if there was no opposite movement, increase speed in wanted direction
            velocity.X += acceleration; // right has positive X

            // dont allow to exceed max speed
            if (velocity.X > maxSpeedX)
                velocity.X = maxSpeedX;

            // change facing on move commands that actually increase movement
            previousFacing = facing;
            facing = FacingDirection.Right;
        }


        protected virtual bool startAction(CharacterAction action)
        {
            if (currentAction == null && TouchesGround)
            {
                currentAction = action;
                return true;
            }
            return false;
        }

        protected virtual void finishAction()
        {
            currentAction = null;
        }

        // calculates healing and prevents exceeding max hp
        protected virtual void HandleHealing(int amount)
        {
            // if the heal amount would exceed maxhealth, change to heal to difference on max and current
            if (CurrentHealth + amount > MaxHealth)
                amount = MaxHealth - CurrentHealth;

            CurrentHealth += amount;
        }

        public virtual int GetMeleeDamage ()
         {
            float damage = baseMeleeDamage;
            return (int)damage;
         }

         public virtual int GetRangedDamage ()
         {
            float damage = baseRangeDamage;
            return (int)damage;
        }

        public virtual int GetSpellDamage()
        {
            float damage = baseSpellDamage;
            return (int)damage;
        }
        
        // overrides

        public virtual void HitByHarmfulObject(IHarmfulObject O)
        {
            
        }

        public Rectangle GetBoundingBox()
        {
            return BoundingBox;
        }

        public TeamID GetTeamID()
        {
            return teamID;
        }
    }
}
