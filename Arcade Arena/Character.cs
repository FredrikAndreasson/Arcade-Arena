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
    class Character : DynamicObject
    {
        protected int weaponLvl; //pay for lvls or get powerups?
        protected int health;
        protected int mana;


        public Character(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position.Y -= speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                position.X -= speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position.Y += speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position.X += speed;
            }

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
