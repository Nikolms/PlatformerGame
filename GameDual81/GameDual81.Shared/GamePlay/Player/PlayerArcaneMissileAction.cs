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
        }

        protected override void OnExecute()
        {
            throw new NotImplementedException();
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
            IsDead = true;
        }
    }
}
