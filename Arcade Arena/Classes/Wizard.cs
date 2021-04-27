﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Arcade_Arena.Classes
{
    class Wizard : Character
    {
        SpriteAnimation walkingAnimation;
        SpriteAnimation teleportInAnimation;
        SpriteAnimation teleportOutAnimation;
        SpriteAnimation backwardsAnimation;
        SpriteAnimation iceBlockWizardAnimation;
        SpriteAnimation iceBlockAnimation;

        private bool teleporting;
        private double teleportCooldown = 0;

        private bool inIceBlock;
        private double iceBlockCooldown = 0;

        private Vector2 newPosition;

        public Wizard(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(texture, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 3), 150);
            teleportInAnimation = new SpriteAnimation(texture, new Vector2(0, 2), new Vector2(5, 2), new Vector2(14, 20), new Vector2(7, 3), 150);
            teleportOutAnimation = new SpriteAnimation(texture, new Vector2(0, 3), new Vector2(5, 3), new Vector2(14, 20), new Vector2(7, 3), 150);
            backwardsAnimation = new SpriteAnimation(texture, new Vector2(0, 1), new Vector2(5, 1), new Vector2(14, 20), new Vector2(7, 3), 150);
            iceBlockWizardAnimation = new SpriteAnimation(texture, new Vector2(1, 2), new Vector2(1, 2), new Vector2(14, 20), new Vector2(7, 3), 1000);
            iceBlockAnimation = new SpriteAnimation(AssetManager.WizardIceBlock, new Vector2(0, 0), new Vector2(4, 0), new Vector2(14, 20), new Vector2(4, 0), 1000);

            currentAnimation = backwardsAnimation;

            speed = 1;
        }

        public override void Update(GameTime gameTime)
        {
            currentAnimation.Update(gameTime);
            UpdateCooldowns(gameTime);
            //kanske ändra till "actionable" debuffs sen istället för att kolla om man är i varje ability
            if (!teleporting && !inIceBlock)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.E) && teleportCooldown <= 0)
                {
                    TeleportAbility();
                    teleportCooldown = 6;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && iceBlockCooldown <= 0)
                {
                    IceBlockAbility();
                    iceBlockCooldown = 10;
                }
            }

            if (teleporting)
            {
                teleportOutAnimation.Update(gameTime);

                if (teleportOutAnimation.XIndex >= 4)
                {
                    teleporting = false;
                    position = newPosition;
                    middleOfSprite = new Vector2(position.X + 35, position.Y + 60);
                    aimDirection = UpdateAimDirection();
                }
            }
            else if (inIceBlock)
            {
                iceBlockAnimation.Update(gameTime);
                if ((iceBlockCooldown <= 9.7f && MouseKeyboardManager.Clicked(Keys.LeftShift)) || iceBlockAnimation.XIndex >= 4)
                {
                    inIceBlock = false;
                    middleOfSprite = new Vector2(position.X + 35, position.Y + 60);
                    aimDirection = UpdateAimDirection();
                }
            }
            else
            {
                base.Update(gameTime);
                middleOfSprite = new Vector2(position.X + 35, position.Y + 60);
            }
        }

        private void UpdateCooldowns(GameTime gameTime)
        {
            teleportCooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            iceBlockCooldown -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, 5.0f);
            if (teleporting)
            {
                teleportOutAnimation.Draw(spriteBatch, newPosition, 0.0f, Vector2.Zero, 5.0f);
            }
            else if (inIceBlock)
            {
                iceBlockAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, 5.0f);
            } else
            {
                currentAnimation = walkingAnimation;
            }
        }

        private void TeleportAbility()
        {
            teleporting = true;
            currentAnimation = teleportInAnimation;
            teleportOutAnimation.XIndex = 0;
            teleportInAnimation.XIndex = 0;

            Vector2 teleportVelocity;
            teleportVelocity.Y = (float)(Math.Sin(MathHelper.ToRadians((float)aimDirection)) * speed);
            teleportVelocity.X = (float)(Math.Cos(MathHelper.ToRadians((float)aimDirection)) * speed);

            newPosition = position + (teleportVelocity * 100);
        }

        private void IceBlockAbility()
        {
            inIceBlock = true;
            currentAnimation = iceBlockWizardAnimation;
            iceBlockAnimation.XIndex = 0;
        }
    }
}
