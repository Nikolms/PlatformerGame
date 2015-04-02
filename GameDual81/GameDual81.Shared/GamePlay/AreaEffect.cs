using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    class AreaEffect : GameObject, ICollisionObject
    {
        bool isVisible;
        List<Character> CharactersHitByThis = new List<Character>();
        int damage = 0;
        float duration, elapsedTime;
        Animation animation;

        // TODO create additional constructor for animated area effects
        public AreaEffect(float duration, int damage, ObjectAlignment alignment, Rectangle hitBox) 
        {
            this.damage = damage;
            this.duration = duration;
            this.alignment = alignment;
            isVisible = true;
            InteractsWithCharacter = true;
            actualSize = hitBox;
            position.X = hitBox.X; position.Y = hitBox.Y;
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
                C.HitByAttack(damage);
            }
        }
        
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S, AnimationFiles.TextureLoader T)
        {
            if (isVisible)
                // TODO
                S.Draw(T.GetTexture("TODO"),
                    BoundingBox,
                    Color.White);
        }

        public void CheckCollisionWithCharacter(Character C)
        {
            if (BoundingBox.Intersects(C.BoundingBox))
            {
                // check that objects are different alignment and character was not already in the list
                if (C.Alignment != alignment && !CharactersHitByThis.Contains(C))
                {
                    CharactersHitByThis.Add(C);
                }
            }
        }

    }
}
