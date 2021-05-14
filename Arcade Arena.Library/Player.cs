
using System.Collections.Generic;

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

        public Animation Animation { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public sbyte Health { get; set; }

        public sbyte Score { get; set; }

        public bool isHit { get; set; }

        public bool IntersectingLava { get; set; }


        public Player(string username, int xPosition, int yPosition)
        {
            this.Username = username;
            this.XPosition = xPosition;
            this.YPosition = yPosition;


            Animation = new Animation();
        }

        public Player() 
        {
            Animation = new Animation();
        }
    }
}
