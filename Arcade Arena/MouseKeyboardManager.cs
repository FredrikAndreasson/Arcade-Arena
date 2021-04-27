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

        public static bool leftClick { get; private set; }
        public static bool leftHold { get; private set; }
        public static bool rightClick { get; private set; }
        public static bool rightHold { get; private set; }
        public static Vector2 mousePosition { get; private set; }

        public static void Update()
        {
            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            CheckMouseClicks();
            mousePosition = mouseState.Position.ToVector2();
        }

        public static void CheckMouseClicks()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                leftHold = true;
                if (MouseKeyboardManager.previousMouseState.LeftButton != ButtonState.Pressed)
                {
                    leftClick = true;
                }
                else
                {
                    leftClick = false;
                }
            }
            else
            {
                leftClick = false;
                leftHold = false;
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                rightHold = true;
                if (MouseKeyboardManager.previousMouseState.RightButton != ButtonState.Pressed)
                {
                    rightClick = true;
                }
                else
                {
                    rightClick = false;
                }
            }
            else
            {
                rightClick = false;
                rightHold = false;
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
