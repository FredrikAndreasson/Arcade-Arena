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
    class Knight : Character
    {
        SpriteAnimation idleAnimation;
        SpriteAnimation walkingAnimation;
        SpriteAnimation backwardsAnimation;
        SpriteAnimation groundSmashOgreAnimation;
        SpriteAnimation groundSmashAnimation;
        SpriteAnimation bodySlamAnimation;


        private bool inGroundSmash;

        private bool inBodySlam;

        private Vector2 sheetSize = new Vector2(6, 3);
        private Vector2 frameSize = new Vector2(24, 20);



        public Knight(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {

            idleAnimation = new SpriteAnimation(AssetManager.KnightSpriteSheet, new Vector2(1, 2), new Vector2(2, 2), frameSize, sheetSize, 1000);

            walkingAnimation = new SpriteAnimation(AssetManager.KnightSpriteSheet, new Vector2(0, 0), new Vector2(6, 0), frameSize, sheetSize, 300);
            backwardsAnimation = new SpriteAnimation(AssetManager.KnightSpriteSheet, new Vector2(0, 3), new Vector2(2, 3), frameSize, sheetSize, 150);
            groundSmashOgreAnimation = new SpriteAnimation(AssetManager.KnightSpriteSheet, new Vector2(0, 2), new Vector2(3, 2), frameSize, sheetSize, 125);
            bodySlamAnimation = new SpriteAnimation(AssetManager.KnightSpriteSheet, new Vector2(0, 3), new Vector2(2, 3), frameSize, sheetSize, 300);

            groundSmashAnimation = new SpriteAnimation(AssetManager.groundSmashCrackle, new Vector2(0, 0), new Vector2(4, 0), new Vector2(71, 71), new Vector2(4, 0), 500);
            currentAnimation = backwardsAnimation;

            shadow = new Shadow(position, AssetManager.WizardShadow, speed, direction, frameSize);

            ChangeAnimation(ref currentAnimation, idleAnimation);

            speed = 1;
            maxHealth = 100;

            health = maxHealth;

            abilityOneMaxCooldown = 1;
            abilityTwoMaxCooldown = 5;
        }
        public override void Update()
        {
            currentAnimation.Update();
            UpdateCooldowns();
            CheckAbilityUse();

            if (inGroundSmash)
            {

                groundSmashAnimation.Update();

                if (groundSmashAnimation.XIndex >= 4)
                {
                    inGroundSmash = false;
                    UpdateMiddleOfSprite();
                    aimDirection = UpdateAimDirection();
                    CheckRegularAnimation();
                }
            }
            else if (inBodySlam)
            {
                bodySlamAnimation.Update();
                if ((abilityTwoCooldown <= 2f))
                {
                    inBodySlam = false;
                    UpdateMiddleOfSprite();
                    aimDirection = UpdateAimDirection();
                    CheckRegularAnimation();
                }
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

            shadow.Update(Position);
        }

        private void CheckAbilityUse()
        {
            if (!inGroundSmash && !inBodySlam && !Stunned)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.E) && abilityOneCooldown <= 0)
                {
                    GroundSmash();
                    abilityOneCooldown = 1;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && abilityTwoCooldown <= 0)
                {
                    BodySlam();
                    abilityTwoCooldown = 5;
                }
            }
        }

        private void CheckRegularAnimation()
        {
            if (walking)
            {
                if (WalkingBackwards())
                {
                    ChangeAnimation(ref currentAnimation, backwardsAnimation);
                }
                else
                {
                    ChangeAnimation(ref currentAnimation, walkingAnimation);
                }
            }
            else
            {
                ChangeAnimation(ref currentAnimation, idleAnimation);
            }
        }

        private void UpdateCooldowns()
        {
            abilityOneCooldown -= Game1.elapsedGameTimeSeconds;
            abilityTwoCooldown -= Game1.elapsedGameTimeSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDead)
            {
                DrawAnimations(spriteBatch);
                return;
            }

            shadow.Draw(spriteBatch);
            DrawAnimations(spriteBatch);
            currentAnimation.Draw(spriteBatch, LastPosition, 0.0f, Vector2.Zero, Game1.SCALE);
            base.Draw(spriteBatch);
        }

        private void DrawAnimations(SpriteBatch spriteBatch)
        {
            if (inGroundSmash)
            {

            }
            else if (inBodySlam)
            {
               
            }
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
