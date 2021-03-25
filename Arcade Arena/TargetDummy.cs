using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    class TargetDummy : GameObject
    {

        public TargetDummy(Vector2 position, Texture2D texture) : base(position, texture)
        {

        }

        public Rectangle bounds => new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
