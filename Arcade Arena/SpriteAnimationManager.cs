using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    class SpriteAnimationManager
    {
        private Texture2D texture;
        private int rows;
        private int columns;
        int x;
        int y;
        private float msBetweenFrames;
        private float msSinceLastFrame;

        public SpriteAnimationManager(Texture2D texture, int rows, int columns)
        {
            this.texture = texture;
            this.rows = rows;
            this.columns = columns;

            msBetweenFrames = 50;
            x = 0;
            y = 0;
        }

        private Rectangle Source => new Rectangle(0, 0, (int)(texture.Width / columns) * x, (int)(texture.Height / rows) * y);

        public void Update(GameTime gameTime)
        {
            if (msSinceLastFrame >= msBetweenFrames)
            {
                if (x >= columns)
                {
                    x = 0;
                    y++;
                }
                else
                {
                    x++;
                }
                msSinceLastFrame = 0;
            }
            else
            {
                msSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 origin, float scale)
        {
            spriteBatch.Draw(texture, position, Source, Color.White, rotation, origin, scale, SpriteEffects.None, 1.0f);
        }
    }
}
