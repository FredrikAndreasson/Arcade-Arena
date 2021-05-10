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
        public Shadow(Vector2 position, Texture2D texture, float speed, double direction) : base(position, speed, direction)
        {
            this.texture = texture;
        }

        public void Update(Vector2 newPos)
        {
            if (texture == AssetManager.OgreShadow)
            {

            }
            else if (texture == AssetManager.WizardShadow)
            {
                position = new Vector2(newPos.X + AssetManager.WizardShadow.Width / 4, newPos.Y+85);
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.Red);
        }

    }
}
