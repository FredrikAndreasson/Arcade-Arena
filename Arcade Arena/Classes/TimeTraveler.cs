using Microsoft.Xna.Framework;
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

        private int timeTravelPositionsSkipped = 0;
        private int timeTravelPosSkip = 3;
        private bool doingTimeZone = false;

        private bool doingTimeTravel = false;

        private double timeZoneTimer = 10;

        bool shootingFrenzy = false;
        float shootingDelayInFrenzy = 0.1f;

        List<TimeTravelPosition> previousPositions = new List<TimeTravelPosition>(); //för time travel

        sbyte weaponDmg = 2;
        float shootingSpeed = 5f;

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

            projectileAnim = new SpriteAnimation(AssetManager.TimeTravelerRayGunLaser, new Vector2(50, 0), new Vector2(50, 0),
                new Vector2(2, 1), new Vector2(1, 1), 5000);

            ChangeAnimation(ref currentAnimation, idleAnimation);
            ChangeAnimation(ref currentHandAnimation, handIdleAnimation);

            shadow = new Shadow(position, AssetManager.WizardShadow, speed, direction);

            maxHealth = 110;
            health = maxHealth;

            baseSpeed = 0.9f;
            speed = baseSpeed;

            abilityOneMaxCooldown = 10;
            abilityTwoMaxCooldown = 6;
        }

        

        protected override void PrepareWeaponAnim()
        {
            weaponOffsetX = 2;
            weaponOffsetY = 7;
            weaponAnim = new SpriteAnimation(AssetManager.TimeTravelerRayGun, new Vector2(0, 0), new Vector2(0, 0), new Vector2(6, 4), new Vector2(2, 1), 5000);
            weaponShootAnim = new SpriteAnimation(AssetManager.TimeTravelerRayGun, new Vector2(1, 0), new Vector2(1, 0), new Vector2(6, 4), new Vector2(2, 1), 5000);
            shootingCooldown = 0f;
            shootingDelayMaxTimer = 1f;
            weaponOrigin = new Vector2(0.4f * Game1.SCALE, 0.45f * Game1.SCALE);
            base.PrepareWeaponAnim();
        }

        public override void Update()
        {
            currentAnimation.Update();
            currentHandAnimation.Update();
            UpdateWeapon(currentAnimation.SpriteFX);
            UpdateCooldowns();
            CheckAbilityUse();
            CheckShooting();

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
                    if (abilityTwoCooldown <= abilityTwoMaxCooldown - 0.7f)//slut på ability
                    {
                        ExitTimeZone();
                        ChangeAnimation(ref currentAnimation, idleAnimation);
                        ChangeAnimation(ref currentHandAnimation, handIdleAnimation);
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
                if (abilityOneCooldown <= abilityOneMaxCooldown - 1.4f)// slut på ability
                {
                    ExitTimeTravel();
                    ChangeAnimation(ref currentAnimation, idleAnimation);
                    ChangeAnimation(ref currentHandAnimation, handIdleAnimation);
                }
            }
            shadow.Update(Position);
        }

        private void CheckShooting()
        {
            if (shootingFrenzy)
            {
                if (Stunned || !MouseKeyboardManager.LeftHold)
                {
                    shootingFrenzy = false;
                }
            }
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
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && abilityOneCooldown <= 0)
                    {
                        TimeTravelAbility();
                    }
                    else if (!doingTimeZone)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.E) && abilityTwoCooldown <= 0)
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

        public override void PrepareShooting()
        {
            base.PrepareShooting();
            if (shootingFrenzy != true)
            {
                AlterSpeedEffect speedEffect = new AlterSpeedEffect(-1.5f, shootingDelayMaxTimer, this);
            }
            else
            {
                shootingDelayTimer = shootingDelayInFrenzy;
            }
        }

        public override void Shoot()
        {
            shootingFrenzy = true;
            Projectile projectile = new Projectile(projectileAnim, weaponDmg, 3, Position, shootingSpeed, (double)orbiterRotation);
            projectile.SetPosition(weaponPosition + projectile.Velocity * 15 / shootingSpeed);
            abilityBuffer.Add(projectile);
            shootingDelayTimer = 0.08f;
        }

        protected override void Die()
        {
            base.Die();
            ChangeAnimation(ref currentAnimation, deadAnimation);
        }

        public override void StartKnockback()
        {
            base.StartKnockback();
            shooting = false;
            ChangeAnimation(ref currentAnimation, knockBackAnimation);
            ChangeAnimation(ref currentHandAnimation, handKnockBackAnimation);
        }

        private void UpdateCooldowns()
        {
            abilityOneCooldown -= Game1.elapsedGameTimeSeconds;
            abilityTwoCooldown -= Game1.elapsedGameTimeSeconds;
        }

        void TimeTravelAbility()
        {
            CancelShooting();
            RemoveAllEffects();
            doingTimeZone = false;
            doingTimeTravel = true;
            AddInvincibleEffect();
            abilityOneCooldown = abilityOneMaxCooldown;
            ChangeAnimation(ref currentAnimation, timeTravelAnimation);
            ChangeAnimation(ref currentHandAnimation, handTimeTravelAnimation);
            UpdateSpriteEffect();
        }

        void TimeZoneAbility()
        {
            CancelShooting();
            doingTimeZone = true;
            abilityTwoCooldown = abilityTwoMaxCooldown;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDead)
            {
                currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
                currentHandAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
                return;
            }

            shadow.Draw(spriteBatch);
            currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
            base.Draw(spriteBatch);
            currentHandAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
        }
    }
}
