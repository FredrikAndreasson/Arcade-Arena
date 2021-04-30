using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcade_Arena
{
    class WizardWandProjectile : Projectile
    {
        public WizardWandProjectile(int damage, double timer, Vector2 position, Texture2D texture, float speed, double direction) : base(damage, timer, position, texture, speed, direction)
        {

        }
    }
}
