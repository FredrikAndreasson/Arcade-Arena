﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class Obstacle : GameObject
    {
        private Rectangle hitBox;
        private int heightShortener = 10;
        public Rectangle HitBox()
        {
            return hitBox;
        }
        public Obstacle(Vector2 position, Texture2D texture) : base(position, texture)
        {
            UpdateHitbox();
        }

        public void MoveObstacle(Vector2 newPosition)
        {
            position = newPosition;
            UpdateHitbox();
        }

        private void UpdateHitbox()
        {
            hitBox = new Rectangle((int)Position.X, (int)Position.Y + heightShortener, Texture.Width, Texture.Height - heightShortener);
        }
    }
}
