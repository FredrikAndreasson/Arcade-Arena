using Arcade_Arena.Abilites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Arcade_Arena.Classes
{
    public class Huntress : ProjectileCharacter
    {
        SpriteAnimation idleAnimation;
        SpriteAnimation handIdleAnimation;
        SpriteAnimation walkingAnimation;
        SpriteAnimation handWalkingAnimation;
        SpriteAnimation backwardsAnimation;
        SpriteAnimation handBackwardsAnimation;
        SpriteAnimation knockbackAnimation;
        SpriteAnimation handKnockbackAnimation;
        SpriteAnimation bearTrapAnimation;
        SpriteAnimation handBearTrapAnimation;
        SpriteAnimation boarAnimation;
        SpriteAnimation handBoarAnimation;
        SpriteAnimation deadAnimation;
        SpriteAnimation handDeadAnimation;

        SpriteAnimation currentHandAnimation;

        bool doingBearTrap;
        double bearTrapCooldown;
        double bearTrapMaxCooldown = 10;

        bool doingBoar;
        double boarCooldown;
        double boarMaxCooldown = 10;

        List<BearTrap> bearTraps = new List<BearTrap>();
        List<Boar> boars = new List<Boar>();

        Rectangle clientBounds;

        public Huntress(Vector2 position, float speed, double direction, Rectangle clientBounds) : base(position, speed, direction)
        {
            idleAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(0, 0), new Vector2(1, 0), new Vector2(14, 20), new Vector2(7, 2), 900);
            handIdleAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(0, 0), new Vector2(1, 0), new Vector2(14, 20), new Vector2(7, 2), 900);
            walkingAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 2), 130);
            handWalkingAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 2), 130);
            backwardsAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(0, 1), new Vector2(5, 1), new Vector2(14, 20), new Vector2(7, 2), 135);
            handBackwardsAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(0, 1), new Vector2(5, 1), new Vector2(14, 20), new Vector2(7, 2), 135);
            knockbackAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(6, 1), new Vector2(6, 1), new Vector2(14, 20), new Vector2(7, 2), 5000);
            handKnockbackAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(6, 1), new Vector2(6, 1), new Vector2(14, 20), new Vector2(7, 2), 5000);
            bearTrapAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(7, 1), new Vector2(7, 1), new Vector2(14, 20), new Vector2(7, 2), 5000);
            handBearTrapAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(7, 1), new Vector2(7, 1), new Vector2(14, 20), new Vector2(7, 2), 5000);
            boarAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(0, 2), new Vector2(0, 2), new Vector2(14, 20), new Vector2(7, 2), 5000);
            handBoarAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(0, 2), new Vector2(0, 2), new Vector2(14, 20), new Vector2(7, 2), 5000);
            deadAnimation = new SpriteAnimation(AssetManager.HuntressSpriteSheet, new Vector2(1, 2), new Vector2(1, 2), new Vector2(14, 20), new Vector2(7, 2), 5000);
            handDeadAnimation = new SpriteAnimation(AssetManager.HuntressHandSpriteSheet, new Vector2(1, 2), new Vector2(1, 2), new Vector2(14, 20), new Vector2(7, 2), 5000);

            ChangeAnimation(ref currentAnimation, idleAnimation);
            ChangeAnimation(ref currentHandAnimation, handIdleAnimation);

            this.clientBounds = clientBounds;

            shadow = new Shadow(position, AssetManager.WizardShadow, speed, direction);

            speed = 1.2f;
        }

        public override void Update()
        {
            currentAnimation.Update();
            currentHandAnimation.Update();
            UpdateCooldowns();
            UpdateWeapon();
            UpdateAbilities();
            if (!doingBearTrap && !doingBoar)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.E) && bearTrapCooldown <= 0)
                {
                    BearTrapAbility();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && boarCooldown <= 0)
                {
                    BoarAbility();
                }
            }

            if (doingBearTrap)
            {
                if (bearTrapCooldown <= bearTrapMaxCooldown - 1)
                {
                    ExitBearTrap();
                    CheckRegularAnimation();
                }
            }
            else if (doingBoar)
            {
                if (boarCooldown <= boarMaxCooldown - 1)
                {
                    ExitBoar();
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
            }
            base.Update();
            currentHandAnimation.SpriteFX = currentAnimation.SpriteFX;
            shadow.Update(Position);
        }

        private void UpdateCooldowns()
        {
            bearTrapCooldown -= Game1.elapsedGameTimeSeconds;
            boarCooldown -= Game1.elapsedGameTimeSeconds;
        }

        private void UpdateAbilities()
        {
            List<BearTrap> tempBearTraps = new List<BearTrap>(bearTraps);
            foreach(BearTrap trap in tempBearTraps)
            {
                trap.Update();
            }
            List<Boar> tempBoars = new List<Boar>(boars);
            foreach (Boar boar in tempBoars)
            {
                boar.Update();
            }
        }

        private void BearTrapAbility()
        {
            bearTrapCooldown = bearTrapMaxCooldown;
            BearTrap bearTrap = new BearTrap(this, position);
            bearTraps.Add(bearTrap);
        }

        private void BoarAbility()
        {
            boarCooldown = boarMaxCooldown;
            Boar boar = new Boar(this, aimDirection, position, 1, clientBounds);
            boars.Add(boar);
        }

        private void ExitBoar()
        {
            doingBoar = false;
            aimDirection = UpdateAimDirection();
        }

        private void ExitBearTrap()
        {
            doingBearTrap = false;
            aimDirection = UpdateAimDirection();
        }

        public void DespawnBearTrap(BearTrap trap)
        {
            bearTraps.Remove(trap);
        }

        public void DespawnBoar(Boar boar)
        {
            boars.Remove(boar);
        }

        protected override void ExitAnimationOnHit()
        {
            if (doingBearTrap)
            {
                ExitBearTrap();
            }
            if (doingBoar)
            {
                ExitBoar();
            }
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawAbilities(spriteBatch);
            if (isDead)
            {
                currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
                currentHandAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
                return;
            }

            shadow.Draw(spriteBatch);
            //spriteBatch.Draw(AssetManager.WizardShadow, new Vector2(Position.X + AssetManager.WizardShadow.Width / 4, Position.Y + 85), Color.Red);
            currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
            base.Draw(spriteBatch);
            currentHandAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
        }

        private void DrawAbilities(SpriteBatch spriteBatch)
        {
            foreach (BearTrap trap in bearTraps)
            {
                trap.Draw(spriteBatch);
            }
            foreach (Boar boar in boars)
            {
                boar.Draw(spriteBatch);
            }
        }
    }
}
