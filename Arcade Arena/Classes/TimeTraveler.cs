﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Arcade_Arena.Effects;

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

        SpriteAnimation projectileAnim;

        private double timeZoneCooldown = 0;
        private int timeTravelPositionsSkipped = 0;
        private int timeTravelPosSkip = 3;
        private bool doingTimeZone = false;
        private double timeZoneMaxCooldown = 10;

        private double timeTravelCooldown = 0;
        private bool doingTimeTravel = false;
        private double timeTravelMaxCooldown = 10;

        private double timeZoneTimer = 10;

        List<TimeZone> timeZones = new List<TimeZone>();
        List<TimeTravelPosition> previousPositions = new List<TimeTravelPosition>(); //för time travel

        sbyte weaponDmg = 15;
        float shootingSpeed = 0.2f;

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

            projectileAnim = new SpriteAnimation(AssetManager.TimeTravelerRayGunLaser, Vector2.Zero, Vector2.Zero,
                new Vector2(100, 1), new Vector2(1, 1), 5000);

            ChangeAnimation(ref currentAnimation, idleAnimation);
            ChangeAnimation(ref currentHandAnimation, handIdleAnimation);

            shadow = new Shadow(position, AssetManager.WizardShadow, speed, direction);

            maxHealth = 110;
            health = maxHealth;

            baseSpeed = 0.9f;
            speed = baseSpeed;
        }

        protected override void PrepareWeaponAnim()
        {
            weaponOffsetX = 2;
            weaponOffsetY = 7;
            weaponAnim = new SpriteAnimation(AssetManager.TimeTravelerRayGun, new Vector2(0, 0), new Vector2(0, 0), new Vector2(6, 4), new Vector2(2, 1), 5000);
            weaponShootAnim = new SpriteAnimation(AssetManager.TimeTravelerRayGun, new Vector2(1, 0), new Vector2(1, 0), new Vector2(6, 4), new Vector2(2, 1), 5000);
            shootingCooldown = 2;
            shootingDelayMaxTimer = 0.8;
            weaponOrigin = new Vector2(0.4f * Game1.SCALE, 0.45f * Game1.SCALE);
            base.PrepareWeaponAnim();
        }

        public override void Update()
        {
            UpdateTimeZones();
            currentAnimation.Update();
            currentHandAnimation.Update();
            UpdateWeapon(currentAnimation.SpriteFX);
            UpdateCooldowns();
            CheckAbilityUse();

            if (!doingTimeTravel)
            {
                timeTravelPositionsSkipped++;
                if (timeTravelPositionsSkipped >= timeTravelPosSkip)
                {
                    timeTravelPositionsSkipped = 0;
                    if (previousPositions.Count != 0)
                    {
                        previousPositions[previousPositions.Count - 1].Health = (sbyte)MathHelper.Max(previousPositions[previousPositions.Count - 1].Health, health);
                    }
                    TimeTravelPosition previousPosition = new TimeTravelPosition(health, position);
                    previousPositions.Add(previousPosition);
                }
                if (previousPositions.Count > 150)
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
                    if (!Stunned)
                    {
                        ChangeAnimation(ref currentAnimation, idleAnimation);
                        ChangeAnimation(ref currentHandAnimation, handIdleAnimation);
                    }
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
            shadow.Update(Position);
        }

        private void UpdateTimeTravelPosition()
        {
            if (previousPositions.Count > 2)
            {
                position = previousPositions[previousPositions.Count - 1].Position;
                health = (sbyte)MathHelper.Max(previousPositions[previousPositions.Count - 1].Health, health);

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

        private void ExitTimeZone()
        {
            doingTimeZone = false;
            aimDirection = UpdateAimDirection();
        }

        private void ExitTimeTravel()
        {
            doingTimeTravel = false;
            LastPosition = position;
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

        public override void PrepareShooting()
        {
            base.PrepareShooting();
            AlterSpeedEffect speedEffect = new AlterSpeedEffect(-1.5f, shootingDelayMaxTimer, this);
        }

        public override void Shoot()
        {
            Projectile projectile = new Projectile(projectileAnim, weaponDmg, 3, Position, shootingSpeed, (double)orbiterRotation);
            projectile.SetPosition(weaponPosition + projectile.Velocity * 100);
            abilityBuffer.Add(projectile);
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
            CancelShooting();
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
            CancelShooting();
            doingTimeZone = true;
            timeZoneCooldown = timeZoneMaxCooldown;
            int timeZonePositionX = (int)(middleOfSprite.X - (AssetManager.TimeTravelerTimeZone.Width / 2) * Game1.SCALE);
            int timeZonePositionY = (int)(middleOfSprite.Y - (AssetManager.TimeTravelerTimeZone.Height / 2) * Game1.SCALE);
            TimeZone newTimeZone = new TimeZone(timeZoneTimer, this, new Vector2(timeZonePositionX, timeZonePositionY), AssetManager.TimeTravelerTimeZone, speed, direction);
            abilityBuffer.Add(newTimeZone);
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
