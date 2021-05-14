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
        public static float orbiterRotation = 0;
        float cooldownTimer;
        Vector2 distance;
        private int xPos;
        private int yPos;

        public ProjectileCharacter(Vector2 position, float speed, double direction) : base(position, speed, direction) 
        {
            weaponOrigin = new Vector2(0, 0);
            cooldownTimer = 0;
            xPos = 35;
            yPos = 65;
        }

        public void UpdateWeapon()
        {
            weaponPosition.X = Position.X + xPos;
            weaponPosition.Y = Position.Y + yPos;
            MouseState mousePosition = Mouse.GetState();

            distance.X = mousePosition.X - Position.X - xPos;
            distance.Y = mousePosition.Y - Position.Y - yPos;

            orbiterRotation = (float)Math.Atan2(distance.Y, distance.X);

            cooldownTimer += (float)Game1.elapsedGameTimeSeconds;

            if (cooldownTimer >= 1f && (Keyboard.GetState().IsKeyDown(Keys.Space) || MouseKeyboardManager.Pressed(Buttons.RightTrigger)))
            {
                Shoot();
                cooldownTimer = 0;
            }
        }

        public virtual void Shoot()
        {

            Projectile projectile = new Projectile(1, 3, Position, speed/2, (double)orbiterRotation);
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
