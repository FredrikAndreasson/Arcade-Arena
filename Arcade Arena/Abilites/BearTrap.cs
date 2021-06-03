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
    public class BearTrap : Ability
    {
        SpriteAnimation openAnimation;
        SpriteAnimation activatedAnimation;

        double timer;
        public bool Activated { get; set; }
        Huntress owner;
        Texture2D texture;

        public BearTrap(Huntress owner, sbyte damage, Vector2 position, float speed, double direction) : base(position, speed, direction, damage)
        {
            Type = Library.AbilityOutline.AbilityType.AbilityTwo;
            Activated = false;
            openAnimation = new SpriteAnimation(AssetManager.HuntressBearTrap, new Vector2(0, 0), new Vector2(0, 0), new Vector2(10, 6), new Vector2(1, 0), 5000);
            activatedAnimation = new SpriteAnimation(AssetManager.HuntressBearTrap, new Vector2(1, 0), new Vector2(1, 0), new Vector2(10, 6), new Vector2(1, 0), 5000);

            currentAnimation = openAnimation;
            this.owner = owner;
            timer = 20;
        }

        public override void Update()
        {
            timer -= Game1.elapsedGameTimeSeconds;
            if (timer <= 0)
            {
                isDead = true;
            }
        }

        public void Activate()
        {
            currentAnimation = activatedAnimation;
            Activated = true;
            timer = 5;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, Position, 0.0f, Vector2.Zero, Game1.SCALE);
        }
    }
}
