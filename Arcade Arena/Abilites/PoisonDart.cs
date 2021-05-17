using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Abilites
{
    class PoisonDart : Projectile
    {
        public PoisonDart(int damage, double timer, Vector2 position, float speed, double direction) : base(damage, timer, position, speed, direction)
        {
            Type = Library.AbilityOutline.AbilityType.AbilityTwo;
            currentAnimation = new SpriteAnimation(AssetManager.PoisonDart, new Vector2(0, 0), new Vector2(0, 0), new Vector2(15, 7), new Vector2(15, 7));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
