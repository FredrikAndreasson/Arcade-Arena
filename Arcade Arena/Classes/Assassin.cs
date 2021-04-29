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
        SpriteAnimation walkingAnimation;
        SpriteAnimation backwardsAnimation;

        private const float ROLL_COOLDOWN_TIME = 5000;
        private const float DART_COOLDOWN_TIME = 5000;

        private float rollCooldown = 0;
        private float dartCooldown;
        
        private float rotation = 0;
        private Vector2 origin;

        private bool rolling = false;
        private float rollTimer;

        public Assassin(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(AssetManager.TargetDummy, Vector2.Zero, Vector2.Zero, new Vector2(32, 64), new Vector2(32, 64));

            currentAnimation = walkingAnimation;

            origin = new Vector2(AssetManager.TargetDummy.Width / 2, AssetManager.TargetDummy.Height / 2);
        }

        public override void Update()
        {
            rollCooldown -= (float)Game1.elapsedGameTimeMilliseconds;
            if (Keyboard.GetState().IsKeyDown(Keys.E) && rollCooldown < 0)
            {
                rollCooldown = ROLL_COOLDOWN_TIME;
                Roll();
            }
            if (!rolling)
            {
                rotation = 0;
                base.Update();
            }
            else
            {

                rotation += (float)((Math.PI / 180) * 13);
                rotation %= (float)(Math.PI * 2);
                UpdateVelocity(aimDirection, 1);
                rollTimer -= (float)Game1.elapsedGameTimeMilliseconds;
            }

            if (rollTimer < 0)
            {
                rolling = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            currentAnimation.Draw(spriteBatch, position, rotation, origin, 2.0f);
        }

        private void Roll()
        {
            rolling = true;
            rollTimer = 2000;
        }

        private void Shoot()
        {
            //implementera efter merge av aim kod
        }
    }
}
