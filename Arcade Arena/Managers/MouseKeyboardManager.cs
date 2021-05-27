using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Arcade_Arena
{
    public class MouseKeyboardManager
    {
        static MouseState mouseState = Mouse.GetState();
        static MouseState previousMouseState;

        static GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        static GamePadState previousGamePadState;

        static KeyboardState keyboardState = Keyboard.GetState();
        static KeyboardState previousKeyboardState;

        public static bool LeftClick { get; private set; }
        public static bool LeftHold { get; private set; }



        public static bool RightClick { get; private set; }
        public static bool RightHold { get; private set; }
        public static Vector2 MousePosition { get; private set; }

        public static void Update()
        {
            if (gamePadState.IsConnected)
            {
                previousGamePadState = gamePadState;
                gamePadState = GamePad.GetState(PlayerIndex.One);
            }
            
            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            CheckMouseClicks();
            MousePosition = mouseState.Position.ToVector2();
        }

        public static void CheckMouseClicks()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                LeftHold = true;
                if (MouseKeyboardManager.previousMouseState.LeftButton != ButtonState.Pressed)
                {
                    LeftClick = true;
                    Debug.Print("yo2");
                }
                else
                {
                    LeftClick = false;
                }
            }
            else
            {
                LeftClick = false;
                LeftHold = false;
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                RightHold = true;
                if (MouseKeyboardManager.previousMouseState.RightButton != ButtonState.Pressed)
                {
                    RightClick = true;
                }
                else
                {
                    RightClick = false;
                }
            }
            else
            {
                RightClick = false;
                RightHold = false;
            }
        }

        public static bool Clicked(Keys key)
        {
            if (keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Pressed(Buttons button)
        {
            if(gamePadState.IsButtonDown(button)) // Fixa so it will also consider IsButtonUp. Funkade inte sista jag gjorde det lol
            {
                return true;
    
            }
            else
            {
                return false;
            }

        }

        public static bool LeftThumbStickDown()
        {
            if (gamePadState.ThumbSticks.Left.Y < 0)
                return true;
            else
                return false;
        }

        public static bool LeftThumbStickUp()
        {
            if (gamePadState.ThumbSticks.Left.Y > 0)
                return true;
            else
                return false;
        }

        public static bool LeftThumbStickLeft()
        {
            if (gamePadState.ThumbSticks.Left.X < 0)
                return true;
            else
                return false;
        }

        public static bool LeftThumbStickRight()
        {
            if (gamePadState.ThumbSticks.Left.X > 0)
                return true;
            else
                return false;
        }




    }
}
