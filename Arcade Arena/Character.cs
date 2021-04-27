using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcade_Arena
{
    class Character : DynamicObject
    {
        protected SpriteAnimation currentAnimation;

        protected int weaponLvl; //pay for lvls or get powerups?
        protected bool walking = false;
        protected bool canWalk = true;

        protected int health;
        protected int mana;

        protected double aimDirection;

        protected Vector2 middleOfSprite;

        public Character(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {

        }

        public SpriteAnimation CurrentAnimation => currentAnimation;

        public virtual void Update(GameTime gameTime)
        {
            direction = UpdateMovementDirection();
            aimDirection = UpdateAimDirection();
            if (walking)
            {
                velocity.Y = (float)(Math.Sin(MathHelper.ToRadians((float)direction)) * speed);
                velocity.X = (float)(Math.Cos(MathHelper.ToRadians((float)direction)) * speed);
                position += velocity;
            }
        }

        //returnerar angle i grader
        protected double UpdateMovementDirection()
        {
            walking = false;
            double newDirection = direction;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                walking = true;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    newDirection = 315;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    newDirection = 225;
                }
                else
                {
                    newDirection = 270;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                walking = true;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    newDirection = 45;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    newDirection = 135;
                }
                else
                {
                    newDirection = 90;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                walking = true;
                {
                    newDirection = 0;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                walking = true;
                {
                    newDirection = 180;
                }
            }

            return newDirection;
        }

        //returnerar aim angle i grader
        protected double UpdateAimDirection()
        {
            double newDirection = MathHelper.ToDegrees((float)Math.Atan2(MouseKeyboardManager.mousePosition.Y - middleOfSprite.Y, MouseKeyboardManager.mousePosition.X - middleOfSprite.X));
            return newDirection;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(AssetManager.TargetDummy, position, Color.White);
        }
    }
}
