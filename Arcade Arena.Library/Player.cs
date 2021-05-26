
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
        public ClassType Type { get; set; }

        public string Username { get; set; }

        public Animation Animation { get; set; }
        public short XPosition { get; set; }
        public short YPosition { get; set; }

        public sbyte Health { get; set; }

        public sbyte Score { get; set; }

        public bool isHit { get; set; }

        public bool Ready { get; set; }

        public bool IntersectingLava { get; set; }


        public Player(string username, short xPosition, short yPosition)
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
