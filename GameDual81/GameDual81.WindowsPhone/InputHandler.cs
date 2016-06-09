using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.GamePlay;
using ThielynGame.Screens;

namespace ThielynGame
{
    // Windows phone version is built to read touchinput
    public class InputHandler
    {
        public List<Vector2> TapLocation { get; protected set; } = new List<Vector2>();
        public List<Vector2> InputLocations { get; protected set; } = new List<Vector2>();
        TouchCollection touchCollection;        

        public bool Developer_Skip { get; protected set; }
        public bool ExitGame_Input { get; protected set; }

        public InputHandler()
        {
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        public void Update()
        {
            // reset all input before reading new data for this frame
            Developer_Skip = false;
            ExitGame_Input = false;
            TapLocation.Clear();
            InputLocations.Clear();

            // read any tap gestures
            var gesture = default(GestureSample);
            touchCollection = TouchPanel.GetState();

            while (TouchPanel.IsGestureAvailable)
            {
                gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Tap)
                    TapLocation.Add(gesture.Position);
            }

            // read all non-gesture touch locations
            foreach (TouchLocation tl in touchCollection)
            {
                InputLocations.Add(tl.Position);
            }

            // check if back button has been pressed
            GamePadState padstate = GamePad.GetState(PlayerIndex.One);
            if (padstate.Buttons.Back == ButtonState.Pressed) ExitGame_Input = true;

        }

    }
}
