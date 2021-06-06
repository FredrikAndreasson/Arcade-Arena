using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Abilites
{
    class BodySlam : Ability
    {
        public BodySlam(Character player, Vector2 position, float speed, double direction) : base(position, speed, direction, 10)
        {
            Type = Library.AbilityOutline.AbilityType.MeeleAttack;
            currentAnimation = new SpriteAnimation(AssetManager.ogreSpriteSheet, new Vector2(3, 3), new Vector2(5, 3), new Vector2(23, 33), new Vector2(7, 3), 300);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //currentAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, Game1.SCALE);
        }

        public override void Update()
        {
            currentAnimation.Update();
            if (currentAnimation.Loop > 0 && currentAnimation.XIndex == 0)
            {
                isDead = true;
            }
        }

    }
}
