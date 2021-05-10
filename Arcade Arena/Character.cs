using System;
using System.Collections.Generic;
using Arcade_Arena.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcade_Arena
{
    public class Character : DynamicObject
    {
        protected SpriteAnimation currentAnimation;

        protected int weaponLvl; //pay for lvls or get powerups?
        protected bool walking = false;
        protected bool canWalk = true;

        protected int health;
        protected int mana;
        protected bool invincible = false;


        protected double aimDirection;

        protected Vector2 middleOfSprite;

        public List<Effect> EffectList = new List<Effect>();

        public Shadow shadow;

        public List<Ability> abilityBuffer;

        public Character(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            abilityBuffer = new List<Ability>();
            IntersectingLava = false;
            health = 100;
        }

        public SpriteAnimation CurrentAnimation => currentAnimation;

        public bool IntersectingLava { get; set; }

        public virtual void Update()
        {
            UpdateEffects();
            direction = UpdateMovementDirection();
            aimDirection = UpdateAimDirection();
            if (walking && canWalk)
            {
                UpdateVelocity(direction, speed);
            }
        }

        public void UpdateVelocity(double newDirection, float newSpeed)
        {
            velocity.Y = (float)(Math.Sin(MathHelper.ToRadians((float)newDirection)) * newSpeed * SpeedAlteration);
            velocity.X = (float)(Math.Cos(MathHelper.ToRadians((float)newDirection)) * newSpeed * SpeedAlteration);
            position += velocity;
        }

        public void TakeDamage(int damage)
        {
            if (!invincible)
            {
                health -= damage;
            }
        }

        public virtual void StartKnockback() //behöver en egen metod pga sprite sheet
        {
            canWalk = false;
            walking = false;
        }

        public virtual void EndKnockback()
        {
            canWalk = true;
            walking = true;
        }

        //returnerar angle i grader
        protected double UpdateMovementDirection()
        {
            walking = false;
            double newDirection = direction;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                walking = true;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    newDirection = 315;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    newDirection = 225;
                }
                else
                {
                    newDirection = 270;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                walking = true;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    newDirection = 45;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    newDirection = 135;
                }
                else
                {
                    newDirection = 90;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                walking = true;
                {
                    newDirection = 0;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                walking = true;
                {
                    newDirection = 180;
                }
            }

            return newDirection;
        }

        //returnerar aim angle i grader
        protected double UpdateAimDirection()
        {
            double newDirection = MathHelper.ToDegrees((float)Math.Atan2(MouseKeyboardManager.MousePosition.Y - middleOfSprite.Y, MouseKeyboardManager.MousePosition.X - middleOfSprite.X));
            return newDirection;
        }

        protected void ChangeAnimation(SpriteAnimation newAnimation)
        {
            currentAnimation = newAnimation;
            currentAnimation.StartAnimation();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public void CheckLavaCollision(Lava lava)
        {
            Color[] pixels = new Color[shadow.texture.Width * shadow.texture.Height];
            Color[] pixels2 = new Color[shadow.texture.Width * shadow.texture.Height];
            //shadow.texture.GetData<Color>(0, new Rectangle(shadow.position.ToPoint(), new Point(shadow.texture.Width, shadow.texture.Height)), pixels2, 0, pixels2.Length);
            shadow.texture.GetData<Color>(pixels2);
            lava.renderTarget.GetData(0, new Rectangle(shadow.Position.ToPoint(), new Point(shadow.texture.Width, shadow.texture.Height)), pixels, 0, pixels.Length);
            for (int i = 0; i < pixels.Length; ++i)
            {
                if (pixels[i].A > 0.0f && pixels2[i].A > 0.0f)
                {
                    IntersectingLava = true;
                    return;
                }

            }
            IntersectingLava = false;
        }
    }
}
