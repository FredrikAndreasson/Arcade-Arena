using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcade_Arena
{
    class Ball : Character
    {

        public Ball(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {

        }
    }
}
