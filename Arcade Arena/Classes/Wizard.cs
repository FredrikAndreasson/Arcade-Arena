using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Classes
{
    public class Wizard : Character
    {
        SpriteAnimation currentAnimation;
        SpriteAnimation walkingAnimation;
        SpriteAnimation teleportInAnimation;
        SpriteAnimation teleportOutAnimation;
        SpriteAnimation backwardsAnimation;

        private bool teleporting;
        private double teleportCooldown = 0;

        private Vector2 newPosition;

        public Wizard(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(texture, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 3), 150);
            teleportInAnimation = new SpriteAnimation(texture, new Vector2(0, 2), new Vector2(5, 2), new Vector2(14, 20), new Vector2(7, 3), 150);
            teleportOutAnimation = new SpriteAnimation(texture, new Vector2(0, 3), new Vector2(5, 3), new Vector2(14, 20), new Vector2(7, 3), 150);
            backwardsAnimation = new SpriteAnimation(texture, new Vector2(0, 1), new Vector2(5, 1), new Vector2(14, 20), new Vector2(7, 3), 150);



            currentAnimation = backwardsAnimation;

            speed = 1;
        }

        public override void Update(GameTime gameTime)
        {
            currentAnimation.Update(gameTime);
            UpdateCooldowns(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.E) && !teleporting && teleportCooldown <= 0)
            {
                TeleportAbility();
                teleportCooldown = 6;
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
            else
            {
                base.Update(gameTime);
                middleOfSprite = new Vector2(position.X + 35, position.Y + 60);
            }
        }

        private void UpdateCooldowns(GameTime gameTime)
        {
            teleportCooldown -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, 5.0f);
            if (teleporting)
            {
                
                teleportOutAnimation.Draw(spriteBatch, newPosition, 0.0f, Vector2.Zero, 5.0f);
            }
            else
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
    }
}
