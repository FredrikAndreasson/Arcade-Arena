using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcade_Arena.Classes;
using Arcade_Arena.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class TimeZone : Ability
    {
        double timer;
        TimeTraveler owner;
        private Texture2D texture;

        public TimeZone(double timer, TimeTraveler owner, Vector2 position, Texture2D texture, float speed, double direction) : base(position, speed, direction, 0)
        {
            Type = AbilityOutline.AbilityType.AbilityTwo;
            currentAnimation = new SpriteAnimation(AssetManager.TimeTravelerTimeZone, new Vector2(0, 0), new Vector2(0, 0), new Vector2(50, 50), new Vector2(1, 1), 50000);
            this.timer = timer;
            this.owner = owner;
            this.texture = texture;
        }

        public override void Update()
        {
            timer -= Game1.elapsedGameTimeSeconds;

            if (timer <= 0)
            {
                isDead = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1);
        }
    }
}
