using Arcade_Arena.Abilites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        SpriteAnimation idleAnimation;
        SpriteAnimation meleeAttackAnimation;

        private bool inGroundSmash;
        private double groundSmashCooldown = 0;

        private bool inMeeleAttack;
        private double meeleAttackCooldown = 0;


        private bool inBodySlam;

        private Vector2 frameSize = new Vector2(23, 33);
        private Vector2 spriteDimensions = new Vector2(7, 3);


        double dashDirection;


        public Ogre(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(2, 0), new Vector2(5, 0), frameSize, spriteDimensions, 150);
            backwardsAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(0, 1), new Vector2(4, 1), frameSize, spriteDimensions, 150);
            groundSmashOgreAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(0, 2), new Vector2(3, 2), frameSize, spriteDimensions, 125);
            bodySlamAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(0, 3), new Vector2(2, 3), frameSize, spriteDimensions, 300);
            idleAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(0, 0), new Vector2(1, 0), frameSize, spriteDimensions, 1000);
            meleeAttackAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(3, 3), new Vector2(5, 3), frameSize, spriteDimensions, 125);

            groundSmashAnimation = new SpriteAnimation(AssetManager.groundSmashCrackle, new Vector2(0, 0), new Vector2(4, 0), new Vector2(71, 71), new Vector2(4, 0), 500);
            currentAnimation = backwardsAnimation;

            shadow = new Shadow(Position, AssetManager.OgreShadow, speed, direction, frameSize);

            maxHealth = 120;
            health = maxHealth;

            speed = 1;

            abilityOneMaxCooldown = 1;
            abilityTwoMaxCooldown = 5;
        }

        public override void Update( )
        {
            currentAnimation.Update();
            UpdateCooldowns();


            //kanske ändra till "actionable" debuffs sen istället för att kolla om man är i varje ability
            if (!inGroundSmash && !inBodySlam && !inMeeleAttack)
            {
                if (MouseKeyboardManager.LeftHold && meeleAttackCooldown <= 0)
                {
                    Debug.WriteLine("bruh");
                    MeeleAttack();
                    meeleAttackCooldown = 1;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.E) && groundSmashCooldown <= 0)
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

            if (inMeeleAttack)
            {
                UpdateSpriteEffect();
                base.Update();
                meleeAttackAnimation.Update();
                if(meleeAttackAnimation.XIndex >= 3)
                {
                    inMeeleAttack = false;
                    UpdateMiddleOfSprite();
                    CheckRegularAnimation();
                    aimDirection = UpdateAimDirection();                  
                }
            }
            else if (inGroundSmash)
            {
                UpdateSpriteEffect();
                groundSmashAnimation.Update();

                if (groundSmashAnimation.XIndex >= 4)
                {
                    inGroundSmash = false;
                    UpdateMiddleOfSprite();
                    CheckRegularAnimation();
                    aimDirection = UpdateAimDirection();
                }
            }
            else if (inBodySlam)
            {
                UpdateSpriteEffect();
                UpdateVelocity(dashDirection, speed * 1.5f);
                bodySlamAnimation.Update();
                UpdatePosition();

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

            shadow.Update(position);
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
                UpdateMiddleOfSprite();
                
            }
        }


        private void UpdateCooldowns( )
        {
            groundSmashCooldown -= Game1.elapsedGameTimeSeconds;
            bodySlamCooldown -= Game1.elapsedGameTimeSeconds;
            meeleAttackCooldown -= Game1.elapsedGameTimeSeconds;

            abilityOneCooldown -= Game1.elapsedGameTimeSeconds;
            abilityTwoCooldown -= Game1.elapsedGameTimeSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDead)
            {
                return;
            }

            shadow.Draw(spriteBatch);
            currentAnimation.Draw(spriteBatch, LastPosition, 0.0f, Vector2.Zero, Game1.SCALE);
            base.Draw(spriteBatch);
        }

        private void GroundSmash()
        {
            inGroundSmash = true;
            currentAnimation = groundSmashOgreAnimation;
            groundSmashAnimation.XIndex = 0;

            Ability ability = new GroundSlamAbility(this, Position, speed, direction);
            abilityBuffer.Add(ability);

        }

        private void BodySlam()
        {
            inBodySlam = true;
            currentAnimation = bodySlamAnimation;
            currentAnimation.XIndex = 0;

            dashDirection = UpdateAimDirection();

            Ability ability = new BodySlam(this, Position, speed, direction);
            abilityBuffer.Add(ability);

            
        }

        private void MeeleAttack()
        {
            inMeeleAttack = true;
            currentAnimation = meleeAttackAnimation;
            currentAnimation.XIndex = 0;

            Ability ability = new MeeleAttack(this, Position, speed, direction);
            abilityBuffer.Add(ability);
        }
    }
}