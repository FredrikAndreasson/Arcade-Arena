using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcade_Arena
{
    class Projectile : Ability
    {
        public Vector2 Velocity;
        private double activeTimer;


        public Projectile(SpriteAnimation spriteAnim, sbyte damage, double timer, Vector2 position, float speed, double direction) : base(position, speed, direction, damage)
        {
            Type = Library.AbilityOutline.AbilityType.Projectile;
            activeTimer = timer;
            this.direction = direction;
            Velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
            currentAnimation = spriteAnim;
        }

        public void SetPosition(Vector2 pos)
        {
            position = pos;
        }

        public override void Update()
        {
            currentAnimation.Update();
            position += Velocity;
            activeTimer -= Game1.elapsedGameTimeSeconds;
            if (activeTimer <= 0)
            {
                isDead = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, Position, (float)direction, Vector2.Zero, Game1.SCALE);
        }
    }
}
