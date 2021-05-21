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
        SpriteAnimation walkingAnimation;
        SpriteAnimation backwardsAnimation;
        SpriteAnimation groundSmashOgreAnimation;
        SpriteAnimation groundSmashAnimation;
        SpriteAnimation bodySlamAnimation;


        private bool inGroundSmash;
        private double groundSmashCooldown = 0;

        private bool inBodySlam;
        private double bodySlamCooldown = 0;

        private Vector2 sheetSize = new Vector2(6, 3);
        private Vector2 frameSize = new Vector2(24, 20);



        public Knight(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(AssetManager.KnightSpriteSheet, new Vector2(0, 0), new Vector2(6, 0), frameSize, sheetSize, 300);
            backwardsAnimation = new SpriteAnimation(AssetManager.KnightSpriteSheet, new Vector2(0, 3), new Vector2(2, 3), frameSize, sheetSize, 150);
            groundSmashOgreAnimation = new SpriteAnimation(AssetManager.KnightSpriteSheet, new Vector2(0, 2), new Vector2(3, 2), frameSize, sheetSize, 125);
            bodySlamAnimation = new SpriteAnimation(AssetManager.KnightSpriteSheet, new Vector2(0, 3), new Vector2(2, 3), frameSize, sheetSize, 300);

            groundSmashAnimation = new SpriteAnimation(AssetManager.groundSmashCrackle, new Vector2(0, 0), new Vector2(4, 0), new Vector2(71, 71), new Vector2(4, 0), 500);
            currentAnimation = backwardsAnimation;

            shadow = new Shadow(position, AssetManager.WizardShadow, speed, direction);

            speed = 1;

            maxHealth = 130;
            health = maxHealth;
        }
        public override void Update()
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
                middleOfSprite = new Vector2(Position.X + 35, Position.Y + 60);
            }
                base.Update();

            shadow.Update(Position);
        }

        private void UpdateCooldowns()
        {
            groundSmashCooldown -= Game1.elapsedGameTimeSeconds;
            bodySlamCooldown -= Game1.elapsedGameTimeSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            shadow.Draw(spriteBatch);


            if (inGroundSmash)
            {
                // groundSmashAnimation.Draw(spriteBatch, Position - new Vector2(71, 71), 0.0f, Vector2.Zero, Game1.SCALE);
            }
            else if (inBodySlam)
            {
                // bodySlamAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, Game1.SCALE);
            }
            else
            {
                currentAnimation = walkingAnimation;
            }
            currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
        }

        private void GroundSmash()
        {
            inGroundSmash = true;
            currentAnimation = groundSmashOgreAnimation;
            groundSmashAnimation.XIndex = 0;

            //Ability ability = new GroundSlamAbility(this);
            //abilityBuffer.Add(ability);

        }

        private void BodySlam()
        {
            inBodySlam = true;
            currentAnimation = bodySlamAnimation;
            currentAnimation.XIndex = 0;

        }
    }



}
