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

    public class AttackDetailObject
    {
        public int damage;
        public int healing;
        public bool ignoresArmor;
        public List<StatusEffect> BuffEffects = new List<StatusEffect>();
        public List<StatusEffect> DeBuffEffect = new List<StatusEffect>();
    }
    public class CharacterStatuses 
    {
        public bool CannotAttack;
        public bool StealsLife;
        public bool Shielded;
        public float receiveDamageMod,
           moveSpeedMod,
           meleeDamageMod,
           rangedDamageMod,
           spellPowerMod,
           gravityMod;


        public void Reset() 
        {
            CannotAttack = false;
            StealsLife = false;
            Shielded = false;
            receiveDamageMod = 1;
            moveSpeedMod = 1;
            meleeDamageMod = 1;
            rangedDamageMod = 1;
            spellPowerMod = 1;
            gravityMod = 0;
        }
    }

    //base class for player and enemies
     public abstract class Character : PhysicsObjects
    {
        #region fields and properties
        protected FacingDirection previousFacing;
        protected CharacterState characterState, previousState;

        public string characterType { get; protected set; }

        protected Animation animation;
        protected BaseAction currentAction;

        protected bool hasTakenDamage;
        protected float hurtImmunityTimer;

        public CharacterStatuses statusModifiers { get; protected set; }
        protected List<StatusEffect> currentEffectsList = new List<StatusEffect>();

        protected int baseMeleeDamage, baseRangeDamage, baseSpellPower;
        protected int armor;
        public int level { get; protected set; }

        public int MaxHealth { get; protected set; }
        public int CurrentHealth { get; protected set; }

        public Rectangle MeleeReach { get; protected set; }

        public float AttackSpeed { get; protected set; }

        // characters can modify their speed values through effects
        protected float actualMaxSpeed;

        #endregion

        public Character(Vector2 startPosition, int level) 
        {
            TextureFileName = "test_character";

            this.level = level;
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

            foreach (StatusEffect E in currentEffectsList)
            {
                E.Draw(S, T);
            }

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
        
        // primary function to handle receiving damage and effects
        
        // this function handles statuses like immunity. use this to send damage and hostile effects        
        public virtual void OnReceiveAttackOrEffect(AttackDetailObject attack)
        {
            // resolve are positive effects first
            HandleHealing(attack.healing);
            foreach (StatusEffect S in attack.BuffEffects) { HandleNewEffect(S); }

            // dont resolve negative effects if character is immune
            if (statusModifiers.Shielded || hasTakenDamage) return;

            hasTakenDamage = true;
            hurtImmunityTimer = 500;

            HandleReceivedDamage(attack.damage, attack.ignoresArmor);

            Debug.WriteLine(characterType + " got hit!");
            foreach (StatusEffect S in attack.BuffEffects)
            {
                HandleNewEffect(S);
            }
        }
       
        // damage reduction from armor happens in this function
        protected virtual void HandleReceivedDamage(int damage, bool ignoresArmor)
        {
            float reduction = 1;

            if (!ignoresArmor)
                reduction = 1 - ((float)armor / 100);

            float actualDamage = damage * statusModifiers.receiveDamageMod * reduction;
            if (actualDamage < 1) actualDamage = 1;

            //Debug.WriteLine(characterType + "takes " + actualDamage + "damage!");

            CurrentHealth -= (int)actualDamage;
            if (CurrentHealth <= 0)
                IsDead = true;
        }

        // applies new status effects and prevents duplicates
        protected virtual void HandleNewEffect(StatusEffect effect) 
        {
            if (effect == null)
                return;

            bool isExistingStatus = false;

                foreach (StatusEffect SE in currentEffectsList)
                {
                    if (SE.GetType() == effect.GetType())
                    {
                        SE.ReplaceWithNewInstance(effect.duration);
                        isExistingStatus = true;
                    }
                }

            if (!isExistingStatus)
                currentEffectsList.Add(effect);
        }

        // calculates healing and prevents exceeding max hp
        protected virtual void HandleHealing(int amount) 
        {
            // if the heal amount would exceed maxhealth, change to heal to difference on max and current
            if (CurrentHealth + amount > MaxHealth)
                amount = MaxHealth - CurrentHealth;
                        
            CurrentHealth += amount;
        }

        // clears all status effects
        public virtual void ClearStatuses()
        {
            currentEffectsList.Clear();
        }

        // returns false if new action was not started
        public bool startNewAction (BaseAction action) 
         {
            if (action.CanBeUsedInAir || TouchesGround)
            {
                currentAction = action;
                return true;
            }
            return false;
         }

         // accelerates character towards left
         public virtual void DoMovementLeft()
         {
             if (currentAction != null) return;

             if (velocity.X > 0) velocity.X = 0;
             // increase velocity only if touches ground and has not reached max speed
             if (velocity.X > ( -maxSpeedX * statusModifiers.moveSpeedMod) )
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
             if (velocity.X <  ( maxSpeedX * statusModifiers.moveSpeedMod ))
                 velocity.X += acceleration;

             previousFacing = facing;
             facing = FacingDirection.Right;
         }



         public virtual int GetMeleeDamage ()
         {
            float damage = baseMeleeDamage * statusModifiers.meleeDamageMod;
            return (int)damage;
         }

         public virtual int GetRangedDamage ()
         {
            float damage = baseRangeDamage * statusModifiers.rangedDamageMod;
            return (int)damage;
        }

         public virtual int GetSpellPower ()
         {
            float damage = baseSpellPower * statusModifiers.spellPowerMod;
            return (int)damage;
        }

        // overrides
        // override gravity logic since some effects can change character falling speed
        public override void ApplyGravity(TimeSpan time)
        {
                velocity.Y += 1;

            if (velocity.Y > MAX_FALL_SPEED - statusModifiers.gravityMod)
                velocity.Y = MAX_FALL_SPEED - statusModifiers.gravityMod;

        }

    }
}
