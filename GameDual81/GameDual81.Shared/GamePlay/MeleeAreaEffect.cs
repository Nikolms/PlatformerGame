using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    // MeleeArea is a special AreaEffect that needs to follow its actor
    class MeleeAreaEffect : AreaEffect
    {
        Character actor;

        public MeleeAreaEffect(float duration, int damage, ObjectAlignment alignment, Rectangle hitbox, Character actor) 
            : base (duration, damage, alignment, hitbox) 
        {
            this.actor = actor;
        }

        public override void Update(TimeSpan time)
        {
            // read actors position every frame and set those as current
            position.X = actor.BoundingBox.X;
            position.Y = actor.BoundingBox.Y;

            // then readjust position depending on facing
            if (actor.Facing == FacingDirection.Left)
                position.X -= actualSize.Width - (actor.BoundingBox.Width / 2);
            if (actor.Facing == FacingDirection.Right)
                position.X += actor.BoundingBox.Width / 2;

            base.Update(time);
        }
    }
}
