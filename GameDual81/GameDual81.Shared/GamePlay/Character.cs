using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.GamePlay.Actions;
using System.Diagnostics;

namespace ThielynGame.GamePlay
{
    public enum CharacterState { Run, Jump, Idle }

    //base class for player and enemies
     public abstract class Character : MovableObject
    {
        protected FacingDirection previousFacing;
        protected CharacterState characterState, previousState;

        public string characterType { get; protected set; }

        protected Animation animation;
        protected BaseAction currentAction;

        protected bool hasTakenDamage;
        protected float hurtTimer;

        protected int maxHealth,
            armor, level;

        public int CurrentHealth { get; protected set; }
        protected Rectangle MeleeReach;

        public float AttackSpeed { get; protected set; }

        protected float speed;

        public Character(Vector2 startPosition) 
        {
            position = startPosition;
        }

         //this method is to be used by child classes to set parameters that are unknown at the time of
         // calling base constructor
        protected void setParameters () 
        {
            CurrentHealth = maxHealth;
        }

        public override void Update(TimeSpan time)
        {
            hurtTimer -= time.Milliseconds;

            if (hurtTimer <= 0)
                hasTakenDamage = false;

            if (currentAction != null) 
            {
                // performs update on action, and also sets it to null if it returns true
                if (currentAction.UpdateAndCheckIfDone(time))
                {
                    currentAction = null;
                }
            }

            if (currentAction == null)
            {
                setStateAndFacing();
                setStateAnimation();
                animation.CheckIfDoneAndUpdate(time);
            }

            // apply friction
            if (touchesGround)
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
            if (currentAction != null) currentAction.Draw(S, T);
            else
            {
                S.Draw(
                    T.GetTexture(TextureFileName),
                    MyRectangle.AdjustExistingRectangle(BoundingBox),
                    animation.AnimationFrameToDraw,
                    Color.White
                    );
            }
        }

         // determines characters current state depending on different parameters, such as speed and direction or touches ground
        protected virtual void setStateAndFacing()
        {
            previousState = characterState;

            if (touchesGround)
            {
                if (velocity.X == 0)
                    characterState = CharacterState.Idle;

                else 
                    characterState = CharacterState.Run;
            }
            else 
                characterState = CharacterState.Idle;

            /*
            if (velocity.X >= 0) 
                facing = Direction.Right;
            if (velocity.X < 0) 
                facing = Direction.Left;
             */
             
        }

        // creates an animation for the character based on what state character is in
        protected virtual void setStateAnimation() 
         {
             AnimationLists A = new AnimationLists();
             List<FrameObject> framelist = new List<FrameObject>();

             switch (characterState) 
             {
                 case CharacterState.Run:
                     // we create a new animation only if previous was not run and facing has not changed
                     if (previousState != CharacterState.Run || facing != previousFacing)
                     {
                         if (facing == FacingDirection.Left)
                             framelist = A.getAnimation(characterType + "_run_left");
                         if (facing == FacingDirection.Right)
                             framelist = A.getAnimation(characterType + "_run_right");

                         animation = new Animation(framelist, true);
                         animation.Start();
                     }
                     break;

                 case CharacterState.Idle:
                     if (previousState != CharacterState.Idle) 
                     {
                         framelist = A.getAnimation(characterType + "_idle");

                         animation = new Animation(framelist, true);
                         animation.Start();
                     }
                     break;

                 case CharacterState.Jump:
                     if (previousState != CharacterState.Jump) 
                     {
                         framelist = A.getAnimation(characterType + "_jump");

                         animation = new Animation(framelist, true);
                     }
                     break;
             }
         }


         // functions to handle being hit by attacks and similar
         // damage reduction from armor happens in this function
        public void HitByAttack(int damage)
        {
            // if character is still recovering from recent damage, ignore this damage
            if (hasTakenDamage) return;

            // switch the invincibility on after receiving damage
            hasTakenDamage = true;
            hurtTimer = 150;

            float reduction = ((float)armor / 100) * damage;
            int actualDamage = damage - (int)reduction;

            //Debug.WriteLine("DamageDealt:  "+damage+"\nDamageTaken:  " + actualDamage + "\n\n");

            CurrentHealth -= actualDamage;
            if (CurrentHealth <= 0)
                IsDead = true;
        }
         // call this function if the attack had a knockback effect
        public void KnockBack(AllDirections knockDirection, int knockingStrength) 
        {
            if (knockDirection == AllDirections.Down) velocity.Y =  knockingStrength;
            if (knockDirection == AllDirections.Left) velocity.X = knockingStrength * -1;
            if (knockDirection == AllDirections.Right) velocity.X = knockingStrength;
            if (knockDirection == AllDirections.Up) velocity.Y = knockingStrength * -1;
        }


         // accelerates character towards left
         public virtual void DoMovementLeft()
         {
             if (touchesGround && velocity.X > 0) velocity.X = 0;
             // increase velocity only if touches ground and has not reached max speed
             if (touchesGround && velocity.X > -maxSpeed)
                 // left has a negative X
                 velocity.X -= acceleration;

             // change facing on move command always
             previousFacing = facing;
             facing = FacingDirection.Left;
         }
         //accelerates character towards right
         public virtual void DoMovementRight()
         {
             if (touchesGround && velocity.X < 0) velocity.X = 0;
             // increase velocity only if touches ground and has not reached max speed
             if (touchesGround && velocity.X < maxSpeed)
                 velocity.X += acceleration;

             previousFacing = facing;
             facing = FacingDirection.Right;
         }


         public virtual void DoMeleeAttack(GameButton G)
         {
             if (currentAction == null)
             {
                 currentAction = new MeleeAttackAction(400, this);
             }
         }

         public virtual void DoRangedAttack(GameButton G)
         {
         }

         public virtual void DoSkillAction(GameButton G)
         {
         }

         public virtual int GetMeleeDamage ()
         {
             return 1;
         }

         public virtual int GetRangedDamage ()
         {
             return 1;
         }

         public virtual int GetSpellDamage ()
         {
             return 1;
         }

    }
}
