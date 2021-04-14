using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Library
{
    public class Player
    {
        public enum ClassType 
        { 
            Wizard,
            Ogre,
            Huntress,
            TimeTraveler,
            Assassin,
            Knight
        }
        public ClassType Type;
        public Rectangle SourceRectangle { get; set; } //By combining the enum ClassType and the Source rectangle we can calculate what frame to draw from the spritesheets
        public string Username { get; set; }
        public Vector2 Position;

        public Player(string username, int xPosition, int yPosition)
        {
            this.Username = username;
            this.Position.X = xPosition;
            this.Position.Y = yPosition;
        }

        public Player() { }
    }
}
