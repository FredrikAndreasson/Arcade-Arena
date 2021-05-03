using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class DynamicObject : GameObject
    {
        protected float speed;
        protected double direction;
        protected Vector2 velocity;
        public double speedAlteration { get; set; } //för time zone

        public DynamicObject(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture)
        {
            speedAlteration = 1;
            this.speed = speed;
            this.direction = direction;
        }
    }
}
