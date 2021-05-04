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
        public abstract void Update(GameTime gameTime, ref States state);


    }
}