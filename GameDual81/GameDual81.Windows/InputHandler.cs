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
    public class InputHandler
    {
        MouseState previousMouseState;
        KeyboardState previousKeyboardState;
        List<Vector2> inputLocations = new List<Vector2>();
        public List<Vector2> InputLocations { get { return inputLocations; } }

        public bool moveLeftInput { get; protected set; }
        public bool moveRightInput { get; protected set; }
        public bool jumpInput { get; protected set; }
        public bool MeleeAttackInput { get; protected set; }
        public bool RangedAttackInput { get; protected set; }
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
            RangedAttackInput = false;

            Developer_Skip = false;
            ExitGame_Input = false;


            // check keyboard and mouse
            KeyboardState keyBoardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // reset click positions everyframe
            inputLocations.Clear();

            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                inputLocations.Add(new Vector2(mouseState.X, mouseState.Y));
            }

            // request melee attack if rightMouse was pressed
            if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
                MeleeAttackInput = true;

            previousMouseState = mouseState;

            ///////////////////
            // handle keyboard section
            //////////////////
            

            // movement left if A is pressed and not D
            if (keyBoardState.IsKeyDown(Keys.A) && keyBoardState.IsKeyUp(Keys.D))
            {
                moveLeftInput = true;
            }
            // movement right if D is pressed but not D
            if (keyBoardState.IsKeyDown(Keys.D) && keyBoardState.IsKeyUp(Keys.A))
            {
                moveRightInput = true;
            }
            // Jump if W is pressed
            if (keyBoardState.IsKeyDown(Keys.W) && previousKeyboardState.IsKeyUp(Keys.W))
            {
                jumpInput = true;
            }
            // request skill slot 1 if 1 was pressed
            if (keyBoardState.IsKeyDown(Keys.D1) && previousKeyboardState.IsKeyUp(Keys.D1)) 
            {
                Developer_Skip = true;
            }
            // skill 2
            if (keyBoardState.IsKeyDown(Keys.D2) && previousKeyboardState.IsKeyUp(Keys.D2)) 
            {
                Skill_2_Input = true;
            }
            // skill 3
            if (keyBoardState.IsKeyDown(Keys.D3) && previousKeyboardState.IsKeyUp(Keys.D3))
            {
                Skill_3_Input = true;
            }
            // Exit GameScreen if ESC is pressed
            if (keyBoardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape)) 
            {
                ExitGame_Input = true;
            }
                
            previousKeyboardState = keyBoardState;

        }

    }
}
