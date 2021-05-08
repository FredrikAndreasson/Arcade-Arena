using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Library
{
    public class AbilityOutline
    {
        public enum AbilityType //using vauge names like these lets the clients figure out through the combination of AbilityType and ClassType the type of ability being used
        {
            AbilityOne,
            AbilityTwo
        }
        public AbilityType Type { get; set; }
        public Byte ID { get; set; }
        public string UserName { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public Animation Animation { get; set; }

        public AbilityOutline()
        {
            Animation = new Animation();
        }
    }
}
