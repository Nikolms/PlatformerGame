using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    class PlayerArcaneMissileAction : CharacterAction
    {
        public PlayerArcaneMissileAction(Character Actor) : base(Actor)
        {
            preDuration = 100;
            postDuration = 100;
            totalDuration = 200;

            char_animation = new Animation(AnimationLists.GetAnimationFrames("player_melee"),true);
            char_animation.Start();

            effect_animation = null;
        }

        protected override void OnExecute()
        {
            ArcaneMissile missile = new ArcaneMissile();
            missile.Damage = actor.GetSpellDamage();
            missile.teamID = actor.teamID;
            missile.Position = actor.Position;
            missile.StartVelocity = new Vector2((int)actor.Facing * 7, 0);
            missile.affectedByGravity = false;

            LevelManager L = new LevelManager();
            L.AddGameObject(missile);
        }
    }

    class ArcaneMissile : MovableObject, IHarmfulObject
    {
        public float Damage
        {
            get; set;
        }

        public ArcaneMissile()
        {
            actualSize = new Rectangle(0,0,35,35);
        }

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            // TODO
            S.Draw(T.GetTexture("TODO"), MyRectangle.AdjustExistingRectangle(BoundingBox), Color.White);
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
            D.HitByHarmfulObject(this);
            IsDead = true;
        }

        public override void HandleObsticleCollision(CollisionDetailObject CC, Platform collidedWith)
        {
            IsDead = true;
        }
    }
}
