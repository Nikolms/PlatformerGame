using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay.Actions
{
    public abstract class BaseAction
    {
        protected Character actor;
        protected Animation animation;
        protected float duration, castTime, elapsedTime;
        bool castComplete;
        protected string textureFileName;
        
        public bool UpdateAndCheckIfDone(TimeSpan time) 
        {
            // set a flag to return whether the action was done or not
            bool actionComplete = false;

            elapsedTime += time.Milliseconds;
            animation.CheckIfDoneAndUpdate(time);

            if (elapsedTime >= castTime && !castComplete) 
            {
                castComplete = true;
                CreateEffect();
            }

            if (elapsedTime >= duration) 
            {
                // set the flag that actio has run its full duration
                actionComplete = true;
            }

            return actionComplete;
        }

        public abstract void Draw(SpriteBatch S, TextureLoader T);

        protected abstract  void CreateEffect();
    }
}
