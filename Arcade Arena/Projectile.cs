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
    class Projectile : DynamicObject
    {
        public bool projectileIsActive;
        public Vector2 velocity;
        public int damage;
        double activeTimer;

        public Projectile(int damage, double timer, Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {
            projectileIsActive = false;
            this.damage = damage;
            activeTimer = timer;
        }

        public void Update()
        {
            position += velocity;
            activeTimer -= Game1.elapsedGameTimeSeconds;
            if (activeTimer <= 0)
            {
                projectileIsActive = false;
            }
        }

        public void CheckCollision()
        {
            foreach(Character player in Game1.PlayerList)
            {
                if (true)//check collision 
                {

                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 6, SpriteEffects.None, 1);
        }
    }
}