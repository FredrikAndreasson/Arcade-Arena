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
        private float rollCooldown;
        private float rolling;

        private float rotation;
        

        public Assassin(Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            currentAnimation = new SpriteAnimation(AssetManager.SmallBox, Vector2.Zero, Vector2.Zero, new Vector2(10,10), new Vector2(10,10));
            rollCooldown = 0;
        }

        public override void Update()
        {
            

            if (Keyboard.GetState().IsKeyDown(Keys.E) && rollCooldown <= 0 )
            {
                Roll();
            }
            else
            {
                rollCooldown -= (float)Game1.elapsedGameTimeSeconds;
                if (rolling > 0)
                {
                    rotation += (float)(Math.PI / 180) * 5;
                    UpdateVelocity(aimDirection, speed);
                    position += velocity;
                    rolling -= (float)Game1.elapsedGameTimeMilliseconds;
                    return;
                }
            }
            base.Update();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, position, rotation, new Vector2(5, 5), Game1.SCALE);
        }

        private void Roll()
        {
            rollCooldown = ROLL_COOLDOWN;
            rolling = 1000;
        }


    }
}
