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

        public Shadow(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {

        }

        public void Update(Vector2 newPos)
        {
            if (Texture == AssetManager.OgreShadow)
            {

            }
            else if (Texture == AssetManager.WizardShadow)
            {
                position = new Vector2(newPos.X + AssetManager.WizardShadow.Width / 4, newPos.Y+85);
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.Red);
        }

    }
}
