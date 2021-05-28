using Arcade_Arena.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    public abstract class Ability : DynamicObject
    {
        public byte ID;

        protected bool isDead;
        protected SpriteAnimation currentAnimation;

        public Ability(Vector2 position, float speed, double direction, sbyte damage) : base(position, speed, direction)
        {
            isDead = false;
            this.Damage = damage;
        }

        public string Username { get; set; }
        public AbilityOutline.AbilityType Type { get; set; }
        public bool IsDead => isDead;
        public double Direction => direction;
        public SpriteAnimation CurrentAnimation => currentAnimation;
        public sbyte Damage;


        public void Kill()
        {
            isDead = true;
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
