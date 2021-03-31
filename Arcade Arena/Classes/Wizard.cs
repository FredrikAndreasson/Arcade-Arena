using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Classes
{
    class Wizard : Character
    {
        SpriteAnimation currentAnimation;
        SpriteAnimation walkingAnimation;
        SpriteAnimation teleportInAnimation;
        SpriteAnimation teleportOutAnimation;
        SpriteAnimation backwardsAnimation;
        public Wizard(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {
            walkingAnimation = new SpriteAnimation(texture, new Vector2(2, 0), new Vector2(7, 0), new Vector2(14, 20), new Vector2(7, 3), 150);
            teleportInAnimation = new SpriteAnimation(texture, new Vector2(0, 2), new Vector2(4, 2), new Vector2(14, 20), new Vector2(7, 3), 150);
            teleportOutAnimation = new SpriteAnimation(texture, new Vector2(0, 3), new Vector2(3, 3), new Vector2(14, 20), new Vector2(7, 3), 150);
            backwardsAnimation = new SpriteAnimation(texture, new Vector2(0, 1), new Vector2(5, 1), new Vector2(14, 20), new Vector2(7, 3), 150);



            currentAnimation = backwardsAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            currentAnimation.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, 5.0f);
        }
    }
}
