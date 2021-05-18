using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Arcade_Arena.Classes
{
    public class TimeTraveler : Character
    {
        SpriteAnimation walkingAnimation;

        private double timeZoneCooldown = 0;
        private bool doingTimeZone = false;
        private double timeZoneMaxCooldown = 10;

        private double timeTravelCooldown = 0;
        private bool doingTimeTravel = false;
        private double timeTravelMaxCooldown = 10;

        private double timeZoneTimer = 10;

        List<TimeZone> timeZones = new List<TimeZone>();
        List<Vector2> previousPositions = new List<Vector2>(); //för time travel

        public TimeTraveler(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(AssetManager.TimeTravelerSpriteSheet, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 3), 150);

            currentAnimation = walkingAnimation;

            speed = 1;
        }

        public override void Update()
        {
            if (!doingTimeTravel)
            {
                previousPositions.Add(new Vector2(Position.X, Position.Y));
            }
            UpdateTimeZones();
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
                    ExitTimeTravel();
                }
            }
            else if (doingTimeZone)
            {
                if (true)//slut på ability
                {
                    ExitTimeZone();
                }
            }
            else
            {
            }
            base.Update();
        }

        private void ExitTimeZone()
        {
            doingTimeZone = false;
            aimDirection = UpdateAimDirection();
        }

        private void ExitTimeTravel()
        {
            doingTimeTravel = false;
            aimDirection = UpdateAimDirection();
        }

        private void UpdateTimeZones()
        {
            List<TimeZone> tempList = new List<TimeZone>(timeZones);
            foreach (TimeZone timeZone in tempList)
            {
                timeZone.Update();
            }
        }

        private void UpdateCooldowns()
        {
            timeTravelCooldown -= Game1.elapsedGameTimeSeconds;
            timeZoneCooldown -= Game1.elapsedGameTimeSeconds;
        }

        void TimeTravelAbility()
        {
            doingTimeTravel = true;
            timeTravelCooldown = timeTravelMaxCooldown;
        }

        void TimeZoneAbility()
        {
            doingTimeZone = true;
            timeZoneCooldown = timeZoneMaxCooldown;
            TimeZone newTimeZone = new TimeZone(timeZoneTimer, this, Position, AssetManager.TimeTravelerTimeZone);
            timeZones.Add(newTimeZone);
        }

        public void RemoveTimeZoneFromList(TimeZone timeZone)
        {
            timeZones.Remove(timeZone);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
            currentAnimation = walkingAnimation;

            foreach (TimeZone timeZone in timeZones)
            {
                timeZone.Draw(spriteBatch);
            }
        }
    }
}
