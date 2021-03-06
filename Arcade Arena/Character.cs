using System;
using System.Collections.Generic;
using System.Diagnostics;
using Arcade_Arena.Classes;
using Arcade_Arena.Effects;
using Arcade_Arena.Managers;
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

        protected double abilityOneCooldown = 0;
        protected double abilityOneMaxCooldown = 10;
        protected double abilityTwoCooldown = 0;
        protected double abilityTwoMaxCooldown = 6;


        public bool Stunned { get; private set; }
        private int nStunEffects;
        

        public bool Invincible { get; private set; }
        private int nInvincibleEffects;
       

        protected int mana;

        public sbyte maxHealth { get; protected set; }

        protected float baseSpeed;

        protected bool isDead = false;

        protected double aimDirection;

        protected Vector2 middleOfSprite;

        public List<Effect> EffectList = new List<Effect>();

        public sbyte health;

        public bool isHit;

        public Shadow shadow;

        public List<Ability> abilityBuffer;

        public Character(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            abilityBuffer = new List<Ability>();
            IntersectingLava = false;
            health = 100;
            speed = 1;
            CanWalk = true;
            isHit = false;
        }

        public double AbilityOneCooldown => abilityOneCooldown;
        public double AbilityOneMaxCooldown => abilityOneMaxCooldown;
        public double AbilityTwoCooldown => abilityTwoCooldown;
        public double AbilityTwoMaxCooldown => abilityTwoMaxCooldown;

        public SpriteAnimation CurrentAnimation => currentAnimation;

        public sbyte Health { get { return health; } private set { health = value; } }

        public float OrbiterRotation { get { return orbiterRotation; } private set { orbiterRotation = value; } }

        public bool IsDead => isDead;

        public bool IntersectingLava { get; set; }

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
                    UpdateVelocity(direction, speed);
                }
            }

            UpdateSpriteEffect();
        }

        protected virtual void Die()
        {
            isDead = true;
        }

        public void SpawnLocation(Vector2 position)
        {
            RemoveAllEffects();
            this.position = position;
            LastPosition = position;
            health = maxHealth;
            isDead = false;
            InvincibilityEffect invincibilityEffect = new InvincibilityEffect(this, 1);
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
            Debug.Print("added stun effect" + nStunEffects);
        }
        public void RemoveStunEffect()
        {
            nStunEffects--;
            if (nStunEffects <= 0)
            {
                Stunned = false;
                nStunEffects = 0;
            }
            Debug.Print("removed stun effect" + nStunEffects);
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
            UpdatePosition();
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

        public virtual void TakeDamage(sbyte damage)
        {
            if (!Invincible)
            {
                health -= damage;
                isHit = true;
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

        public void ChangeSpeed(float value)
        {
            speed += value;
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

        protected void UpdateMiddleOfSprite()
        {
            middleOfSprite = new Vector2(Position.X + (currentAnimation.FrameSize.X *  Game1.SCALE / 2), Position.Y + (currentAnimation.FrameSize.Y * Game1.SCALE / 2));
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
            try{
                lava.renderTarget.GetData(0, new Rectangle(shadow.Position.ToPoint(), new Point(shadow.texture.Width, shadow.texture.Height)), pixels, 0, pixels.Length);
            }  catch (ArgumentException e)
            {
                IntersectingLava = true;
                return;
            }
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
