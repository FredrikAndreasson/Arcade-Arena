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
    class Assassin : Character
    {
        private const float ROLL_COOLDOWN = 5;
        private const float DART_COOLDOWN = 5;

        private float rollCooldown;
        private float rolling;
        private float dartCooldown;

        private float rotation;
        private Vector2 origin;
        

        public Assassin(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            currentAnimation = new SpriteAnimation(AssetManager.SmallBox, Vector2.Zero, Vector2.Zero, new Vector2(10,10), new Vector2(10,10));
            rollCooldown = 0;
            dartCooldown = 0;
            origin = new Vector2(5, 5);
        }

        public override void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Q) && dartCooldown <= 0)
            {
                Shoot();
            }
            else { dartCooldown -= (float)Game1.elapsedGameTimeSeconds; }

            if (Keyboard.GetState().IsKeyDown(Keys.E) && rollCooldown <= 0 )
            {
                Roll();
            }
            else
            {
                rollCooldown -= (float)Game1.elapsedGameTimeSeconds;
                if (rolling > 0)
                {
                    rotation += (float)(Math.PI / 180) * 30;
                    UpdateVelocity(aimDirection, speed*5);
                    rolling -= (float)Game1.elapsedGameTimeMilliseconds;
                    return;
                }
            }
            base.Update();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, position, rotation, origin, Game1.SCALE);
        }

        private void Roll()
        {
            rollCooldown = ROLL_COOLDOWN;
            rolling = 300;
            middleOfSprite = new Vector2(position.X + currentAnimation.FrameSize.X / 2, position.Y + currentAnimation.FrameSize.Y / 2);
            aimDirection = UpdateAimDirection();
        }

        public virtual void Shoot()
        {
            dartCooldown = DART_COOLDOWN;
            middleOfSprite = new Vector2(position.X + currentAnimation.FrameSize.X / 2, position.Y + currentAnimation.FrameSize.Y / 2);
            aimDirection = UpdateAimDirection();
            PoisonDart dart = new PoisonDart(1, 1, Position, speed / 2, (double)aimDirection);
            dart.velocity = new Vector2((float)Math.Cos(aimDirection) * 1f, (float)Math.Sin(aimDirection) * 1f);
            dart.SetPosition((Position - (new Vector2(currentAnimation.FrameSize.X*2, currentAnimation.FrameSize.Y*2))) + dart.velocity);
            dart.projectileIsActive = true;
            abilityBuffer.Add(dart);
        }


    }
}
