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
        List<Projectile> projectileList;

        public ProjectileCharacter(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction) 
        {
            weaponOrigin = new Vector2(0, 0);
            cooldownTimer = 0;
            projectileList = new List<Projectile>();
        }

        public void UpdateWeapon(GameTime gameTime)
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

            cooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (cooldownTimer >= 0.1f && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Shoot();
                cooldownTimer = 0;
            }
            UpdateProjectile(gameTime);

        }

        public void UpdateProjectile(GameTime gameTime)
        {
            foreach (Projectile projectile in projectileList)
            {
                projectile.position += projectile.velocity;
                if (Vector2.Distance(projectile.position, position) > 500)
                {
                    projectile.projectileIsActive = false;
                }
            }

            for (int i = 0; i < projectileList.Count; i++)
            {
                if (!projectileList[i].projectileIsActive)
                {
                    projectileList.RemoveAt(i);
                }
            }
        }

        public void Shoot()
        {
            Projectile projectile = new Projectile(position, AssetManager.WizardWandProjectile, speed, direction);
            projectile.velocity = new Vector2((float)Math.Cos(orbiterRotation) * 10f, (float)Math.Sin(orbiterRotation) * 10f);
            projectile.position = (position - (new Vector2(-40,-58))) + projectile.velocity;
            projectile.projectileIsActive = true;
            if (projectileList.Count() < 40)
            {
                projectileList.Add(projectile);
            }
        }

        public virtual void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.WizardWand, new Vector2(weaponPosition.X, weaponPosition.Y), null, Color.White, orbiterRotation, weaponOrigin, 6, SpriteEffects.None, 0);
            foreach (Projectile projectile in projectileList)
            {
                projectile.Draw(spriteBatch);
            }

        }
    }
}
