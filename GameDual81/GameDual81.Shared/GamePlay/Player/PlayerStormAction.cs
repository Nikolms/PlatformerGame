using System;
using System.Collections.Generic;
using System.Text;
using ThielynGame.AnimationFiles;

namespace ThielynGame.GamePlay
{
    class PlayerStormAction : CharacterAction
    {
        public PlayerStormAction(Character Actor) : base(Actor)
        {
            preDuration = 1000;
            postDuration = 100;
            totalDuration = 1100;

            char_animation = new Animation(
                AnimationLists.GetAnimationFrames("player_spell"), true);
            char_animation.Start();

            effect_animation = null;
        }

        protected override void OnExecute()
        {
            // TODO
        }
    }
}
