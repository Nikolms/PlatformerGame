using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay.Actions
{
    class MeleeAttackAction : BaseAction
    {
        public MeleeAttackAction(float attackSpeed, Character actor) 
        {
            duration = attackSpeed;
            this.actor = actor;

            // melee attacks have no delay
            castTime = 0;

            textureFileName = actor.characterType + "_melee";

            string animationName = actor.characterType + "_melee_";
            if (actor.Facing == FacingDirection.Left) animationName += "left";
            if (actor.Facing == FacingDirection.Right) animationName += "right";

            animation = new Animation(AnimationLists.GetAnimation(animationName), false);
            animation.Start();
        }

        protected override void CreateEffect()
        {
            // calculates the melee attack hitbox base on half the animation frame width
            // then repositions it depending on facing
            Rectangle AttackSize = new Rectangle
                (actor.BoundingBox.X,
                actor.BoundingBox.Y,
                animation.AnimationFrameToDraw.Width / 2,
                actor.BoundingBox.Height
                );

            if (actor.Facing == FacingDirection.Left)
                AttackSize.X -= AttackSize.Width - (actor.BoundingBox.Width / 2);
            if (actor.Facing == FacingDirection.Right)
                AttackSize.X += actor.BoundingBox.Width / 2;


            AreaEffect AE = new MeleeAreaEffect(
                actor.AttackSpeed, 
                actor.GetMeleeDamage(), 
                actor.Alignment, 
                AttackSize, actor);

            LevelManager O = new LevelManager();
            O.AddGameObject(AE);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch S, TextureLoader T)
        {
            // Action frames are typically different size than the normal character fram so we must adjust positioning
            Rectangle drawLocation = new Rectangle(
                actor.BoundingBox.X,
                actor.BoundingBox.Y,
                animation.AnimationFrameToDraw.Width,
                animation.AnimationFrameToDraw.Height);

            drawLocation.X += (actor.BoundingBox.Width - animation.AnimationFrameToDraw.Width) / 2;
            drawLocation.Y += (actor.BoundingBox.Height - animation.AnimationFrameToDraw.Height);

            S.Draw(
                T.GetTexture(textureFileName),
                MyRectangle.AdjustExistingRectangle(drawLocation),
                animation.AnimationFrameToDraw,
                Color.White);
        }
    }
}
