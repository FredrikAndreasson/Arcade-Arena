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
        SpriteAnimation walkingAnimation;
        SpriteAnimation teleportInAnimation;
        SpriteAnimation teleportOutAnimation;
        SpriteAnimation backwardsAnimation;
        SpriteAnimation iceBlockWizardAnimation;
        SpriteAnimation iceBlockAnimation;

        private bool teleporting;
        private double teleportCooldown = 0;
        private double teleportMaxCooldown = 6;

        private bool inIceBlock;
        private double iceBlockCooldown = 0;
        private double iceBlockMaxCooldown = 10;

        private Vector2 newPosition;

        public Wizard(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(texture, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 3), 150);
            teleportInAnimation = new SpriteAnimation(texture, new Vector2(0, 2), new Vector2(5, 2), new Vector2(14, 20), new Vector2(7, 3), 150);
            teleportOutAnimation = new SpriteAnimation(texture, new Vector2(0, 3), new Vector2(5, 3), new Vector2(14, 20), new Vector2(7, 3), 150);
            backwardsAnimation = new SpriteAnimation(texture, new Vector2(0, 1), new Vector2(5, 1), new Vector2(14, 20), new Vector2(7, 3), 150);
            iceBlockWizardAnimation = new SpriteAnimation(texture, new Vector2(1, 2), new Vector2(1, 2), new Vector2(14, 20), new Vector2(7, 3), 1000);
            iceBlockAnimation = new SpriteAnimation(AssetManager.WizardIceBlock, new Vector2(0, 0), new Vector2(4, 0), new Vector2(14, 20), new Vector2(4, 0), 1000);

            ChangeAnimation(backwardsAnimation);

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
                    middleOfSprite = new Vector2(position.X + 35, position.Y + 60);
                    aimDirection = UpdateAimDirection();
                }
            }
            else if (inIceBlock)
            {
                iceBlockAnimation.Update();
                if ((iceBlockCooldown <= 9.7f && MouseKeyboardManager.Clicked(Keys.LeftShift)) || iceBlockAnimation.XIndex >= 4)
                {
                    inIceBlock = false;
                    middleOfSprite = new Vector2(position.X + 35, position.Y + 60);
                    aimDirection = UpdateAimDirection();
                }
            }
            else
            {
                base.Update();
                middleOfSprite = new Vector2(position.X + 35, position.Y + 60);
            }
        }

        private void UpdateCooldowns()
        {
            teleportCooldown -= Game1.elapsedGameTimeSeconds;
            iceBlockCooldown -= Game1.elapsedGameTimeSeconds;
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
                ChangeAnimation(walkingAnimation);
            }
            base.Draw(spriteBatch);
        }

        private void TeleportAbility()
        {
            teleporting = true;
            teleportCooldown = teleportMaxCooldown;
            ChangeAnimation(teleportInAnimation);
            teleportOutAnimation.XIndex = 0;
            teleportInAnimation.XIndex = 0;
            teleportCooldown = 6;

            Vector2 teleportVelocity;
            teleportVelocity.Y = (float)(Math.Sin(MathHelper.ToRadians((float)aimDirection)) * speed);
            teleportVelocity.X = (float)(Math.Cos(MathHelper.ToRadians((float)aimDirection)) * speed);

            newPosition = position + (teleportVelocity * 100);

            Ability ability = new TeleportAbility(this, newPosition);
            abilityBuffer.Add(ability);
        }

        private void IceBlockAbility()
        {
            inIceBlock = true;
            iceBlockCooldown = iceBlockMaxCooldown;
            ChangeAnimation(iceBlockWizardAnimation);
            iceBlockAnimation.XIndex = 0;
            iceBlockCooldown = 10;


            Ability ability = new IceblockAbility(this);
            abilityBuffer.Add(ability);
        }
    }
}
