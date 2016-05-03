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
        protected float currentDuration = 0;

        public Animation char_animation { get; protected set; }
        public Animation effect_animation { get; protected set; }
        protected Character actor;

        // random variables
        public bool canBeUsedInAir { get; protected set; }



        public CharacterAction(Character Actor)
        {
            actor = Actor;
            currentDuration = 0;
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

            // perform potential continuous effects during the action
            if (preDurationComplete)
                postExecuteUpdate();
            else preExecuteUpdate();
        }
        
        protected abstract void OnExecute();

        // these functions can be used for any constant updates this actions needs to do
        protected virtual void preExecuteUpdate() { }
        protected virtual void postExecuteUpdate() { }

        /// <summary>
        /// this function draws any special effects related to this action
        /// </summary>
        public void DrawEffect()
        {
            // do nothing if there is no special effects
            if (effect_animation == null) return;

            // TODO add drawing logic
        }
    }
}
