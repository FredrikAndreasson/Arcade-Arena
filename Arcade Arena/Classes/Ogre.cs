﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Classes
{
    class Ogre : Character
    {
        SpriteAnimation walkingAnimation;
        SpriteAnimation backwardsAnimation;
        SpriteAnimation groundSmashOgreAnimation;
        SpriteAnimation groundSmashAnimation;
        SpriteAnimation bodySlamAnimation;


        private bool inGroundSmash;
        private double groundSmashCooldown = 0;

        private bool inBodySlam;
        private double bodySlamCooldown = 0;


        public Ogre(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(texture, new Vector2(2, 0), new Vector2(5, 0), new Vector2(23, 33), new Vector2(7, 3), 300);
            backwardsAnimation = new SpriteAnimation(texture, new Vector2(0, 1), new Vector2(4, 1), new Vector2(23, 33), new Vector2(7, 3), 150);
            groundSmashOgreAnimation = new SpriteAnimation(texture, new Vector2(0, 2), new Vector2(3, 2), new Vector2(23, 33), new Vector2(7, 3), 125);
            bodySlamAnimation = new SpriteAnimation(texture, new Vector2(0, 3), new Vector2(2, 3), new Vector2(23, 33), new Vector2(7, 3), 300);

            groundSmashAnimation = new SpriteAnimation(AssetManager.groundSmashCrackle, new Vector2(0, 0), new Vector2(4, 0), new Vector2(71, 71), new Vector2(4, 0), 500);
            currentAnimation = backwardsAnimation;

            speed = 1;
        }

        public override void Update( )
        {
            currentAnimation.Update();
            UpdateCooldowns();
            //kanske ändra till "actionable" debuffs sen istället för att kolla om man är i varje ability
            if (!inGroundSmash && !inBodySlam)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.E) && groundSmashCooldown <= 0)
                {
                    GroundSmash();
                    groundSmashCooldown = 1;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && bodySlamCooldown <= 0)
                {
                    BodySlam();
                    bodySlamCooldown = 5;
                }
            }

            if (inGroundSmash)
            {

                groundSmashAnimation.Update();

                if (groundSmashAnimation.XIndex >= 4)
                {
                    inGroundSmash = false;
                    middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
                    aimDirection = UpdateAimDirection();
                }
            }
            else if (inBodySlam)
            {
                bodySlamAnimation.Update();
                if ((bodySlamCooldown <= 2f))
                {
                    inBodySlam = false;
                    middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
                    aimDirection = UpdateAimDirection();
                }
            }
            else
            {
                base.Update();
                middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
            }
        }

        private void UpdateCooldowns( )
        {
            groundSmashCooldown -= Game1.elapsedGameTimeSeconds;
            bodySlamCooldown -= Game1.elapsedGameTimeSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (inGroundSmash)
            {
                groundSmashAnimation.Draw(spriteBatch, Position - new Vector2(71, 71), 0.0f, Vector2.Zero, 5.0f);
            }
            else if (inBodySlam)
            {
                // bodySlamAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, 5.0f);
            }
            else
            {
                currentAnimation = walkingAnimation;
            }
            currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, 5.0f);
        }

        private void GroundSmash()
        {
            inGroundSmash = true;
            currentAnimation = groundSmashOgreAnimation;
            groundSmashAnimation.XIndex = 0;
        }

        private void BodySlam()
        {
            inBodySlam = true;
            currentAnimation = bodySlamAnimation;
            currentAnimation.XIndex = 0;

        }
    }
}