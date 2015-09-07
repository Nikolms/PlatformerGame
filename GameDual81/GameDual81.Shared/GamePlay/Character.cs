using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.GamePlay.Actions;
using System.Diagnostics;
using ThielynGame.GamePlay.StatusEffects;

namespace ThielynGame.GamePlay
{
    public enum CharacterState { Run, Jump, Idle, Action }
    public enum AttackEffects { None, Poison, Fragile}

    public class CharacterStatuses 
    {
        public bool Shielded;
         public float Fragile,
            Slow,
            Fury;

        public void Reset() 
        {
            Shielded = false;
            Fragile = 1;
            Slow = 1;
            Fury = 1;
        }
    }

    //base class for player and enemies
     public abstract class Character : PhysicsObjects
    {
        protected FacingDirection previousFacing;
        protected CharacterState characterState, previousState;

        public string characterType { get; protected set; }

        protected Animation animation;
        protected BaseAction currentAction;

        protected bool hasTakenDamage;
        protected float hurtImmunityTimer;

        public CharacterStatuses statusModifiers { get; protected set; }
        protected List<StatusEffect> currentEffectsList = new List<StatusEffect>();

        protected int armor;
        public int level { get; protected set; }

        public int MaxHealth { get; protected set; }
        public int CurrentHealth { get; protected set; }

        public Rectangle MeleeReach { get; protected set; }

        public float AttackSpeed { get; protected set; }

        public Character(Vector2 startPosition) 
        {
            position = startPosition;
            statusModifiers = new CharacterStatuses();
        }

         //this method is to be used by child classes to set parameters that are unknown at the time of
         // calling base constructor
        protected void setParameters () 
        {
            CurrentHealth = MaxHealth;
        }

        public override void Update(TimeSpan time)
        {
            statusModifiers.Reset();

            List<StatusEffect> tempList = new List<StatusEffect>();

            foreach (StatusEffect S in currentEffectsList) 
            {
                if (S.CheckIfTimeLeft(time, this, statusModifiers)) 
                    tempList.Add(S);
            }
            currentEffectsList = tempList;

            hurtImmunityTimer -= time.Milliseconds;

            if (hurtImmunityTimer <= 0)
                hasTakenDamage = false;

            if (currentAction != null) 
            {
                // performs update on action, and also sets it to null if it returns true
                if (currentAction.UpdateAndCheckIfDone(time))
                {
                    currentAction = null;
                }
            }

                setStateAndFacing();
                setStateAnimation();
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
            if (animation == null) return;

            Rectangle RenderBox = animation.AnimationFrameToDraw;
            RenderBox.X = BoundingBox.X - ((animation.AnimationFrameToDraw.Width - BoundingBox.Width) / 2);
            RenderBox.Y = BoundingBox.Y - ((animation.AnimationFrameToDraw.Height - BoundingBox.Height) / 2);
             
            if (facing == FacingDirection.Left)
                S.Draw(
                    T.GetTexture(TextureFileName),
                    MyRectangle.AdjustExistingRectangle(RenderBox),
                    animation.AnimationFrameToDraw,
                    Color.White
                    );

            if (facing == FacingDirection.Right)
                S.Draw(T.GetTexture(TextureFileName),
                    MyRectangle.AdjustExistingRectangle(RenderBox),
                    animation.AnimationFrameToDraw,
                    Color.White, 0,
                    Vector2.Zero,
                    Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally,
                    0);
            
        }

         // determines characters current state depending on different parameters, such as speed and direction or touches ground
        protected virtual void setStateAndFacing()
        {
            previousState = characterState;

            if (currentAction != null) 
            {
                characterState = CharacterState.Action;
                return;
            }

            if (TouchesGround)
            {
                if (velocity.X == 0)
                    characterState = CharacterState.Idle;

                else 
                    characterState = CharacterState.Run;
            }
            else 
                characterState = CharacterState.Idle;
        }

        // creates an animation for the character based on what state character is in
        protected virtual void setStateAnimation() 
         {
            // if state has not changed there is no need to change current animation
             if (characterState == previousState) return;

             AnimationLists A = new AnimationLists();
             List<FrameObject> framelist = new List<FrameObject>();

             switch (characterState) 
             {
                 case CharacterState.Action:
                     framelist = A.getAnimation(characterType + currentAction.actionAnimationName);
                     animation = new Animation(framelist, false);
                     animation.Start();
                     break;

                 case CharacterState.Run:
                        framelist = AnimationLists.GetAnimation(characterType + "_run");
                        animation = new Animation(framelist, true);
                        animation.Start();
                     break;

                 case CharacterState.Idle:
                         framelist = A.getAnimation(characterType + "_idle");
                         animation = new Animation(framelist, true);
                         animation.Start();
                     break;

                 case CharacterState.Jump:
                         framelist = A.getAnimation(characterType + "_jump");
                         animation = new Animation(framelist, true);
                     break;
             }
         }



         // functions to handle being hit by attacks and similar
         // damage reduction from armor happens in this function
        public virtual void OnReceiveDamage(int damage, bool ignoresArmor, StatusEffect effect)
        {
            // if character is still recovering from recent damage or has an active shield buff, ignore this damage
            if (hasTakenDamage || statusModifiers.Shielded) 
                return;

            // switch the invincibility on after receiving damage
            hasTakenDamage = true;
            hurtImmunityTimer = 500;

            float reduction = 1;

            if (!ignoresArmor)
                reduction = ((float)armor / 100);

            float actualDamage = damage * statusModifiers.Fragile * reduction;

            //Debug.WriteLine("DamageDealt:  "+damage+"\nDamageTaken:  " + actualDamage + "\n\n");

            CurrentHealth -= (int)actualDamage;
            if (CurrentHealth <= 0)
                IsDead = true;

            if (effect != null) OnReceiveEffect(effect);
        }

        public virtual void OnReceiveEffect(StatusEffect effect) 
        {
            currentEffectsList.Add(effect);
        }

        public virtual void OnReceiveHeal(int amount) 
        {
            CurrentHealth += amount;
        }

         public void startNewAction (BaseAction action) 
         {
             currentAction = action;
         }

         // accelerates character towards left
         public virtual void DoMovementLeft()
         {
             if (currentAction != null) return;

             if (velocity.X > 0) velocity.X = 0;
             // increase velocity only if touches ground and has not reached max speed
             if (velocity.X > -maxSpeedX)
                 // left has a negative X
                 velocity.X -= acceleration;

             // change facing on move command always
             previousFacing = facing;
             facing = FacingDirection.Left;
         }
         //accelerates character towards right
         public virtual void DoMovementRight()
         {
             if (currentAction != null) return;

             if (velocity.X < 0) velocity.X = 0;
             // increase velocity only if touches ground and has not reached max speed
             if (velocity.X < maxSpeedX)
                 velocity.X += acceleration;

             previousFacing = facing;
             facing = FacingDirection.Right;
         }



         public virtual int GetMeleeDamage ()
         {
             return 20;
         }

         public virtual int GetRangedDamage ()
         {
             return 20;
         }

         public virtual int GetSpellPower ()
         {
             return level;
         }

    }
}
