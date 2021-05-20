using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcade_Arena.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena.Abilites
{
    public class Boar : DynamicObject
    {
        SpriteAnimation animation;

        double timer;
        Huntress owner;
        double damage = 1;
        
        public Boar(Huntress owner, double direction, Vector2 position, float speed, Rectangle clientBounds) : base(position, speed, direction)
        {
            animation = new SpriteAnimation(AssetManager.HuntressBoar, new Vector2(0, 0), new Vector2(2, 0), new Vector2(20, 9), new Vector2(2, 0), 200);

            timer = 0;
            this.owner = owner;
            CalculateStartingPosition(position, clientBounds);
            CheckRotation(direction);
        }

        private void CheckRotation(double direction)
        {
            if (direction > Math.PI * 0.5 && direction < Math.PI * 1.5)
            {
                animation.SpriteFX = SpriteEffects.FlipHorizontally;
            }
        }

        private void CalculateStartingPosition(Vector2 ownerPosition, Rectangle clientBounds)
        {
            bool ready = false;
            while (!ready)
            {
                UpdateVelocity(direction, -10);
                if (position.X < 0 || position.X > clientBounds.Width
                    || position.Y < 0 || position.Y > clientBounds.Height)
                {
                    ready = true;
                }
            }
        }

        public void Update()
        {
            UpdateVelocity(direction, speed);
            timer -= Game1.elapsedGameTimeSeconds;
            if (timer <= 0)
            {
                Despawn();
            }
        }

        private void UpdateVelocity(double newDirection, float newSpeed)
        {
            velocity.Y = (float)(Math.Sin((float)newDirection) * newSpeed * speedAlteration);
            velocity.X = (float)(Math.Cos((float)newDirection) * newSpeed * speedAlteration);
            position += velocity;
        }

        void Despawn()
        {
            owner.DespawnBoar(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, Game1.SCALE);
        }
    }
}
