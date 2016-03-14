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

        float jumpEnergyDrainTimer = 0;
        float JumpEnergyDrainInterval = 30;

        public int AmmoLeft { get; protected set; }
        public int MaxAmmo { get; protected set; }
        public int EnergyLeft { get; protected set; }
        int MaxEnergy { get; set; }

        float energyRegenTimer = 0, energyRegenInterval = 40;
        int energyRegenValue = 1;

        int jumpPower = 4;
        float jumpAcceleration = 1.5f;

        bool isShooting;
        bool isMeleeing;

        float weaponRotation;
        Vector2 WeaponBoxOrigin = new Vector2(12, 13);
        Rectangle combatWeaponRenderBox = new Rectangle(0,0,67,23);
        Animation weaponAnimationMelee, 
            weaponAnimationRanged, currentWeaponAnimation;
        

        public Player(Vector2 startPosition, int level) : base(startPosition, level) 
        {
            alignment = ObjectAlignment.Player;
            TextureFileName = "catapultmonster1";
            characterType = "player";
            this.actualSize = new Rectangle(0,0,35,70);

            maxSpeedX = 5;
            acceleration = 0.75f;
            MeleeReach = new Rectangle(0,0,70,70);
            

            facing = FacingDirection.Right;
            MaxHealth = 100;
            armor = GameSettings.ArmorUpgrade + 1;
            ReachedEndOfLevel = false;

            MaxEnergy = 100;
            MaxAmmo = 1000;

            weaponAnimationMelee = new Animation(
                AnimationLists.GetAnimation("player_weapon_melee"), true);
            weaponAnimationRanged = new Animation(
                AnimationLists.GetAnimation("player_weapon_ranged"), true);

            setParameters();
        }

        /////////////////////////////////
        ///  overrides
        /////////////////////////////////

        protected override void setParameters()
        {
            AmmoLeft = MaxAmmo;
            EnergyLeft = MaxEnergy;
            base.setParameters();
        }

        public override void Update(TimeSpan time)
        {
            // update all cooldowns
            MeleeCoolDown += time.Milliseconds;
            RangedCoolDown += time.Milliseconds;
            jumpEnergyDrainTimer += time.Milliseconds;
            energyRegenTimer += time.Milliseconds;

            base.Update(time);

            if (currentWeaponAnimation != null)
                currentWeaponAnimation.CheckIfDoneAndUpdate(time);

            if (EnergyLeft < MaxEnergy && energyRegenTimer >= energyRegenInterval)
            {
                EnergyLeft += energyRegenValue;
                energyRegenTimer = 0;
            }
            
        }

        protected override void setStateAndFacing()
        {
            if (isShooting || isMeleeing)
                return;
            base.setStateAndFacing();
        }

        protected override void setCommonStateAnimation()
        {
            // the player combat stance overrides normal state animation
            if (characterState == CharacterState.PlayerCombat)
            {
                List<FrameObject> framelist = AnimationLists.GetAnimation("player_combat");
                animation = new Animation(framelist, true);
                characterState = CharacterState.Idle;
                return;
            }

            base.setCommonStateAnimation();
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // player attack modes require we draw the combat arm separately for rotation purposes
            // also depends on which arm is attacking in which order we draw things

            if (isMeleeing)
                S.Draw(T.GetTexture("player_meleearm"),
                      MyRectangle.AdjustExistingRectangle(combatWeaponRenderBox),
                      currentWeaponAnimation.AnimationFrameToDraw, Color.White,
                      weaponRotation, WeaponBoxOrigin, SpriteEffects.None, 0);

            base.Draw(S, T);

            if (isShooting)
                S.Draw(T.GetTexture("lightmech_weaponarm"), 
                       MyRectangle.AdjustExistingRectangle(combatWeaponRenderBox),
                       currentWeaponAnimation.AnimationFrameToDraw , Color.White, 
                       weaponRotation , WeaponBoxOrigin, SpriteEffects.None, 0);

           

            isShooting = false;
            isMeleeing = false;
        }



        ////////////////////////////
        // player UI input methods
        ///////////////////////////


        // makes the player jump
        public void DoJump() 
        {
            // so nothing if out of energy
            if (EnergyLeft <= 0) return;

                velocity.Y -= jumpAcceleration;

            if (velocity.Y < -jumpPower)
                velocity.Y = -jumpPower;

            if (jumpEnergyDrainTimer >= JumpEnergyDrainInterval)
            {
                jumpEnergyDrainTimer = 0;
                EnergyLeft -= 1;
                energyRegenTimer = 0;
            }
        }

        public bool DoRangedAttack(Vector2 targetPoint)
        {
            if (!TouchesGround || currentAction != null || isMeleeing)
                return false;

            isShooting = true;
            characterState = CharacterState.PlayerCombat;
            currentWeaponAnimation = weaponAnimationRanged;

            combatWeaponRenderBox.X = BoundingBox.X + 25; // (int)WeaponBoxOrigin.X;
            combatWeaponRenderBox.Y = BoundingBox.Y + 30; // (int)WeaponBoxOrigin.Y;

            Vector2 bulletOrigin = new Vector2(combatWeaponRenderBox.X,
                                        combatWeaponRenderBox.Y);
            Vector2 temp = targetPoint;
            if (facing == FacingDirection.Left) temp.X = 400;
            if (facing == FacingDirection.Right) temp.X = 880;

            //Vector2 direction = temp - bulletOrigin;

            //weaponRotation = (float)Math.Atan2(direction.Y, direction.X);

            // if rangedweapon is not on cooldown, create a new Bullet. Also must have ammo left
            if (RangedCoolDown >= rangedAttackSpeed && AmmoLeft > 0)
            {
                PlayerBullet bullet = new PlayerBullet(bulletOrigin, facing);
                bullet.Damage = GetRangedDamage();
                LevelManager L = new LevelManager();
                L.AddGameObject(bullet);

                RangedCoolDown = 0;
                AmmoLeft -= 1;
            }

            return true;
        }

        public bool DoMeleeAttack(Vector2 targetPoint)
        {
            if (!TouchesGround || currentAction != null || isShooting)
            return false;

            characterState = CharacterState.PlayerCombat;
            isMeleeing = true;
            currentWeaponAnimation = weaponAnimationMelee;

            combatWeaponRenderBox.X = BoundingBox.X + 25; // (int)WeaponBoxOrigin.X;
            combatWeaponRenderBox.Y = BoundingBox.Y + 30; // (int)WeaponBoxOrigin.Y;

            Vector2 weaponOrigin = new Vector2(combatWeaponRenderBox.X,
                                        combatWeaponRenderBox.Y);
            Vector2 temp = targetPoint;
            if (facing == FacingDirection.Left) temp.X = 200;
            if (facing == FacingDirection.Right) temp.X = 1080;

            Vector2 direction = temp - weaponOrigin;

            weaponRotation = (float)Math.Atan2(direction.Y, direction.X);

            float Y = (float)Math.Sin(weaponRotation) * 42;
            float X = (float)Math.Cos(weaponRotation) * 42;

            if (MeleeCoolDown >= MeleeAttackSpeed)
            {
                MeleeCoolDown = 0;
                playerMeleeHitBox M = new playerMeleeHitBox(new Vector2(
                    weaponOrigin.X + X, weaponOrigin.Y + Y));
                M.Damage = GetMeleeDamage();

                LevelManager L = new LevelManager();
                L.AddGameObject(M);
            }

            return true;
        }


        public bool PlayerActionStart()
        {
            return false;
        }

        
        
        // this function is used to reset player for the start of level
        // sets velocity to 0 and clears negative effects etc
        public void ResetPlayerStatus() 
        {
            ReachedEndOfLevel = false;
        }

        public void LevelUP()
        {
            this.level++;
        }

        public override int GetMeleeDamage()
        {
            return 20;
        }

        public override int GetRangedDamage()
        {
            return 10;
        }
    }

    ////////////////////////////////
    /// Character Support classes
    ////////////////////////////////

    class playerMeleeHitBox : GameObject, IHarmfulObject
    {
        public float Damage { get; set; }
        int timer = 60, currentTime =0;

        public playerMeleeHitBox(Vector2 Position)
        {
            actualSize = new Rectangle(0,0,20,20);

            // center the rectangle on the coordinates given
            position.X = Position.X - actualSize.Width / 2;
            position.Y = Position.Y - actualSize.Height / 2;
        }

        public override void Update(TimeSpan time)
        {
            currentTime += time.Milliseconds;

            if (currentTime >= timer)
                IsDead = true;

            base.Update(time);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // this is only a hitbox area, no graphics shown
            // TODO remove, only for testing
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(BoundingBox),
                Color.White);
        }

        public Rectangle GetBoundingBox()
        {
            return BoundingBox;
        }

        public void HitAnObject(IDestroyableObject D)
        {
            IsDead = true;
        }

        public ObjectAlignment GetAlignment()
        {
            return alignment;
        }
    }


    class PlayerBullet : PhysicsObjects, IHarmfulObject
    {
        public float Damage { get; set; }

        public PlayerBullet(Vector2 start, FacingDirection Direction)
        {
            affectedByGravity = false;
            actualSize = new Rectangle(0,0,12,4);
            position = start;
            this.alignment = ObjectAlignment.Player;

            // calculate the normalvector for direction and multiply with a speed component
            velocity = new Vector2((int)Direction * 30, 0);
        }        

        public override void HandleObsticleCollision(CollisionDetailObject CC, Platform collidedWith)
        {
            IsDead = true;
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(BoundingBox), new Rectangle(0,0,1,1),
                Color.White);
        }

        public Rectangle GetBoundingBox()
        {
            return BoundingBox;
        }
        
        public void HitAnObject(IDestroyableObject D)
        {
            IsDead = true;
        }

        public ObjectAlignment GetAlignment()
        {
            return alignment;
        }
    }
}
