using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    public delegate void actionDoneCallBack();

    public abstract class CharacterAction
    {
        protected bool preDurationComplete;   // flag used to trigger mid action changes
        public bool actionComplete { get; protected set; }        // flag triggers action completion

        protected float preDuration, postDuration, totalDuration;
        float currentDuration;

        public Animation animation { get; protected set; }        // the action might use a special animation
        protected Character actor;

        public CharacterAction(Character Actor)
        {
            actor = Actor;
        }

        public virtual void Update(TimeSpan time)
        {
            currentDuration += time.Milliseconds;

            if (animation != null)
                animation.CheckIfDoneAndUpdate(time);

            if (currentDuration >= preDuration && !preDurationComplete)
            {
                preDurationComplete = true;
                OnExecute();
            }

            if (currentDuration >= totalDuration)
                actionComplete = true;
            

            if (actionComplete)
                OnComplete();
        }

        protected abstract void OnComplete();
        protected abstract void OnExecute();
    }
}
