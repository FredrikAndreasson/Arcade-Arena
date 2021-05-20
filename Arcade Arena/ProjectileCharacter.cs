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
    public class ProjectileCharacter : Character
    {
        public float rotation;
        public Vector2 weaponPosition;
        public Vector2 weaponOrigin;
        float cooldownTimer;
        Vector2 distance; 

        public ProjectileCharacter(Vector2 position, float speed, double direction) : base(position, speed, direction) 
        {
            weaponOrigin = new Vector2(0, 0);
            cooldownTimer = 0;
        }

        public void UpdateWeapon()
        {
            weaponPosition.X = lastPosition.X + 35;
            weaponPosition.Y = lastPosition.Y + 65;
            MouseState mousePosition = Mouse.GetState();

            distance.X = mousePosition.X - Position.X - 35;
            distance.Y = mousePosition.Y - Position.Y - 65;

            orbiterRotation = (float)Math.Atan2(distance.Y, distance.X);
           // Console.WriteLine(orbiterRotation);
            //if (Keyboard.GetState().IsKeyDown(Keys.O))
            //{
            //    orbiterRotation += 0.1f;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.P))
            //{
            //    orbiterRotation -= 0.1f;
            //}

            cooldownTimer += (float)Game1.elapsedGameTimeSeconds;

            if (cooldownTimer >= 1f && (Keyboard.GetState().IsKeyDown(Keys.Space) || MouseKeyboardManager.Pressed(Buttons.RightTrigger)))
            {
                Shoot();
                cooldownTimer = 0;
            }

        }

        public virtual void Shoot()
        {

            Projectile projectile = new Projectile(1, 1, Position, speed/2, (double)orbiterRotation);
            projectile.velocity = new Vector2((float)Math.Cos(orbiterRotation) * 10f, (float)Math.Sin(orbiterRotation) * 10f);
            projectile.SetPosition((Position - (new Vector2(-40,-58))) + projectile.velocity);
            projectile.projectileIsActive = true;
            abilityBuffer.Add(projectile);
        }

        public virtual void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.WizardWand, new Vector2(weaponPosition.X, weaponPosition.Y), null, Color.White, orbiterRotation,
                weaponOrigin, 6, SpriteEffects.None, 0);

        }
    }
}
