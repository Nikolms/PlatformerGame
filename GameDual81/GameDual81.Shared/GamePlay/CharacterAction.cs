using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    public delegate void actionDoneCallBack();

    /// <summary>
    /// An action has 2 phases, the preExecution and post execution
    /// preExecution can be used to 
    /// </summary>
    public abstract class CharacterAction
    {
        protected bool preDurationComplete;   // flag used to trigger mid action changes
        public bool actionComplete { get; protected set; }        // use this flag to check if the action has fully completed

        protected float preDuration, postDuration, totalDuration;
        float currentDuration;

        public Animation char_animation { get; protected set; }
        public Animation effect_animation { get; protected set; }
        protected Character actor;

        // random variables
        public bool canBeUsedInAir { get; protected set; }



        public CharacterAction(Character Actor)
        {
            actor = Actor;
        }



        public virtual void Update(TimeSpan time)
        {
            currentDuration += time.Milliseconds;

            if (char_animation != null)
                char_animation.CheckIfDoneAndUpdate(time);
            if (effect_animation != null)
                effect_animation.CheckIfDoneAndUpdate(time);

            if (currentDuration >= preDuration && !preDurationComplete)
            {
                preDurationComplete = true;
                OnExecute();
            }

            // flag tha action complete if it has run its full duration
            if (currentDuration >= totalDuration)
                actionComplete = true;
        }
        
        protected abstract void OnExecute();
    }
}
