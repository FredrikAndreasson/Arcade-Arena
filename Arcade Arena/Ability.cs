﻿using Arcade_Arena.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    public abstract class Ability
    {

        public Vector2 position;
        public byte ID;

        protected bool isDead;
        protected SpriteAnimation currentAnimation;
        protected double direction;

        public Ability()
        {
            isDead = false;
        }

        public string Username { get; set; }
        public AbilityOutline.AbilityType Type { get; set; }
        public bool IsDead => isDead;
        public double Direction => direction;
        public SpriteAnimation CurrentAnimation => currentAnimation;


        public void Kill()
        {
            isDead = true;
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
