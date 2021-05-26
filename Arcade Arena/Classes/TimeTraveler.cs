using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Arcade_Arena.Classes
{
    public class TimeTraveler : ProjectileCharacter
    {
        SpriteAnimation idleAnimation;
        SpriteAnimation handIdleAnimation;
        SpriteAnimation timeTravelAnimation;
        SpriteAnimation handTimeTravelAnimation;
        SpriteAnimation timeZoneAnimation;
        SpriteAnimation handTimeZoneAnimation;
        SpriteAnimation knockBackAnimation;
        SpriteAnimation handKnockBackAnimation;
        SpriteAnimation deadAnimation;

        SpriteAnimation currentHandAnimation;

        private double timeZoneCooldown = 0;
        private bool doingTimeZone = false;
        private double timeZoneMaxCooldown = 10;

        private double timeTravelCooldown = 0;
        private bool doingTimeTravel = false;
        private double timeTravelMaxCooldown = 10;

        private double timeZoneTimer = 10;

        List<TimeZone> timeZones = new List<TimeZone>();
        List<TimeTravelPosition> previousPositions = new List<TimeTravelPosition>(); //för time travel

        public TimeTraveler(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            idleAnimation = new SpriteAnimation(AssetManager.TimeTravelerSpriteSheet, new Vector2(0, 0), new Vector2(0, 0), new Vector2(14, 20), new Vector2(5, 0), 5000);
            handIdleAnimation = new SpriteAnimation(AssetManager.TimeTravelerHandSpriteSheet, new Vector2(0, 0), new Vector2(0, 0), new Vector2(14, 20), new Vector2(5, 0), 5000);
            timeTravelAnimation = new SpriteAnimation(AssetManager.TimeTravelerSpriteSheet, new Vector2(2, 0), new Vector2(3, 0), new Vector2(14, 20), new Vector2(5, 0), 200);
            handTimeTravelAnimation = new SpriteAnimation(AssetManager.TimeTravelerHandSpriteSheet, new Vector2(2, 0), new Vector2(3, 0), new Vector2(14, 20), new Vector2(5, 0), 600);
            timeZoneAnimation = new SpriteAnimation(AssetManager.TimeTravelerSpriteSheet, new Vector2(4, 0), new Vector2(4, 0), new Vector2(14, 20), new Vector2(5, 0), 5000);
            handTimeZoneAnimation = new SpriteAnimation(AssetManager.TimeTravelerHandSpriteSheet, new Vector2(4, 0), new Vector2(4, 0), new Vector2(14, 20), new Vector2(5, 0), 5000);
            knockBackAnimation = new SpriteAnimation(AssetManager.TimeTravelerSpriteSheet, new Vector2(1, 0), new Vector2(1, 0), new Vector2(14, 20), new Vector2(5, 0), 5000);
            handKnockBackAnimation = new SpriteAnimation(AssetManager.TimeTravelerHandSpriteSheet, new Vector2(1, 0), new Vector2(1, 0), new Vector2(14, 20), new Vector2(5, 0), 5000);
            deadAnimation = new SpriteAnimation(AssetManager.TimeTravelerSpriteSheet, new Vector2(5, 0), new Vector2(5, 0), new Vector2(14, 20), new Vector2(5, 0), 5000);

            ChangeAnimation(ref currentAnimation, idleAnimation);
            ChangeAnimation(ref currentHandAnimation, handIdleAnimation);

            shadow = new Shadow(position, AssetManager.WizardShadow, speed, direction);

            maxHealth = 110;
            health = maxHealth;

            speed = 1;
        }

        public override void Update()
        {
            UpdateTimeZones();
            currentAnimation.Update();
            currentHandAnimation.Update();
            UpdateCooldowns();
            CheckAbilityUse();

            if (!doingTimeTravel)
            {
                TimeTravelPosition previousPosition = new TimeTravelPosition(health, position);
                previousPositions.Add(previousPosition);
                if (previousPositions.Count > 400)
                {
                    previousPositions.RemoveAt(0);
                }

                if (doingTimeZone)
                {
                    if (timeZoneCooldown <= timeZoneMaxCooldown - 0.7f)//slut på ability
                    {
                        ExitTimeZone();
                        ChangeAnimation(ref currentAnimation, idleAnimation);
                        ChangeAnimation(ref currentHandAnimation, handIdleAnimation);
                        Debug.WriteLine("yo");
                    }
                    UpdateEffects();
                }
                else
                {
                    ChangeAnimation(ref currentAnimation, idleAnimation);
                    ChangeAnimation(ref currentHandAnimation, handIdleAnimation);
                    UpdateMiddleOfSprite();
                    base.Update();
                }
                currentHandAnimation.SpriteFX = currentAnimation.SpriteFX;
                shadow.Update(Position);
            }
            else
            {
                UpdateTimeTravelPosition();
                if (timeTravelCooldown <= timeTravelMaxCooldown - 1.4f)// slut på ability
                {
                    ExitTimeTravel();
                    ChangeAnimation(ref currentAnimation, idleAnimation);
                    ChangeAnimation(ref currentHandAnimation, handIdleAnimation);
                }
            }
        }

        private void UpdateTimeTravelPosition()
        {
            if (previousPositions.Count > 2)
            {
                position = previousPositions[previousPositions.Count - 3].Position;
                health = (sbyte)MathHelper.Max(previousPositions[previousPositions.Count - 3].Health, health);

                previousPositions.Remove(previousPositions[previousPositions.Count - 1]);
                previousPositions.Remove(previousPositions[previousPositions.Count - 1]);
                previousPositions.Remove(previousPositions[previousPositions.Count - 1]);
            }
        }

        private void CheckAbilityUse()
        {
            if (!Stunned)
            {
                if (!doingTimeTravel)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && timeTravelCooldown <= 0)
                    {
                        TimeTravelAbility();
                    }
                    else if (!doingTimeZone)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.E) && timeZoneCooldown <= 0)
                        {
                            TimeZoneAbility();
                        }
                    }
                    return;
                }
            }
        }

        protected override void UpdateMiddleOfSprite()
        {
            middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
        }

        private void ExitTimeZone()
        {
            doingTimeZone = false;
            aimDirection = UpdateAimDirection();
        }

        private void ExitTimeTravel()
        {
            doingTimeTravel = false;
            RemoveInvincibleEffect();
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

        protected override void Die()
        {
            base.Die();
            ChangeAnimation(ref currentAnimation, deadAnimation);
        }

        public override void StartKnockback()
        {
            base.StartKnockback();
            ChangeAnimation(ref currentAnimation, knockBackAnimation);
            ChangeAnimation(ref currentHandAnimation, handKnockBackAnimation);
        }

        private void UpdateCooldowns()
        {
            timeTravelCooldown -= Game1.elapsedGameTimeSeconds;
            timeZoneCooldown -= Game1.elapsedGameTimeSeconds;
        }

        void TimeTravelAbility()
        {
            doingTimeZone = false;
            doingTimeTravel = true;
            AddInvincibleEffect();
            timeTravelCooldown = timeTravelMaxCooldown;
            ChangeAnimation(ref currentAnimation, timeTravelAnimation);
            ChangeAnimation(ref currentHandAnimation, handTimeTravelAnimation);
            UpdateSpriteEffect();
        }

        void TimeZoneAbility()
        {
            doingTimeZone = true;
            timeZoneCooldown = timeZoneMaxCooldown;
            int timeZonePositionX = (int)(middleOfSprite.X - (AssetManager.TimeTravelerTimeZone.Width / 2) * Game1.SCALE);
            int timeZonePositionY = (int)(middleOfSprite.Y - (AssetManager.TimeTravelerTimeZone.Height / 2) * Game1.SCALE);
            TimeZone newTimeZone = new TimeZone(timeZoneTimer, this, new Vector2(timeZonePositionX, timeZonePositionY), AssetManager.TimeTravelerTimeZone, speed, direction);
            timeZones.Add(newTimeZone);
            ChangeAnimation(ref currentAnimation, timeZoneAnimation);
            ChangeAnimation(ref currentHandAnimation, handTimeZoneAnimation);
            UpdateSpriteEffect();
        }

        protected override void ExitAnimationOnHit()
        {
            if (doingTimeZone)
            {
                ExitTimeZone();
            }
        }

        public void RemoveTimeZoneFromList(TimeZone timeZone)
        {
            timeZones.Remove(timeZone);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (TimeZone timeZone in timeZones)
            {
                timeZone.Draw(spriteBatch);
            }
            if (isDead)
            {
                currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
                currentHandAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
                return;
            }

            shadow.Draw(spriteBatch);
            //spriteBatch.Draw(AssetManager.WizardShadow, new Vector2(Position.X + AssetManager.WizardShadow.Width / 4, Position.Y + 85), Color.Red);
            currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
            base.Draw(spriteBatch);
            currentHandAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
        }
    }
}
