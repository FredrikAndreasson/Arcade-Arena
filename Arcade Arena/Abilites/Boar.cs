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
        int offset = 0;
        
        public Boar(Huntress owner, double direction, Vector2 position, float speed, Rectangle clientBounds) : base(position, speed, direction)
        {
            animation = new SpriteAnimation(AssetManager.HuntressBoar, new Vector2(0, 0), new Vector2(2, 0), new Vector2(20, 9), new Vector2(2, 0), 100);

            timer = 20;
            this.owner = owner;
            CalculateStartingPosition(position, clientBounds);
            CheckRotation();
        }

        private void CheckRotation()
        {
            if (direction > Math.PI * 0.5 && direction < Math.PI * 1.5)
            {
                animation.SpriteFX = SpriteEffects.FlipHorizontally;
            }
        }

        private void CalculateStartingPosition(Vector2 ownerPosition, Rectangle clientBounds)
        {
            /*bool ready = false;
            while (!ready)
            {
                UpdateVelocity(direction, -10);
                if (position.X < 0 || position.X > clientBounds.Width
                    || position.Y < 0 || position.Y > clientBounds.Height)
                {
                    ready = true;
                }
            }*/

            double tempDirection = direction - Math.PI;
            if (tempDirection < 0)
            {
                tempDirection += Math.PI * 2;
            }
            double shortestTravelDistance = CalculateTravelDistance(clientBounds, tempDirection);
            UpdateVelocity(tempDirection, (float)shortestTravelDistance);
        }

        private double CalculateTravelDistance(Rectangle clientBounds, double tempDirection)
        {
            double travelDistance1, travelDistance2;
            double distanceX, distanceY;
            double angle;

            if (tempDirection <= Math.PI * 0.5f)
            {
                distanceX = MathHelper.Distance(position.X, clientBounds.Width);
                distanceY = MathHelper.Distance(position.Y, clientBounds.Height);
                angle = tempDirection;
                travelDistance1 = distanceX / Math.Cos(angle);
                angle = Math.PI * 0.5f - angle;
                travelDistance2 = distanceY / Math.Cos(angle);
                position.X += offset;
                position.Y += offset;
            }
            else if (tempDirection <= Math.PI * 1)
            {
                distanceX = MathHelper.Distance(position.X, 0);
                distanceY = MathHelper.Distance(position.Y, clientBounds.Height);
                angle = tempDirection - Math.PI * 0.5f;
                travelDistance1 = distanceY / Math.Cos(angle);
                angle = Math.PI * 0.5f - angle;
                travelDistance2 = distanceX / Math.Cos(angle);
                position.X += offset;
                position.Y -= offset;
            }
            else if (tempDirection <= Math.PI * 1.5f)
            {
                distanceX = MathHelper.Distance(position.X, 0);
                distanceY = MathHelper.Distance(position.Y, 0);
                angle = tempDirection - Math.PI;
                travelDistance1 = distanceX / Math.Cos(angle);
                angle = Math.PI * 0.5f - angle;
                travelDistance2 = distanceY / Math.Cos(angle);
                position.X -= offset;
                position.Y -= offset;
            }
            else
            {
                distanceX = MathHelper.Distance(position.X, clientBounds.Width);
                distanceY = MathHelper.Distance(position.Y, 0);
                angle = tempDirection - Math.PI * 1.5f;
                travelDistance1 = distanceY / Math.Cos(angle);
                angle = Math.PI * 0.5f - angle;
                travelDistance2 = distanceX / Math.Cos(angle);
                position.X -= offset;
                position.Y += offset;
            }

            return Math.Min(travelDistance1, travelDistance2);
        }

        public void Update()
        {
            UpdateVelocity(direction, speed);
            animation.Update();
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
            spriteBatch.DrawString(AssetManager.CooldownFont, position.ToString(), new Vector2(200, 200), Color.Black);
        }
    }
}
