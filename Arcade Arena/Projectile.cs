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
        public Vector2 velocity;
        public int damage;
        private double activeTimer;


        public Projectile(SpriteAnimation spriteAnim, int damage, double timer, Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            Type = Library.AbilityOutline.AbilityType.Projectile;
            this.damage = damage;
            activeTimer = timer;
            this.direction = direction;
            velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
            currentAnimation = spriteAnim;
        }

        public void SetPosition(Vector2 pos)
        {
            position = pos;
        }

        public override void Update()
        {
            currentAnimation.Update();
            position += velocity;
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
