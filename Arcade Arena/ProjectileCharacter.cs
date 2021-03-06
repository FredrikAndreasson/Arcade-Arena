using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcade_Arena
{
    public class ProjectileCharacter : Character
    {
        public float rotation;
        public Vector2 weaponPosition;
        protected Vector2 weaponOrigin;
        Vector2 distance;
        protected SpriteAnimation weaponAnim;
        protected SpriteAnimation weaponShootAnim;
        SpriteAnimation currentAnimation;

        protected float shootingCooldown = 1;
        float cooldownTimer;

        protected double shootingDelayTimer = 0;
        protected double shootingDelayMaxTimer = 0;

        protected bool shooting = false;

        protected int weaponOffsetX = 0;
        protected int weaponOffsetY = 0;

        public ProjectileCharacter(Vector2 position, float speed, double direction) : base(position, speed, direction) 
        {
            weaponOrigin = new Vector2(0, 0);
            cooldownTimer = 0;
            PrepareWeaponAnim();
        }

        public void UpdateWeapon(SpriteEffects fx)
        {
            UpdateMiddleOfSprite();
            currentAnimation.Update();
            if (fx == SpriteEffects.FlipHorizontally)
            {
                weaponPosition.X = middleOfSprite.X - weaponOffsetX;
                currentAnimation.SpriteFX = SpriteEffects.FlipVertically;
            }
            else
            {
                weaponPosition.X = middleOfSprite.X + weaponOffsetX;
                currentAnimation.SpriteFX = SpriteEffects.None;
            }
            weaponPosition.Y = middleOfSprite.Y + weaponOffsetY;
            
            MouseState mousePosition = Mouse.GetState();
            orbiterRotation = UpdateOrbiterRotation();

            cooldownTimer += (float)Game1.elapsedGameTimeSeconds;
            if (!shooting && cooldownTimer >= shootingCooldown && !Stunned && ((MouseKeyboardManager.LeftHold) || MouseKeyboardManager.Pressed(Buttons.RightTrigger)))
            {
                PrepareShooting();
                cooldownTimer = 0.0f;
            }
            if (shooting)
            {
                shootingDelayTimer -= Game1.elapsedGameTimeSeconds;
                if (shootingDelayTimer <= 0)
                {
                    shooting = false;
                    ChangeAnimation(ref currentAnimation, weaponAnim);
                    Shoot();
                }
            }

        }

        protected float UpdateOrbiterRotation()
        {
            float newDirection = (float)Math.Atan2(MouseKeyboardManager.MousePosition.Y - weaponPosition.Y, MouseKeyboardManager.MousePosition.X - weaponPosition.X);
            return newDirection;
        }

        protected virtual void PrepareWeaponAnim()
        {
            ChangeAnimation(ref currentAnimation, weaponAnim);
        }

        public virtual void PrepareShooting()
        {
            shootingDelayTimer = shootingDelayMaxTimer;
            shooting = true;
            ChangeAnimation(ref currentAnimation, weaponShootAnim);
        }

        public virtual void Shoot()
        {
            SpriteAnimation tempProjectile = new SpriteAnimation(AssetManager.WizardWandProjectile, Vector2.Zero, Vector2.Zero,
                new Vector2(2, 1), new Vector2(1, 1), 5000);
            Projectile projectile = new Projectile(tempProjectile, 10, 1, weaponPosition, 1, (double)orbiterRotation);
            projectile.SetPosition(weaponPosition + projectile.Velocity);
            abilityBuffer.Add(projectile);
        }

        public override void StartKnockback()
        {
            base.StartKnockback();
            CancelShooting();
        }

        protected void CancelShooting()
        {
            ChangeAnimation(ref currentAnimation, weaponAnim);
            shooting = false;
        }

        public virtual void Draw (SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, new Vector2(weaponPosition.X, weaponPosition.Y), (float)orbiterRotation, weaponOrigin, Game1.SCALE);

        }
    }
}
