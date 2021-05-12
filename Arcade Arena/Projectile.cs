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
        public bool projectileIsActive;
        public Vector2 velocity;
        public int damage;
        private double activeTimer;


        public Projectile(int damage, double timer, Vector2 position, float speed, double direction)
        {
            Type = Library.AbilityOutline.AbilityType.Projectile;
            this.damage = damage;
            activeTimer = timer;
            this.direction = direction;
            currentAnimation = new SpriteAnimation(AssetManager.WizardWandProjectile, Vector2.Zero, Vector2.Zero,
                new Vector2(2, 1), new Vector2(2, 1));
        }

        public void SetPosition(Vector2 pos)
        {
            position = pos;
        }

        public override void Update()
        {
            position += velocity;
            activeTimer -= Game1.elapsedGameTimeSeconds;
            if (activeTimer <= 0)
            {
                isDead = true;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, Game1.SCALE);
        }
    }
}
