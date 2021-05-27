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
    

    class GroundSlamAbility : Ability
    {
        Ogre player;

        public GroundSlamAbility(Ogre player, Vector2 position, float speed, double direction) : base(position, speed, direction)
        {
            this.player = player;

            Type = AbilityOutline.AbilityType.AbilityOne;
            currentAnimation= new SpriteAnimation(AssetManager.groundSmashCrackle, new Vector2(0, 0), new Vector2(4, 0), new Vector2(71, 71), new Vector2(4, 0), 500);

            this.position = player.Position-new Vector2(71, 71);

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
