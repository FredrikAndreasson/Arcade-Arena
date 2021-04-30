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
    class IceblockAbility : Ability
    {
        Wizard player;
        public IceblockAbility(Wizard player)
        {
            this.player = player;

            Type = AbilityOutline.AbilityType.AbilityOne;
            currentAnimation = new SpriteAnimation(AssetManager.WizardIceBlock, new Vector2(0, 0), new Vector2(4, 0), 
                new Vector2(14, 20), new Vector2(4, 0), 1000);

            this.position = player.position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, 3.0f);
        }

        public override void Update()
        {
            currentAnimation.Update();
        }
    }
}
