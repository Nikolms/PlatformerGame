using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    class AreaEffect : GameObject, HarmfullObject
    {
        protected bool isVisible;
        protected List<Character> CharactersHitByThis = new List<Character>();
        protected int damage = 0;
        float duration, elapsedTime;
        Animation animation;

        public AreaEffect(float duration, int damage, Character actor, Rectangle Size) 
        {
            this.duration = duration;
            this.damage = damage;
            alignment = actor.Alignment;
            actualSize = Size;
            position.X = Size.X; position.Y = Size.Y;
        }


        public override void Update(TimeSpan time) 
        {
            elapsedTime += time.Milliseconds;

            if (elapsedTime >= duration)
            {
                OnComplete();

                // this object disappears when time is out
                IsDead = true;
            }
        }

        void OnComplete() 
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
                S.Draw(T.GetTexture("TODO"),
                    MyRectangle.AdjustExistingRectangle(BoundingBox),
                    Color.White);
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
            isVisible = true; 
        }

        public override void CheckCollisionWithCharacter(Character C)
        {
            if (CharactersHitByThis.Contains(C)) return;
            CharactersHitByThis.Add(C);
        }
       
    }

    class WhirlWindArea : AreaEffect 
    {
        float interval = 1000, lastEffect = 0;
        Character caster;

        public WhirlWindArea(float duration, int damage, Character actor, Rectangle size) :
            base(duration, damage, actor, size)
        {
            isVisible = true;
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
            if (lastEffect > 0) return;

            if (BoundingBox.Intersects(C.BoundingBox))
                C.OnReceiveDamage(damage, false, null);
        }
    }
}