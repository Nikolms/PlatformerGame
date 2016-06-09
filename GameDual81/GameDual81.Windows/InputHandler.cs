using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.GamePlay;
using ThielynGame.Screens;

namespace ThielynGame
{

    /// <summary>
    /// this class reads IO stream and sets different command flags
    /// it does not start executions of those actions but allows other components to read the flags and know if something
    /// needs to execute
    /// </summary>
    public class InputHandler
    {
        public MouseState previousMouseState;
        KeyboardState previousKeyboardState;
        public List<Vector2> TapLocation { get; protected set; } = new List<Vector2>();
        public List<Vector2> InputLocations { get; protected set; } = new List<Vector2>();
        public Vector2 MousePosition { get; protected set; }

        public bool moveLeftInput { get; protected set; }
        public bool moveRightInput { get; protected set; }
        public bool jumpInput { get; protected set; }
        public bool MeleeAttackInput { get; protected set; }
        public bool RangedAttackInput { get; protected set; }  //TODO may not be needed
        public bool SKill_1_Input { get; protected set; }
        public bool Skill_2_Input { get; protected set; }
        public bool Skill_3_Input { get; protected set; }
        public bool Skill_4_Input { get; protected set; }

        public bool Developer_Skip { get; protected set; }

        public bool ExitGame_Input { get; protected set; }

        public void Update() 
        {
            ///////////////////////////////////////////////
            // Set all input to false at start of the frame
            ///////////////////////////////////////////////

            moveLeftInput = false;
            moveRightInput = false;
            jumpInput = false;
            MeleeAttackInput = false;
            SKill_1_Input = false;
            Skill_2_Input = false;
            Skill_3_Input = false;
            Skill_4_Input = false;

            Developer_Skip = false;
            ExitGame_Input = false;


            // check keyboard and mouse
            KeyboardState keyBoardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            MousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y);

            // reset click positions everyframe
            InputLocations.Clear();
            TapLocation.Clear();

            // if a new left click is registered this frame, store the location to list as a tap
            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                TapLocation.Add(new Vector2(mouseState.X, mouseState.Y));
            }

            // if right mouse is clicked set melee input to true
            if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
            {
                MeleeAttackInput = true;
            }


            previousMouseState = mouseState;

            ///////////////////
            // handle keyboard section
            //////////////////
            

            // movement left if A is pressed and not D
            if (keyBoardState.IsKeyDown(Keys.A) && keyBoardState.IsKeyUp(Keys.D))
            {
                moveLeftInput = true;
            }
            // movement right if D is pressed but not A
            if (keyBoardState.IsKeyDown(Keys.D) && keyBoardState.IsKeyUp(Keys.A))
            {
                moveRightInput = true;
            }
            // Jump if W is pressed
            if (keyBoardState.IsKeyDown(Keys.W))
            {
                jumpInput = true;
            }
            // request skill slot 1 if 1 was pressed
            if (keyBoardState.IsKeyDown(Keys.D1) && previousKeyboardState.IsKeyUp(Keys.D1)) 
            {
                SKill_1_Input = true;
            }
            // skill 2
            if (keyBoardState.IsKeyDown(Keys.D2)) 
            {
                Skill_2_Input = true;
            }
            // skill 3
            if (keyBoardState.IsKeyDown(Keys.D3))
            {
                Skill_3_Input = true;
            }
            // Exit GameScreen if ESC is pressed
            if (keyBoardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape)) 
            {
                ExitGame_Input = true;
            }
            //
            if (keyBoardState.IsKeyDown(Keys.D4) && previousKeyboardState.IsKeyUp(Keys.D4))
            {
                Developer_Skip = true;
            }
                
            previousKeyboardState = keyBoardState;

        }

    }
}
