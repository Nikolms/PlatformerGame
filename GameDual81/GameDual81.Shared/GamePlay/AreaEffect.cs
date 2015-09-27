using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;
using Microsoft.Xna.Framework.Graphics;

namespace ThielynGame.GamePlay
{
    class AreaEffect : GameObject, HarmfullObject
    {
        protected bool isVisible;
        protected List<Character> CharactersHitByThis = new List<Character>();
        float duration, elapsedTime;
        Animation animation = null;
        protected string TextureSourceFile = "TODO";

        protected int damage = 0;
        Character actor;
        public bool FollowsActor { get; set; }
        

        public AreaEffect(float duration, int damage, Character actor, Rectangle Size) 
        {
            this.actor = actor;
            isVisible = false; ;
            this.duration = duration;
            this.damage = damage;
            alignment = actor.Alignment;
            actualSize = Size;
            position.X = Size.X; position.Y = Size.Y;
        }

        // set visible to true if an animation is initialzed, otherwise the object is not visible
        public void SetAnimation(Animation A, string textureFile)
        {
            TextureSourceFile = textureFile;
            isVisible = true;
            animation = A;
            animation.Start();
        }

        public override void Update(TimeSpan time) 
        {
            elapsedTime += time.Milliseconds;

            if (animation != null) animation.CheckIfDoneAndUpdate(time);

            if (elapsedTime >= duration)
            {
                OnComplete();

                // this object disappears when time is out
                IsDead = true;
            }

            if (FollowsActor)
            {
                position.X = actor.BoundingBox.X;
                position.Y = actor.BoundingBox.Y;
            }

        }

        protected virtual void OnComplete() 
        {
            // once finished deal damage to all characters that collided with this during its lifespan
            foreach (Character C in CharactersHitByThis)
            {
                C.OnReceiveDamage(damage, false, null);
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

            if (BoundingBox.Intersects(C.BoundingBox))
            {
                // check character was not already in the list
                if (CharactersHitByThis.Contains(C)) return;
                
                    CharactersHitByThis.Add(C);
            }
        }

    }

    class MeleeArea : AreaEffect 
    {
        public MeleeArea(float duration, int damage, Character actor, Rectangle size) : 
            base (duration, damage, actor, size)
        {
            // TODO set to false when no longer needed for testing
            isVisible = true; 
        }

        public override void CheckCollisionWithCharacter(Character C)
        {
            if (CharactersHitByThis.Contains(C) || C.Alignment == alignment)
                return;
            if (BoundingBox.Intersects(C.BoundingBox))
            CharactersHitByThis.Add(C);
        }

        //  TODO override not needed outside testing purposes
        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            S.Draw(T.GetTexture(TextureSourceFile),
                MyRectangle.AdjustExistingRectangle(BoundingBox),
                Color.White);
        }

    }

    class IntervalDamageArea : AreaEffect 
    {
        float interval, lastEffect = 0;
        Character caster;

        public IntervalDamageArea(int damageInterval, float duration, int damage, Character actor, Rectangle size) :
            base(duration, damage, actor, size)
        {
            interval = damageInterval;
            caster = actor;
        }

        public override void Update(TimeSpan time)
        {
            lastEffect += time.Milliseconds;

            if (lastEffect >= interval) 
            {
                lastEffect = 0;
            }

            position.X = caster.BoundingBox.X - ( (actualSize.Width - caster.BoundingBox.Width) / 2 );
            position.Y = caster.BoundingBox.Y - (actualSize.Height - caster.BoundingBox.Height);

            base.Update(time);
        }

        public override void CheckCollisionWithCharacter(Character C)
        {
            // deal damage only when interval timer has been resetB
            if (lastEffect > 0) return;

            if (BoundingBox.Intersects(C.BoundingBox) && C.Alignment != caster.Alignment)
                C.OnReceiveDamage(damage, false, null);
        }
    }
    
}