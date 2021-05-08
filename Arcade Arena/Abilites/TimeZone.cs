using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcade_Arena.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class TimeZone : GameObject
    {
        double timer;
        double timeSlowEffectSeverity = -0.30;
        TimeTraveler owner;

        public TimeZone(double timer, TimeTraveler owner, Vector2 position, Texture2D texture) : base(position, texture)
        {
            this.timer = timer;
            this.owner = owner;
        }

        public void Update()
        {
            timer -= Game1.elapsedGameTimeSeconds;
            if(true)//kollision med dynamic objects, avoid med owner
            {
                //apply time slow effect
            }

            if (timer <= 0)
            {
                //remove this
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1);
        }
    }
}
