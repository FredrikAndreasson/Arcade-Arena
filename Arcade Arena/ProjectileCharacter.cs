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
    class ProjectileCharacter : Character
    {
        public float rotation;
        public Vector2 weaponPosition;
        public Vector2 weaponOrigin;
        public static float orbiterRotation = 0;
        float cooldownTimer;
        Vector2 distance; 

        public ProjectileCharacter(Vector2 position, float speed, double direction) : base(position, speed, direction) 
        {
            weaponOrigin = new Vector2(0, 0);
            cooldownTimer = 0;
        }

        public void UpdateWeapon()
        {
            weaponPosition.X = position.X + 50;
            weaponPosition.Y = position.Y + 60;
            MouseState mousePosition = Mouse.GetState();

            distance.X = mousePosition.X - position.X;
            distance.Y = mousePosition.Y - position.Y;

            orbiterRotation = (float)Math.Atan2(distance.Y, distance.X);
            Console.WriteLine(orbiterRotation);
            //if (Keyboard.GetState().IsKeyDown(Keys.O))
            //{
            //    orbiterRotation += 0.1f;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.P))
            //{
            //    orbiterRotation -= 0.1f;
            //}

            cooldownTimer += (float)Game1.elapsedGameTimeSeconds;

            if (cooldownTimer >= 1f && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Shoot();
                cooldownTimer = 0;
            }

        }

        public virtual void Shoot()
        {
            Projectile projectile = new Projectile(5, 2, position, speed, direction);
            projectile.velocity = new Vector2((float)Math.Cos(orbiterRotation) * 10f, (float)Math.Sin(orbiterRotation) * 10f);
            projectile.position = (position - (new Vector2(-40,-58))) + projectile.velocity;
            abilityBuffer.Add(projectile);

        }

        public virtual void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.WizardWand, new Vector2(weaponPosition.X, weaponPosition.Y), null, Color.White, orbiterRotation,
                weaponOrigin, 6, SpriteEffects.None, 0);
        }
    }
}
