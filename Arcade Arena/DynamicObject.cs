using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class DynamicObject : GameObject
    {
        protected float speed;
        protected double direction;
        protected Vector2 velocity;
        public double SpeedAlteration { get; set; } //för time zone

        public DynamicObject(Vector2 position, float speed, double direction) : base(position)
        {
            SpeedAlteration = 1;
            this.speed = speed;
            this.direction = direction;
        }
    }
}
