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
    class DynamicObject : GameObject
    {
        protected float speed;
        protected double direction;
        protected Vector2 velocity;

        public DynamicObject(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture)
        {
            this.speed = speed;
            this.direction = direction;
        }
    }
}
