using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    abstract class GameState
    {
        protected GameWindow Window;

        public GameState(GameWindow Window)
        {
            this.Window = Window;
        }


        public abstract void Draw(SpriteBatch spritebatch, States state);
        public abstract void Update(GameTime gameTime, ref States state, ref Character player);

        protected Vector2 SetAlignedXPos(Texture2D tex, int xPos)
        {
            Vector2 vector = new Vector2(xPos, Window.ClientBounds.Height / 2 - tex.Height / 2);
            return vector;
        }

        protected Vector2 SetAlignedYPos(Texture2D tex, int yPos)
        {
            Vector2 vector = new Vector2(Window.ClientBounds.Width / 2 - tex.Width / 2, yPos);
            return vector;
        }

    }
}