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
    public class BearTrap : GameObject
    {
        SpriteAnimation openAnimation;
        SpriteAnimation activatedAnimation;

        SpriteAnimation currentAnimation;

        double timer;
        bool activated = false;
        Huntress owner;
        Texture2D texture;

        public BearTrap(Huntress owner, Vector2 position) : base(position)
        {
            openAnimation = new SpriteAnimation(AssetManager.HuntressBearTrap, new Vector2(0, 0), new Vector2(0, 0), new Vector2(10, 6), new Vector2(1, 0), 5000);
            activatedAnimation = new SpriteAnimation(AssetManager.HuntressBearTrap, new Vector2(1, 0), new Vector2(1, 0), new Vector2(10, 6), new Vector2(1, 0), 5000);

            currentAnimation = openAnimation;
            this.owner = owner;
            timer = 10;
        }

        public void Update()
        {
            timer -= Game1.elapsedGameTimeSeconds;
            if (timer <= 0)
            {
                Despawn();
            }
            if (!activated)
            {
                if (true)//check collision
                {
                    //trap character
                    currentAnimation = activatedAnimation;
                    activated = true;
                    timer = 4;
                }
            }
        }

        public void Despawn()
        {
            owner.DespawnBearTrap(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch, position, 0.0f, Vector2.Zero, Game1.SCALE);
        }
    }
}
