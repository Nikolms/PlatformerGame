using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    class PlayerChargeAction : CharacterAction
    {
        public PlayerChargeAction(Character Actor) : base(Actor)
        {
            preDuration = 50;
            postDuration = 450;
            totalDuration = 450;

            char_animation = new Animation(AnimationLists.GetAnimationFrames("player_charge"), true);
            char_animation.Start();

            effect_animation = null;
        }

        protected override void OnExecute()
        {
            // TODO
        }

        protected override void postExecuteUpdate()
        {
            actor.AddExternalSpeed(5 * (int)actor.Facing, 0);
        }
    }

    class PlayerChargeHitBox : GameObject, IHarmfulObject
    {
        public float Damage
        {
            get; set;
        }
        List<IDestroyableObject> objectsHit = new List<IDestroyableObject>();

        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            //TODO
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
            // do nothing if object was already hit once
            if (objectsHit.Contains(D)) return;

            objectsHit.Add(D);
        }

        public void DealDamage()
        {
            foreach (IDestroyableObject D in objectsHit)
            {
                D.HitByHarmfulObject(this);
            }
        }
    }
}
