using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class Obstacle : GameObject
    {
        private Rectangle hitBox;
        private int heightShortener = 10;
        private Texture2D texture;
        public Rectangle HitBox()
        {
            return hitBox;
        }
        public Obstacle(Vector2 position, Texture2D texture) : base(position)
        {
            this.texture = texture;
            UpdateHitbox();
        }

        public void MoveObstacle(Vector2 newPosition)
        {
            position = newPosition;
            UpdateHitbox();
        }

        private void UpdateHitbox()
        {
            hitBox = new Rectangle((int)position.X, (int)position.Y + heightShortener, texture.Width, texture.Height - heightShortener);
        }
    }
}
