using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay.StatusEffects;

namespace ThielynGame.GamePlay
{
    public enum ProjetileType {Arrow, IceBolt}

    class Projectile : PhysicsObjects, HarmfullObject
    {
        int damage;
        StatusEffect appliedEffect;
        bool piercing;

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
                      velocity = new Vector2(6.5f, 0),
                      actualSize = new Rectangle(0, 0, 50, 10)
                    };
                    break;

                case ProjetileType.IceBolt:
                    P = new Projectile() 
                    { 
                        velocity = new Vector2(4f,0), 
                        actualSize = new Rectangle(0,0,60,60), 
                        appliedEffect = StatusEffect.createEffect(actor.level, EffectType.Fragile)
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
        

        public void CheckCollisionWithCharacter(Character C)
        {
            if (C.BoundingBox.Intersects(BoundingBox) && alignment != C.Alignment)
            {
                C.OnReceiveDamage(damage, false, appliedEffect);
                IsDead = true;
            }
        }

        public override void HandleGroundCollision(collisionCorrection CC, Platform collidedWith)
        {
            IsDead = true;
        }

        public void CollideWithObsticle() 
        {
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
}
