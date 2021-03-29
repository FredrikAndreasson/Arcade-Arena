using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Classes
{
    class Wizard : Character
    {
        public Wizard(Vector2 position, Texture2D texture, float speed, double direction) : base(position, texture, speed, direction)
        {
        }
    }
}
