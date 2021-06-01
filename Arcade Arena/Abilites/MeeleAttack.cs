using Arcade_Arena.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Abilites
{
    class MeeleAttack : Ability
    {

        public Rectangle damageRect;
   
        public MeeleAttack(Character player, Vector2 position, float speed, double direction) : base(position, speed, direction, 10)
        {
            Type = Library.AbilityOutline.AbilityType.MeeleAttack;
            damageRect = new Rectangle(player.Position.ToPoint(), new Point((int)currentAnimation.FrameSize.X, (int)currentAnimation.FrameSize.Y));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //currentAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, Game1.SCALE);
        }

        public override void Update()
        {
            damageRect = new Rectangle(player.Position.ToPoint(), new Point((int)currentAnimation.FrameSize.X, (int)currentAnimation.FrameSize.Y));
        }

    }
}
