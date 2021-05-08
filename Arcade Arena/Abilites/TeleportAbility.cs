using Arcade_Arena.Classes;
using Arcade_Arena.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Abilites
{
    class TeleportAbility : Ability
    {
        Wizard player;
        public TeleportAbility(Wizard player, Vector2 position)
        {
            this.player = player;

            Type = AbilityOutline.AbilityType.AbilityTwo;
            currentAnimation = new SpriteAnimation(AssetManager.WizardSpriteSheet, new Vector2(0, 3), new Vector2(5, 3), new Vector2(14, 20), new Vector2(7, 3), 150);

            this.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, Game1.SCALE);
        }

        public override void Update()
        {
            currentAnimation.Update();
            if (!player.Teleporting)
            {
                isDead = true;
            }
        }
    }
}
