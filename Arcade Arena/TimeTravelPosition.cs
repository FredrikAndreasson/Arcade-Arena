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
    public class TimeTravelPosition
    {
        public sbyte Health { get; set; }
        public Vector2 Position { get; set; }

        public TimeTravelPosition(sbyte health, Vector2 position)
        {
            Health = health;
            Position = position;
        }
    }
}
