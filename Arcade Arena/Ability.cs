using Arcade_Arena.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    public abstract class Ability
    {
        protected Vector2 position;
        protected SpriteAnimation currentAnimation;

        public Ability()
        {
            
        }

        public string Username { get; set; }

        public Vector2 Position => position;

        public SpriteAnimation CurrentAnimation => currentAnimation;

        public AbilityOutline.AbilityType Type { get; set; }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
