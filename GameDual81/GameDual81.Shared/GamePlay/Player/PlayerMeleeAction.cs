using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    class PlayerMeleeAction : CharacterAction
    {
        public PlayerMeleeAction(Character Actor) : base(Actor)
        {
            preDuration = 300;
            postDuration = 100;
            totalDuration = 400;

            char_animation = new Animation(AnimationLists.GetAnimationFrames("player_melee"), false);
            char_animation.Start();

            effect_animation = null;
        }


        protected override void OnExecute()
        {
            PlayerMeleeHitBox H = new PlayerMeleeHitBox() { X = 600, Y = 350};
            H.Damage = actor.GetMeleeDamage();
            H.Duration = postDuration;
            H.teamID = actor.teamID;
            

            LevelManager L = new LevelManager();
            L.AddGameObject(H);
        }
    }

    // this object is the actual attack that collides with enemies, its invisible
    class PlayerMeleeHitBox : GameObject, IHarmfulObject
    {
        public float Damage
        {
            get; set;
        }
        public float Duration;
        protected float currentTime = 0;
        IDestroyableObject objectHitByAttack = null;    // stores the first eligible Target hit by this attack

        public PlayerMeleeHitBox()
        {
            actualSize = new Rectangle(0,0,100,100);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // this object is not drawn
            // TODO REMOVE
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(BoundingBox), Color.White);
        }

        public override void Update(TimeSpan time)
        {
            currentTime += time.Milliseconds;
            if (currentTime >= Duration)
            {
                DealDamage();
                IsDead = true;
            }
        }

        public Rectangle GetBoundingBox()
        {
            return BoundingBox;
        }

        public TeamID GetTeamID()
        {
            return teamID;
        }

        public void HitAnObject(IDestroyableObject D)
        {
            if (objectHitByAttack != null) return;
            objectHitByAttack = D;
        }

        void DealDamage()
        {
            if (objectHitByAttack != null)
            objectHitByAttack.HitByHarmfulObject(this);
        }
    }
}
