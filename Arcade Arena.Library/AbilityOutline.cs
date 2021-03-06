using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Library
{
    public class AbilityOutline
    {
        public enum AbilityType
        {
            Projectile,
            MeeleAttack,
            AbilityOne,
            AbilityTwo

        }
        public AbilityType Type { get; set; }
        public Byte ID { get; set; }
        public string Username { get; set; }
        public short XPosition { get; set; }
        public short YPosition { get; set; }
        public double Direction { get; set; }
        public sbyte Damage { get; set; }

        public Animation Animation { get; set; }

        public AbilityOutline()
        {
            Animation = new Animation();
        }
    }
}
