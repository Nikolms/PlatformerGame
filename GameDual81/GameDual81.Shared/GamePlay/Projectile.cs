using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.StatusEffects;

namespace ThielynGame.GamePlay
{
    public enum ProjetileType {Arrow, IceBolt, ChainSpear}

    class Projectile : PhysicsObjects, IHarmfulObject
    {
        int damage;
        StatusEffect appliedEffect;
        bool piercing;
        // this can be used if an action needs a trigger marker
        public bool collidedWithSomething {get; protected set;}

        protected Projectile() 
        {
            affectedByGravity = false;
        }

        public static Projectile createProjectile(ProjetileType type, Character actor) 
        {
            Projectile P = new Projectile();
            switch (type) 
            {
                case ProjetileType.Arrow:
                    P = new Projectile() 
                    { 
                      velocity = new Vector2(8, 0),
                      actualSize = new Rectangle(0, 0, 50, 10)
                    };
                    break;

                case ProjetileType.IceBolt:
                    P = new Projectile() 
                    { 
                        velocity = new Vector2(4.5f,0), 
                        actualSize = new Rectangle(0,0,60,60), 
                        appliedEffect = StatusEffect.createEffect(actor.level, EffectType.Fragile, 3000)
                    };
                    break;

                case ProjetileType.ChainSpear:
                    P = new ChainSpear()
                    {
                        actor = actor,
                        velocity = new Vector2(10f,0),
                        actualSize = new Rectangle(0,0,30,30)
                    };
                    break;
            }

            P.position.X = actor.BoundingBox.Center.X;
            P.position.Y = actor.BoundingBox.Y + ((actor.BoundingBox.Height - P.actualSize.Height) / 2);
            P.damage = actor.GetRangedDamage();
            P.velocity.X = P.velocity.X * (int)actor.Facing;
            P.alignment = actor.Alignment;
            return P;
        }
        

        public virtual void CheckCollisionWithCharacter(Character C)
        {
            if (C.BoundingBox.Intersects(BoundingBox) && alignment != C.Alignment)
            {
                collidedWithSomething = true;
                IsDead = true;
            }
        }

        public override void HandleObsticleCollision(CollisionDetailObject CC, Platform collidedWith)
        {
            collidedWithSomething = true;
            IsDead = true;
        }
        
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S, AnimationFiles.TextureLoader T)
        {
            S.Draw(
                T.GetTexture("TODO"),
                MyRectangle.AdjustExistingRectangle(BoundingBox), 
                Color.White);
        }
    }

    class ChainSpear : Projectile
    {
        public Character actor;

        public override void CheckCollisionWithCharacter(Character C)
        {
            if (C.BoundingBox.Intersects(BoundingBox) && C.Alignment != alignment)
            {
                C.ApplyForce(velocity.X * -2 , -3);
            }
            base.CheckCollisionWithCharacter(C);
        }

        public override void HandleObsticleCollision(CollisionDetailObject CC, Platform collidedWith)
        {
            actor.ApplyForce(velocity.X * 2, 0);
            base.HandleObsticleCollision(CC, collidedWith);
        }
    }
}
