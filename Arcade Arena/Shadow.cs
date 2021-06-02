using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    public class Shadow : DynamicObject
    {
        public Texture2D texture;
        private Vector2 frameSize = new Vector2(0, 85);

        public Shadow(Vector2 position, Texture2D texture, float speed, double direction, Vector2 frameSize) : base(position, speed, direction)
        {
            this.texture = texture;
            this.frameSize = frameSize;
            
        }

        public void Update(Vector2 newPos)
        {

            position = new Vector2(newPos.X + AssetManager.OgreShadow.Width / 4, newPos.Y + frameSize.Y*4);


        }

        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);

        public void Draw(SpriteBatch spriteBatch)
        {
           spriteBatch.Draw(texture, Position, Color.Red);
        }

    }
}
