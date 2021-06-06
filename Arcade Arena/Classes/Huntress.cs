using Arcade_Arena.Abilites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Arcade_Arena.Classes
{
    public class Huntress : ProjectileCharacter
    {
        SpriteAnimation idleAnimation;
        SpriteAnimation handIdleAnimation;
        SpriteAnimation walkingAnimation;
        SpriteAnimation handWalkingAnimation;
        SpriteAnimation backwardsAnimation;
        SpriteAnimation handBackwardsAnimation;
        SpriteAnimation knockbackAnimation;
        SpriteAnimation handKnockbackAnimation;
        SpriteAnimation bearTrapAnimation;
        SpriteAnimation handBearTrapAnimation;
        SpriteAnimation boarAnimation;
        SpriteAnimation handBoarAnimation;
        SpriteAnimation deadAnimation;
        SpriteAnimation handDeadAnimation;

        SpriteAnimation currentHandAnimation;

        SpriteAnimation projectileAnim;

        bool doingBearTrap;

        bool doingBoar;

        List<BearTrap> bearTraps = new List<BearTrap>();
        List<Boar> boars = new List<Boar>();

        Rectangle clientBounds;

        sbyte weaponDmg = 10;
        sbyte boarDmg = 15;
        sbyte bearTrapDmg = 2;
        float shootingSpeed = 15;

        public Huntress(Vector2 position, float speed, double direction, Rectangle clientBounds) : base(position, speed, direction)
        {
            idleAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(0, 0), new Vector2(1, 0), new Vector2(14, 20), new Vector2(7, 2), 900);
            handIdleAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(0, 0), new Vector2(1, 0), new Vector2(14, 20), new Vector2(7, 2), 900);
            walkingAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 2), 130);
            handWalkingAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 2), 130);
            backwardsAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(0, 1), new Vector2(5, 1), new Vector2(14, 20), new Vector2(7, 2), 135);
            handBackwardsAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(0, 1), new Vector2(5, 1), new Vector2(14, 20), new Vector2(7, 2), 135);
            knockbackAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(6, 1), new Vector2(6, 1), new Vector2(14, 20), new Vector2(7, 2), 5000);
            handKnockbackAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(6, 1), new Vector2(6, 1), new Vector2(14, 20), new Vector2(7, 2), 5000);
            bearTrapAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(7, 1), new Vector2(7, 1), new Vector2(14, 20), new Vector2(7, 2), 5000);
            handBearTrapAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(7, 1), new Vector2(7, 1), new Vector2(14, 20), new Vector2(7, 2), 5000);
            boarAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(0, 2), new Vector2(0, 2), new Vector2(14, 20), new Vector2(7, 2), 5000);
            handBoarAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(0, 2), new Vector2(0, 2), new Vector2(14, 20), new Vector2(7, 2), 5000);
            deadAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(1, 2), new Vector2(1, 2), new Vector2(14, 20), new Vector2(7, 2), 5000);
            handDeadAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(1, 2), new Vector2(1, 2), new Vector2(14, 20), new Vector2(7, 2), 5000);
            projectileAnim = new SpriteAnimation(AssetManager.HuntressArrow, Vector2.Zero, Vector2.Zero,
                new Vector2(6, 3), new Vector2(1, 1), 5000);

            ChangeAnimation(ref currentAnimation, idleAnimation);
            ChangeAnimation(ref currentHandAnimation, handIdleAnimation);

            this.clientBounds = clientBounds;

            shadow = new Shadow(position, AssetManager.WizardShadow, speed, direction);

            baseSpeed = 1.3f;
            speed = baseSpeed;

            maxHealth = 100;
            health = maxHealth;

            abilityOneMaxCooldown = 10;
            abilityTwoMaxCooldown = 8;
        }

        protected override void PrepareWeaponAnim()
        {
            weaponOffsetX = 2;
            weaponOffsetY = 5;
            weaponAnim = new SpriteAnimation(AssetManager.HuntressLongBow, new Vector2(0, 0), new Vector2(0, 0), new Vector2(5, 7), new Vector2(3, 1), 5000);
            weaponShootAnim = new SpriteAnimation(AssetManager.HuntressLongBow, new Vector2(1, 0), new Vector2(2, 0), new Vector2(5, 7), new Vector2(3, 1), 400);
            shootingDelayMaxTimer = 0.5f;
            shootingCooldown = 2;
            weaponOrigin = new Vector2(0.5f * Game1.SCALE, 0.65f * Game1.SCALE);
            base.PrepareWeaponAnim();
        }

        public override void Update()
        {
            currentAnimation.Update();
            currentHandAnimation.Update();
            UpdateCooldowns();
            UpdateWeapon(currentAnimation.SpriteFX);
            CheckAbilityUse();
            if (doingBearTrap)
            {
                if (abilityOneCooldown <= abilityOneMaxCooldown - 1)
                {
                    ExitBearTrap();
                    CheckRegularAnimation();
                }
                UpdateEffects();
            }
            else if (doingBoar)
            {
                if (abilityTwoCooldown <= abilityTwoMaxCooldown - 1)
                {
                    ExitBoar();
                    CheckRegularAnimation();
                }
                UpdateEffects();
            }
            else
            {
                if (!Stunned)
                {
                    CheckRegularAnimation();
                }
                base.Update();
            }
            currentHandAnimation.SpriteFX = currentAnimation.SpriteFX;
            shadow.Update(Position);
        }

        private void CheckAbilityUse()
        {
            if (!doingBearTrap && !doingBoar && !Stunned)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.E) && abilityOneCooldown <= 0)
                {
                    BearTrapAbility();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && abilityTwoCooldown <= 0)
                {
                    BoarAbility();
                }
            }
        }

        public override void Shoot()
        {
            Projectile projectile = new Projectile(projectileAnim, weaponDmg, 3, Position, shootingSpeed, (double)orbiterRotation);
            projectile.SetPosition(weaponPosition + projectile.Velocity * 15 / shootingSpeed);
            abilityBuffer.Add(projectile);
        }

        private void UpdateCooldowns()
        {
            abilityOneCooldown -= Game1.elapsedGameTimeSeconds;
            abilityTwoCooldown -= Game1.elapsedGameTimeSeconds;
        }

        private void BearTrapAbility()
        {
            CancelShooting();
            doingBearTrap = true;
            ChangeAnimation(ref currentAnimation, bearTrapAnimation);
            ChangeAnimation(ref currentHandAnimation, handBearTrapAnimation);
            abilityOneCooldown = abilityOneMaxCooldown;
            UpdateSpriteEffect();
            BearTrap bearTrap = new BearTrap(this, bearTrapDmg, shadow.Position, speed, direction);
            abilityBuffer.Add(bearTrap);
        }

        private void BoarAbility()
        {
            CancelShooting();
            doingBoar = true;
            ChangeAnimation(ref currentAnimation, boarAnimation);
            ChangeAnimation(ref currentHandAnimation, handBoarAnimation);
            abilityTwoCooldown = abilityTwoMaxCooldown;
            UpdateSpriteEffect();
            Boar boar = new Boar(this, boarDmg, aimDirection, shadow.Position, 8, clientBounds);
            abilityBuffer.Add(boar);
        }

        private void ExitBoar()
        {
            doingBoar = false;
            aimDirection = UpdateAimDirection();
        }

        private void ExitBearTrap()
        {
            doingBearTrap = false;
            aimDirection = UpdateAimDirection();
        }

        protected override void ExitAnimationOnHit()
        {
            if (doingBearTrap)
            {
                ExitBearTrap();
            }
            if (doingBoar)
            {
                ExitBoar();
            }
        }

        private void CheckRegularAnimation()
        {
            if (walking)
            {
                if (WalkingBackwards())
                {
                    ChangeAnimation(ref currentAnimation, backwardsAnimation);
                    ChangeAnimation(ref currentHandAnimation, handBackwardsAnimation);
                }
                else
                {
                    ChangeAnimation(ref currentAnimation, walkingAnimation);
                    ChangeAnimation(ref currentHandAnimation, handWalkingAnimation);
                }
            }
            else
            {
                ChangeAnimation(ref currentAnimation, idleAnimation);
                ChangeAnimation(ref currentHandAnimation, handIdleAnimation);
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
            //spriteBatch.Draw(AssetManager.WizardShadow, new Vector2(Position.X + AssetManager.WizardShadow.Width / 4, Position.Y + 85), Color.Red);
            currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
            base.Draw(spriteBatch);
            currentHandAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
        }
    }
}
