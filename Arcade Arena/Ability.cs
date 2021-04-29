using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    abstract class Ability
    {
        protected string userName;
        protected Vector2 position;
        protected SpriteAnimation currentAnimation;

        public Ability(string userName)
        {
            this.userName = userName;
        }

        public string UserName => userName;

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
