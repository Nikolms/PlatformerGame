using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.GamePlay.Object_Components;

namespace ThielynGame.GamePlay
{
    class AreaEffect : GameObject, IHarmfulObject
    {
        protected bool isVisible;
        protected List<Character> CharactersHitByThis = new List<Character>();
        protected float duration, elapsedTime;
        protected Animation animation = null;
        protected string TextureSourceFile = "TODO";

        protected float intervalTime, timeSinceLastInterval;
        protected Character actor;
        protected AttackDetailObject attackDetail;

        // behaviour flags
        public bool FollowsActor { get; set; }
        public bool centerAroundActor { get; set;}
        protected bool DoesInterValEffect { get; set; }
        

        public AreaEffect(float duration, Character actor, Rectangle Size, AttackDetailObject attack) 
        {
            this.actor = actor;
            isVisible = false; ;
            this.duration = duration;
            alignment = actor.Alignment;
            actualSize = Size;
            position.X = Size.X; position.Y = Size.Y;
            attackDetail = attack;
        }

        // set visible to true if an animation is initialzed, otherwise the object is not visible
        public void SetAnimation(Animation A, string textureFile)
        {
            TextureSourceFile = textureFile;
            isVisible = true;
            animation = A;
            animation.Start();
        }
        public void SetInterval(float intervaltime)
        {
            intervalTime = intervaltime;
            timeSinceLastInterval = 0;
            DoesInterValEffect = true;
        }

        public override void Update(TimeSpan time) 
        {
            elapsedTime += time.Milliseconds;
            if (DoesInterValEffect) timeSinceLastInterval += time.Milliseconds;

            if (animation != null) animation.CheckIfDoneAndUpdate(time);


            if (elapsedTime >= duration)
            {
                OnComplete();
                // this object disappears when time is out
                IsDead = true;
            }


            if (actor.IsDead)
                IsDead = true;

            if (FollowsActor)
            {
               
                position.X = actor.BoundingBox.X;
                position.Y = actor.BoundingBox.Y;
            }

            if (centerAroundActor && FollowsActor)
            {
                position.X = actor.BoundingBox.X - ((BoundingBox.Width - actor.BoundingBox.Width) / 2);
                position.Y = actor.BoundingBox.Y - ((BoundingBox.Height - actor.BoundingBox.Height) / 2);
            }

        }

        protected virtual void OnComplete() 
        {
            // once finished deal damage to all characters that collided with this during its lifespan
            foreach (Character C in CharactersHitByThis)
            {
                C.OnReceiveAttackOrEffect(attackDetail);
            }
        }
        
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S, AnimationFiles.TextureLoader T)
        {
            if (isVisible)
                // TODO
                S.Draw(T.GetTexture(TextureSourceFile),
                    MyRectangle.AdjustExistingRectangle(BoundingBox),
                    animation.AnimationFrameToDraw, Color.White);
        }

        public virtual void CheckCollisionWithCharacter(Character C)
        {
            // check that objects are different alignment
            if (C.Alignment == alignment) return;

            // check if intervaltimer has expired for interval areas
            if (DoesInterValEffect && timeSinceLastInterval < intervalTime) return;

            if (BoundingBox.Intersects(C.BoundingBox))
            {
                // check character was not already in the list
                if (CharactersHitByThis.Contains(C)) return;

                // if intervalArea, deal damage right away
                if (DoesInterValEffect)
                    C.OnReceiveAttackOrEffect(attackDetail);
                
                // else record character in list to receive damage on completion
                else                
                    CharactersHitByThis.Add(C);
            }
        }

    }


    class ArcaneCloakArea : AreaEffect, IObsticle
    {
        public ArcaneCloakArea(float duration, Character actor, Rectangle Size, AttackDetailObject attack) 
            : base(duration, actor, Size, attack)
        {
        }

        public void CheckObsticleCollision(PhysicsObjects P)
        {
            if (P is Projectile)
                P.HandleObsticleCollision(new CollisionDetailObject(), null);
                
        }
    }


}