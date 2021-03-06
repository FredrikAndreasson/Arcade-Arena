using Arcade_Arena.Abilites;
using Arcade_Arena.Effects;
using Arcade_Arena.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Arcade_Arena.Classes
{
    public class Wizard : ProjectileCharacter
    {
        SpriteAnimation idleAnimation;
        SpriteAnimation handIdleAnimation;
        SpriteAnimation walkingAnimation;
        SpriteAnimation handWalkingAnimation;
        SpriteAnimation teleportInAnimation;
        SpriteAnimation handTeleportInAnimation;
        SpriteAnimation teleportOutAnimation;
        SpriteAnimation handTeleportOutAnimation;
        SpriteAnimation backwardsAnimation;
        SpriteAnimation handBackwardsAnimation;
        SpriteAnimation iceBlockWizardAnimation;
        SpriteAnimation handIceBlockAnimation;
        SpriteAnimation deadAnimation;
        SpriteAnimation knockBackAnimation;
        SpriteAnimation handKnockBackAnimation;
        SpriteAnimation iceBlockAnimation;

        SpriteAnimation currentHandAnimation;

        SpriteAnimation projectileAnim;

        private bool teleporting;

        private bool inIceBlock;

        private int iceBlockHealAmount = 3;

        private Vector2 newPosition;

        private sbyte weaponDmg = 3;
        private float shootingSpeed = 7;

        Vector2 frameSize = new Vector2(14, 20);

        public Wizard(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            
            idleAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(0, 0), new Vector2(1, 0), frameSize, new Vector2(7, 3), 1000);
            handIdleAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(0, 0), new Vector2(1, 0), frameSize, new Vector2(7, 3), 1000);
            walkingAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(2, 0), new Vector2(7, 0), frameSize, new Vector2(7, 3), 150);
            handWalkingAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(2, 0), new Vector2(7, 0), frameSize, new Vector2(7, 3), 150);
            teleportInAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(0, 2), new Vector2(5, 2), frameSize, new Vector2(7, 3), 150);
            handTeleportInAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(0, 2), new Vector2(5, 2), frameSize, new Vector2(7, 3), 150);
            teleportOutAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(0, 3), new Vector2(5, 3), frameSize, new Vector2(7, 3), 150);
            handTeleportOutAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(0, 3), new Vector2(5, 3), frameSize, new Vector2(7, 3), 150);
            backwardsAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(0, 1), new Vector2(5, 1), frameSize, new Vector2(7, 3), 150);
            handBackwardsAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(0, 1), new Vector2(5, 1), frameSize, new Vector2(7, 3), 150);
            iceBlockWizardAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(1, 2), new Vector2(1, 2), frameSize, new Vector2(7, 3), 1000);
            handIceBlockAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(1, 2), new Vector2(1, 2), frameSize, new Vector2(7, 3), 1000);
            knockBackAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(6, 1), new Vector2(6, 1), frameSize, new Vector2(7, 3), 5000);
            handKnockBackAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(6, 1), new Vector2(6, 1), frameSize, new Vector2(7, 3), 5000);
            deadAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(5, 4), new Vector2(5, 4), frameSize, new Vector2(7, 3), 5000);
            iceBlockAnimation = new SpriteAnimation(AssetManager.WizardIceBlock, new Vector2(0, 0), new Vector2(4, 0), frameSize, new Vector2(4, 0), 1000);
            projectileAnim = new SpriteAnimation(AssetManager.WizardWandProjectile, Vector2.Zero, Vector2.Zero,
                new Vector2(2, 1), new Vector2(1, 1), 5000);

            shadow = new Shadow(position, AssetManager.WizardShadow, speed, direction, frameSize);

            ChangeAnimation(ref currentAnimation, idleAnimation);
            ChangeAnimation(ref currentHandAnimation, handIdleAnimation);

            maxHealth = 80;
            health = maxHealth;

            baseSpeed = 1;
            speed = baseSpeed;

            abilityOneMaxCooldown = 10;
            abilityTwoMaxCooldown = 6;
        }

        protected override void PrepareWeaponAnim()
        {
            weaponAnim = new SpriteAnimation(AssetManager.WizardWand, new Vector2(0, 0), new Vector2(0, 0), new Vector2(6, 1), new Vector2(1, 1), 5000);
            weaponShootAnim = weaponAnim;
            shootingCooldown = 1;
            weaponOffsetX = -6;
            weaponOffsetY = 23;
            base.PrepareWeaponAnim();
        }

        

        public bool Teleporting => teleporting;

        public override void Update()
        {
            currentAnimation.Update();
            currentHandAnimation.Update();
            UpdateCooldowns();
            UpdateWeapon(currentAnimation.SpriteFX);
            CheckAbilityUse();

            if (teleporting)
            {
                teleportOutAnimation.Update();

                if (teleportOutAnimation.XIndex >= 4)
                {
                    ExitTeleport(true);
                    CheckRegularAnimation();
                }
                UpdateEffects();
            }
            else if (inIceBlock)
            {
                iceBlockAnimation.Update();
                if ((abilityOneCooldown <= 9.7f && MouseKeyboardManager.Clicked(Keys.LeftShift)) || iceBlockAnimation.XIndex >= 4)
                {
                    ExitIceBlock();
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
                UpdateMiddleOfSprite();
                base.Update();
            }
            currentHandAnimation.SpriteFX = currentAnimation.SpriteFX;
            shadow.Update(Position);
        }

        private void CheckAbilityUse()
        {
            if (!teleporting && !inIceBlock & !Stunned)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.E) && abilityTwoCooldown <= 0)
                {
                    TeleportAbility();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && abilityOneCooldown <= 0)
                {
                    IceBlockAbility();
                }
            }
        }

        private void ExitTeleport(bool finished)
        {
            teleporting = false;
            if (finished)
            {
                position = newPosition;
                LastPosition = newPosition;
            }
            middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
            aimDirection = UpdateAimDirection();
        }

        public void CancelTeleportStart()
        {
            ExitTeleport(false);
            abilityTwoCooldown = 0;
        }

        private void ExitIceBlock()
        {
            inIceBlock = false;
            middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
            aimDirection = UpdateAimDirection();
            RemoveInvincibleEffect();
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

        public override void Shoot()
        {
            Projectile projectile = new Projectile(projectileAnim, weaponDmg, 3, Position, shootingSpeed, (double)orbiterRotation);
            projectile.SetPosition(weaponPosition + projectile.Velocity * 20 / shootingSpeed);
            abilityBuffer.Add(projectile);
        }

        private void UpdateCooldowns()
        {
            abilityTwoCooldown -= Game1.elapsedGameTimeSeconds;
            abilityOneCooldown -= Game1.elapsedGameTimeSeconds;
        }

        protected override void Die()
        {
            base.Die();
            ChangeAnimation(ref currentAnimation, deadAnimation);
        }

        protected override void ExitAnimationOnHit()
        {
            if (teleporting)
            {
                ExitTeleport(false);
            }
        }

        public override void StartKnockback()
        {
            base.StartKnockback();
            ChangeAnimation(ref currentAnimation, knockBackAnimation);
            ChangeAnimation(ref currentHandAnimation, handKnockBackAnimation);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDead)
            {
                currentAnimation.Draw(spriteBatch, LastPosition, 0.0f, Vector2.Zero, Game1.SCALE);
                currentHandAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
                return;
            }

            shadow.Draw(spriteBatch);
            DrawAnimations(spriteBatch);
            currentAnimation.Draw(spriteBatch, LastPosition, 0.0f, Vector2.Zero, Game1.SCALE);
            base.Draw(spriteBatch);
            currentHandAnimation.Draw(spriteBatch, LastPosition, 0.0f, Vector2.Zero, Game1.SCALE);
        }

        private void DrawAnimations(SpriteBatch spriteBatch)
        {
            if (teleporting)
            {
                teleportOutAnimation.Draw(spriteBatch, newPosition, 0.0f, Vector2.Zero, Game1.SCALE);
                handTeleportOutAnimation.Draw(spriteBatch, newPosition, 0.0f, Vector2.Zero, Game1.SCALE);
            }
            else if (inIceBlock)
            {
                iceBlockAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
            }
        }

        private void TeleportAbility()
        {
            CancelShooting();
            teleporting = true;
            abilityTwoCooldown = abilityTwoMaxCooldown;
            ChangeAnimation(ref currentAnimation, teleportInAnimation);
            ChangeAnimation(ref currentHandAnimation, handTeleportInAnimation);
            UpdateSpriteEffect();
            teleportOutAnimation.XIndex = 0;
            teleportInAnimation.XIndex = 0;
            abilityTwoCooldown = 6;

            HoTEffect hoTEffect = new HoTEffect(iceBlockHealAmount, 4, this);

            Vector2 teleportVelocity;
            teleportVelocity.Y = (float)(Math.Sin(aimDirection) * speed);
            teleportVelocity.X = (float)(Math.Cos(aimDirection) * speed);

            newPosition = Position + (teleportVelocity * 100);

            Ability ability = new TeleportAbility(this, newPosition, speed, direction);
            abilityBuffer.Add(ability);
        }

        private void IceBlockAbility()
        {
            CancelShooting();
            inIceBlock = true;
            
            abilityOneCooldown = abilityOneMaxCooldown;
            ChangeAnimation(ref currentAnimation, iceBlockWizardAnimation);
            ChangeAnimation(ref currentHandAnimation, handIceBlockAnimation);
            UpdateSpriteEffect();
            iceBlockAnimation.XIndex = 0;
            abilityOneCooldown = 10;
            AddInvincibleEffect();


            Ability ability = new IceblockAbility(this, Position, speed, direction);
            abilityBuffer.Add(ability);
        }
    }
}
