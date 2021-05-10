﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class BearTrap : GameObject
    {
        double timer;
        bool activated = false;
        Huntress owner;

        public BearTrap(int timer, Vector2 position, Texture2D texture) : base(position, texture)
        {
            this.timer = timer;
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
                    activated = true;
                    timer = 4;
                }
            }
        }

        public void Despawn()
        {
            owner.desp
        }
    }
}