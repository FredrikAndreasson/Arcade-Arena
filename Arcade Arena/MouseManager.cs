using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcade_Arena
{
    public class MouseManager
    {
        static MouseState mouseState = Mouse.GetState();
        static MouseState previousMouseState;

        public static bool leftClick { get; private set; }
        public static bool leftHold { get; private set; }
        public static bool rightClick { get; private set; }
        public static bool rightHold { get; private set; }
        public static Vector2 mousePosition { get; private set; }

        public static void Update()
        {
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
            CheckMouseClicks();
            mousePosition = mouseState.Position.ToVector2();
        }

        public static void CheckMouseClicks()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                leftHold = true;
                if (MouseManager.previousMouseState.LeftButton != ButtonState.Pressed)
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
                if (MouseManager.previousMouseState.RightButton != ButtonState.Pressed)
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
    }
}
