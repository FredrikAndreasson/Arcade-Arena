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

        protected float orbiterRotation = 0;

        protected int weaponLvl; //pay for lvls or get powerups?
        protected bool walking = false;

        public bool CanWalk { get; private set; }
        private int nCanWalkStoppingEffects;
       

        public bool Stunned { get; private set; }
        private int nStunEffects;
        

        public bool Invincible { get; private set; }
        private int nInvincibleEffects;
       

        protected int mana;

        protected sbyte maxHealth;

        protected bool isDead = false;

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
            CanWalk = true;
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
            if (Stunned)
            {

            }
            else
            {
                direction = UpdateMovementDirection();
                aimDirection = UpdateAimDirection();
                if (walking && CanWalk)
                {
                    UpdatePosition();
                    UpdateVelocity(direction, speed);
                }
            }

            if (LastToDamageTimer < 0)
            {
                LastToDamage = "";
            }
            else
            {
                LastToDamageTimer -= (float)Game1.elapsedGameTimeSeconds;
            }
            UpdateSpriteEffect();
        }

        protected virtual void Die()
        {
            isDead = true;
        }

        public void AddCanWalkStoppingEffect()
        {
            nCanWalkStoppingEffects++;
            CanWalk = true;
        }
        public void RemoveCanWalkStoppingEffect()
        {
            nCanWalkStoppingEffects--;
            if (nCanWalkStoppingEffects <= 0)
            {
                CanWalk = false;
                nCanWalkStoppingEffects = 0;
            }
        }

        public void AddInvincibleEffect()
        {
            nInvincibleEffects++;
            Invincible = true;
        }
        public void RemoveInvincibleEffect()
        {
            nInvincibleEffects--;
            if (nInvincibleEffects <= 0)
            {
                Invincible = false;
                nInvincibleEffects = 0;
            }
        }

        public void AddStunEffect()
        {
            nStunEffects++;
            Stunned = true;
        }
        public void RemoveStunEffect()
        {
            nStunEffects--;
            if (nStunEffects <= 0)
            {
                Stunned = false;
                nStunEffects = 0;
            }
        }

        protected void UpdateSpriteEffect()
        {
            if (aimDirection >= 1.53269 || aimDirection <= -1.547545)
            {
                currentAnimation.SpriteFX = SpriteEffects.FlipHorizontally;
            }
            else
            {
                currentAnimation.SpriteFX = SpriteEffects.None;
            }
        }

        protected bool WalkingBackwards()
        {
            if ((direction > Math.PI * 0.5 && direction < Math.PI * 1.5) && !(aimDirection >= 1.53269 || aimDirection <= -1.547545))
            {
                return true;
            }
            if (!(direction > Math.PI * 0.5 && direction < Math.PI * 1.5) && (aimDirection >= 1.53269 || aimDirection <= -1.547545))
            {
                return true;
            }
            return false;
        }
        
        public void UpdateVelocity(double newDirection, float newSpeed)
        {
            velocity.Y = (float)(Math.Sin((float)newDirection) * newSpeed * speedAlteration);
            velocity.X = (float)(Math.Cos((float)newDirection) * newSpeed * speedAlteration);
            position += velocity;
        }
        public void UpdatePosition()
        {
            if (Blocked)
            {
                position = LastPosition;
            }
            else
            {
                LastPosition = position;
            }
            
        }

        public virtual void TakeDamage(sbyte damage, string username, float timerSeconds)
        {
            if (!Invincible)
            {
                health -= damage;
                LastToDamage = username;
                LastToDamageTimer = timerSeconds;
                ExitAnimationOnHit();
                if (health <= 0)
                {
                    Die();
                }
            }
        }

        public void Heal(sbyte amount)
        {
            health += amount;
            if (health > maxHealth || health < 0)
            {
                health = maxHealth;
            }
        }

        protected virtual void ExitAnimationOnHit()
        {

        }

        public virtual void StartKnockback() //behöver en egen metod pga sprite sheet
        {
            CanWalk = false;
            walking = false;
        }

        public virtual void EndKnockback()
        {
            CanWalk = true;
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
                    newDirection = Math.PI * 1.75;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A) || MouseKeyboardManager.LeftThumbStickLeft())
                {
                    newDirection = Math.PI * 1.25;
                }
                else
                {
                    newDirection = Math.PI*1.5;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) || MouseKeyboardManager.LeftThumbStickDown())
            {
                walking = true;
                if (Keyboard.GetState().IsKeyDown(Keys.D) || MouseKeyboardManager.LeftThumbStickRight())
                {
                    newDirection = Math.PI * 0.25;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A) || MouseKeyboardManager.LeftThumbStickLeft())
                {
                    newDirection = Math.PI * 0.75;
                }
                else
                {
                    newDirection = Math.PI * 0.5;
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
                    newDirection = Math.PI;
                }
            }

            return newDirection;
        }

        //returnerar aim angle i grader
        protected double UpdateAimDirection()
        {
            UpdateMiddleOfSprite();
            double newDirection = Math.Atan2(MouseKeyboardManager.MousePosition.Y - middleOfSprite.Y, MouseKeyboardManager.MousePosition.X - middleOfSprite.X);
            return newDirection;
        }

        protected void ChangeAnimation(ref SpriteAnimation currentAnim, SpriteAnimation newAnimation)
        {
            if (currentAnim != newAnimation)
            {
                currentAnim = newAnimation;
                currentAnim.StartAnimation();
            }
        }

        protected virtual void UpdateMiddleOfSprite()
        {

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
