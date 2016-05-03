using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThielynGame.AnimationFiles
{
    public class Animation
    {
        bool continuousAnimation;
        List<FrameObject> AnimationFrames;

        // returns the current Frame source rectangle
        public Rectangle AnimationFrameToDraw
        {
            get { return AnimationFrames[indexCurrentFrame].frameSource; }
        }

        int timeCurrentFrame,   // the time that the current frame has been displayed
            indexCurrentFrame,       // the index value of the frame currently being displayed
            totalNumberOfFrames;         // the total amount of frames in the animation

        public Animation(List<FrameObject> framesToAnimate, bool isContinuous) 
        {
            totalNumberOfFrames = framesToAnimate.Count();
            continuousAnimation = isContinuous;
            AnimationFrames = framesToAnimate;
        }

        /// <summary>
        /// this function sets the frame and time to 0
        /// </summary>
        public void Start() 
        {
            timeCurrentFrame = 0;
            indexCurrentFrame = 0;
        }

        public bool CheckIfDoneAndUpdate(TimeSpan time) 
        {
            // increase the duration of current Frame
            timeCurrentFrame += time.Milliseconds;

            // if enough time has passed move to next frame
            if (timeCurrentFrame >= AnimationFrames[indexCurrentFrame].durationMillisec)
            {
                indexCurrentFrame++;
                timeCurrentFrame = 0;  //reset timer on every frame switch
            }

            // check if last frame was the last frame
            if (indexCurrentFrame + 1 > totalNumberOfFrames) 
            {
                // if continuous, start from beginning
                if (continuousAnimation) indexCurrentFrame = 0;

                // otherwise stay at previous frame and return true to indicate animation has finished
                else { indexCurrentFrame--; return true; }
            }

            // returns false by default
            return false;
        }

    }
}
