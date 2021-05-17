using Arcade_Arena.Abilites;
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
        SpriteAnimation iceBlockAnimation;

        SpriteAnimation currentHandAnimation;

        private bool teleporting;
        private double teleportCooldown = 0;
        private double teleportMaxCooldown = 6;

        private bool inIceBlock;
        private double iceBlockCooldown = 0;
        private double iceBlockMaxCooldown = 10;

        private Vector2 newPosition;
        

        public Wizard(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            idleAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(0, 0), new Vector2(1, 0), new Vector2(14, 20), new Vector2(7, 3), 1000);
            handIdleAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(0, 0), new Vector2(1, 0), new Vector2(14, 20), new Vector2(7, 3), 1000);
            walkingAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 3), 150);
            handWalkingAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 3), 150);
            teleportInAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(0, 2), new Vector2(5, 2), new Vector2(14, 20), new Vector2(7, 3), 150);
            handTeleportInAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(0, 2), new Vector2(5, 2), new Vector2(14, 20), new Vector2(7, 3), 150);
            teleportOutAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(0, 3), new Vector2(5, 3), new Vector2(14, 20), new Vector2(7, 3), 150);
            handTeleportOutAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(0, 3), new Vector2(5, 3), new Vector2(14, 20), new Vector2(7, 3), 150);
            backwardsAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(0, 1), new Vector2(5, 1), new Vector2(14, 20), new Vector2(7, 3), 150);
            handBackwardsAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(0, 1), new Vector2(5, 1), new Vector2(14, 20), new Vector2(7, 3), 150);
            iceBlockWizardAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(1, 2), new Vector2(1, 2), new Vector2(14, 20), new Vector2(7, 3), 1000);
            handIceBlockAnimation = new SpriteAnimation(AssetManager.WizardHandSpriteSheet, new Vector2(1, 2), new Vector2(1, 2), new Vector2(14, 20), new Vector2(7, 3), 1000);
            iceBlockAnimation = new SpriteAnimation(AssetManager.WizardIceBlock, new Vector2(0, 0), new Vector2(4, 0), new Vector2(14, 20), new Vector2(4, 0), 1000);

            shadow = new Shadow(position, AssetManager.WizardShadow, speed, direction);

            ChangeAnimation(ref currentAnimation, idleAnimation);
            ChangeAnimation(ref currentHandAnimation, handIdleAnimation);

            speed = 1;
        }

        public bool Teleporting => teleporting;

        public override void Update()
        {
            
            currentAnimation.Update();
            UpdateCooldowns();
            UpdateWeapon();
            if (!teleporting && !inIceBlock)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.E) && teleportCooldown <= 0)
                {
                    TeleportAbility();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && iceBlockCooldown <= 0)
                {
                    IceBlockAbility();
                }
            }

            if (teleporting)
            {
                teleportOutAnimation.Update();

                if (teleportOutAnimation.XIndex >= 4)
                {
                    teleporting = false;
                    position = newPosition;
                    middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
                    aimDirection = UpdateAimDirection();
                    CheckRegularAnimation();
                }
            }
            else if (inIceBlock)
            {
                iceBlockAnimation.Update();
                if ((iceBlockCooldown <= 9.7f && MouseKeyboardManager.Clicked(Keys.LeftShift)) || iceBlockAnimation.XIndex >= 4)
                {
                    inIceBlock = false;
                    middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
                    aimDirection = UpdateAimDirection();
                    CheckRegularAnimation();
                }
            }
            else
            {
                base.Update();
                CheckRegularAnimation();
                currentHandAnimation.SpriteFX = currentAnimation.SpriteFX;
                middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
            }
            shadow.Update(Position);
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

        private void UpdateCooldowns()
        {
            teleportCooldown -= Game1.elapsedGameTimeSeconds;
            iceBlockCooldown -= Game1.elapsedGameTimeSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDead)
            {
                return;
            }

            shadow.Draw(spriteBatch);
            spriteBatch.Draw(AssetManager.WizardShadow, new Vector2(Position.X + AssetManager.WizardShadow.Width / 4, Position.Y + 85), Color.Red);
            DrawAnimations(spriteBatch);
            currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
            base.Draw(spriteBatch);
            currentHandAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
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
            teleporting = true;
            teleportCooldown = teleportMaxCooldown;
            ChangeAnimation(ref currentAnimation, teleportInAnimation);
            ChangeAnimation(ref currentHandAnimation, handTeleportInAnimation);
            teleportOutAnimation.XIndex = 0;
            teleportInAnimation.XIndex = 0;
            teleportCooldown = 6;

            Vector2 teleportVelocity;
            teleportVelocity.Y = (float)(Math.Sin(MathHelper.ToRadians((float)aimDirection)) * speed);
            teleportVelocity.X = (float)(Math.Cos(MathHelper.ToRadians((float)aimDirection)) * speed);

            newPosition = Position + (teleportVelocity * 100);

            Ability ability = new TeleportAbility(this, newPosition);
            abilityBuffer.Add(ability);
        }

        private void IceBlockAbility()
        {
            inIceBlock = true;
            iceBlockCooldown = iceBlockMaxCooldown;
            ChangeAnimation(ref currentAnimation, iceBlockWizardAnimation);
            ChangeAnimation(ref currentHandAnimation, handIceBlockAnimation);
            iceBlockAnimation.XIndex = 0;
            iceBlockCooldown = 10;


            Ability ability = new IceblockAbility(this);
            abilityBuffer.Add(ability);
        }
    }
}
