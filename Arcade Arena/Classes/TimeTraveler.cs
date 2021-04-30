using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Arcade_Arena.Classes
{
    class TimeTraveler : Character
    {
        SpriteAnimation walkingAnimation;

        private double timeZoneCooldown = 0;
        private bool doingTimeZone = false;

        private double timeTravelCooldown = 0;
        private bool doingTimeTravel = false;

        List<Vector2> previousPositions = new List<Vector2>(); //för time travel

        public TimeTraveler(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(texture, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 3), 150);

            currentAnimation = walkingAnimation;

            speed = 1;
        }

        public override void Update()
        {
            if (!doingTimeTravel)
            {
                previousPositions.Add(new Vector2(position.X, position.Y));
            }
            currentAnimation.Update();
            UpdateCooldowns();
            if (!doingTimeTravel && !doingTimeZone)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.E) && timeTravelCooldown <= 0)
                {
                    TimeTravelAbility();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && timeZoneCooldown <= 0)
                {
                    TimeZoneAbility();
                }
            }

            if (doingTimeTravel)
            {
                position = previousPositions[previousPositions.Count - 1];
                previousPositions.Remove(previousPositions[previousPositions.Count - 1]);
                if (true)// slut på ability
                {
                    doingTimeTravel = false;
                    aimDirection = UpdateAimDirection();
                }
            }
            else if (doingTimeZone)
            {
                if (true)//slut på ability
                {
                    doingTimeZone = false;
                    aimDirection = UpdateAimDirection();
                }
            }
            else
            {
                base.Update();
            }
        }

        private void UpdateCooldowns()
        {
            timeTravelCooldown -= Game1.elapsedGameTimeSeconds;
            timeZoneCooldown -= Game1.elapsedGameTimeSeconds;
        }

        void TimeTravelAbility()
        {
            timeTravelCooldown = 10;
        }

        void TimeZoneAbility()
        {
            timeZoneCooldown = 10;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, 5.0f);
            currentAnimation = walkingAnimation;
        }
    }
}
