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

        protected int mana;

        protected bool isDead = false;

        protected bool invincible = false;



        protected double aimDirection;

        protected Vector2 middleOfSprite;

        public List<Effect> EffectList = new List<Effect>();

        public sbyte health;

        public Shadow shadow;

        public List<Ability> abilityBuffer;

        public Character(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            abilityBuffer = new List<Ability>();
            IntersectingLava = false;
            health = 100;
        }

        public SpriteAnimation CurrentAnimation => currentAnimation;

        public sbyte Health { get { return health; } private set { health = value; } }

        public bool IsDead => isDead;

        public bool IntersectingLava { get; set; }

        public string LastToDamage { get; set; } //This is used to keep track of which player last did damage to you.
        public float LastToDamageTimer { get; set; }//The player gets the kill if the damage has been within the span of this timer.

        public virtual void Update()
        {
            UpdateEffects();
            direction = UpdateMovementDirection();
            aimDirection = UpdateAimDirection();
            if (walking && canWalk)
            {
                UpdateVelocity(direction, speed);
            }

            if(health<= 0)
            {
                isDead = true;
            }

            if (LastToDamageTimer < 0)
            {
                LastToDamage = "";
            }
            else
            {
                LastToDamageTimer -= (float)Game1.elapsedGameTimeSeconds;
            }

        }

        public void TakeDamage(string username, float timerSeconds)
        {
            health -= 10;
            LastToDamage = username;
            LastToDamageTimer = timerSeconds;
        }
        
        public void UpdateVelocity(double newDirection, float newSpeed)
        {
            velocity.Y = (float)(Math.Sin((float)newDirection) * newSpeed * speedAlteration);
            velocity.X = (float)(Math.Cos((float)newDirection) * newSpeed * speedAlteration);
            position += velocity;
        }

        public void TakeDamage(int damage, string username, float timerSeconds)
        {
            if (!invincible)
            {
                health -= (sbyte)damage;
                LastToDamage = username;
                LastToDamageTimer = timerSeconds;
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

            if (Keyboard.GetState().IsKeyDown(Keys.W) || MouseKeyboardManager.LeftThumbStickUp())
            {
                walking = true;
                if (Keyboard.GetState().IsKeyDown(Keys.D) || MouseKeyboardManager.LeftThumbStickRight())
                {
                    newDirection = 315;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A) || MouseKeyboardManager.LeftThumbStickLeft())
                {
                    newDirection = 225;
                }
                else
                {
                    newDirection = 270;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) || MouseKeyboardManager.LeftThumbStickDown())
            {
                walking = true;
                if (Keyboard.GetState().IsKeyDown(Keys.D) || MouseKeyboardManager.LeftThumbStickRight())
                {
                    newDirection = 45;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A) || MouseKeyboardManager.LeftThumbStickLeft())
                {
                    newDirection = 135;
                }
                else
                {
                    newDirection = 90;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || MouseKeyboardManager.LeftThumbStickRight())
            {
                walking = true;
                {
                    newDirection = 0;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A) || MouseKeyboardManager.LeftThumbStickLeft())
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
