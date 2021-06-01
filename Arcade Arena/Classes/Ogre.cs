using Arcade_Arena.Abilites;
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
    class Ogre : Character
    {
        SpriteAnimation walkingAnimation;
        SpriteAnimation backwardsAnimation;
        SpriteAnimation groundSmashOgreAnimation;
        SpriteAnimation groundSmashAnimation;
        SpriteAnimation bodySlamAnimation;
        SpriteAnimation idleAnimation;
        SpriteAnimation punchAnimation;

        private bool inGroundSmash;
        private double groundSmashCooldown = 0;

        private bool inMeeleAttack;
        private double meeleAttackCooldown = 0;

        private bool inBodySlam;
        private double bodySlamCooldown = 0;

        private Vector2 frameSize = new Vector2(23, 33);
        private Vector2 spriteDimensions = new Vector2(7, 3);


        public Ogre(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(2, 0), new Vector2(5, 0), frameSize, spriteDimensions, 150);
            backwardsAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(0, 1), new Vector2(4, 1), frameSize, spriteDimensions, 150);
            groundSmashOgreAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(0, 2), new Vector2(3, 2), frameSize, spriteDimensions, 125);
            bodySlamAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(0, 3), new Vector2(2, 3), frameSize, spriteDimensions, 300);
            idleAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(0, 0), new Vector2(1, 0), frameSize, spriteDimensions, 1000);
            punchAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(3, 3), new Vector2(5, 3), frameSize, spriteDimensions, 300);

            groundSmashAnimation = new SpriteAnimation(AssetManager.groundSmashCrackle, new Vector2(0, 0), new Vector2(4, 0), new Vector2(71, 71), new Vector2(4, 0), 500);
            currentAnimation = backwardsAnimation;

            shadow = new Shadow(Position, AssetManager.OgreShadow, speed, direction);

            maxHealth = 120;
            health = maxHealth;

            speed = 1;
        }

        public override void Update( )
        {
            currentAnimation.Update();
            UpdateCooldowns();

            //kanske ändra till "actionable" debuffs sen istället för att kolla om man är i varje ability
            if (!inGroundSmash && !inBodySlam)
            {
                if (MouseKeyboardManager.LeftClick && meeleAttackCooldown <= 0)
                {
                    
                    meeleAttackCooldown = 1;
                }

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

            if (inMeeleAttack)
            {
                punchAnimation.Update();
                if(punchAnimation.XIndex >= 3)
                {
                    inMeeleAttack = false;
                    middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
                    CheckRegularAnimation();
                    aimDirection = UpdateAimDirection();
                }
            }
            else if (inGroundSmash)
            {

                groundSmashAnimation.Update();

                if (groundSmashAnimation.XIndex >= 4)
                {
                    inGroundSmash = false;
                    middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
                    CheckRegularAnimation();
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
                    CheckRegularAnimation();
                }
            }
            else
            {
                if (!Stunned)
                {
                    CheckRegularAnimation();
                }

                middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
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

            }
        }


        private void UpdateCooldowns( )
        {
            groundSmashCooldown -= Game1.elapsedGameTimeSeconds;
            bodySlamCooldown -= Game1.elapsedGameTimeSeconds;
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

        }

        private void MeeleAttack()
        {
            inMeeleAttack = true;
            currentAnimation = punchAnimation;
            currentAnimation.XIndex = 0;

            Ability ability = new MeeleAttack(this, Position, speed, direction);
            abilityBuffer.Add(ability);
        }
    }
}