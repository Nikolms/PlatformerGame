using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.GamePlay;

namespace ThielynGame.AnimationFiles
{
    /// <summary>
    /// an object that encapsulates an animation that can be played independently even if the related objects
    /// are removed from game
    /// </summary>
    class AnimationObject : GameObject
    {
        Animation animation;
        Rectangle drawPosition;
        string TextureFileName;

        public AnimationObject(string textureName, string animationName)
        {
            // create the animation this objects is going to play
            TextureFileName = textureName;
            animation = new Animation(AnimationLists.GetAnimationFrames("animationName"), false);
            animation.Start();
        }

        public override void Update(TimeSpan time)
        {
            // get the size for next frame
            drawPosition = animation.AnimationFrameToDraw;
            
            // center the draw box around this objects actual position
            drawPosition.X = (int)position.X - drawPosition.Width / 2;
            drawPosition.Y = (int)position.Y - -drawPosition.Height / 2;

            // update the animation and flag to be removed if animation has reached its end
            if (animation.CheckIfDoneAndUpdate(time))
                IsDead = true;
        }


        public override void Draw(SpriteBatch S, TextureLoader T)
        {
            S.Draw(T.GetTexture(TextureFileName), 
                MyRectangle.AdjustExistingRectangle(BoundingBox), 
                animation.AnimationFrameToDraw,
                Color.White);
        }
    }
}
