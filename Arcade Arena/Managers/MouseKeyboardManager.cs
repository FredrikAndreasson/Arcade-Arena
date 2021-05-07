using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Arcade_Arena
{
    public class MouseKeyboardManager
    {
        static MouseState mouseState = Mouse.GetState();
        static MouseState previousMouseState;

        static KeyboardState keyboardState = Keyboard.GetState();
        static KeyboardState previousKeyboardState;

        public static bool LeftClick { get; private set; }
        public static bool LeftHold { get; private set; }
        public static bool RightClick { get; private set; }
        public static bool RightHold { get; private set; }
        public static Vector2 MousePosition { get; private set; }

        public static void Update()
        {
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
    }
}
