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

        public string Username { get; set; }

        public PlayerAnimation Animation { get; set; }
        //Vector2 pos
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public Player(string username, int xPosition, int yPosition)
        {
            this.Username = username;
            this.XPosition = xPosition;
            this.YPosition = yPosition;

            Animation = new PlayerAnimation();
        }

        public Player() 
        {
            Animation = new PlayerAnimation();
        }
    }
}
