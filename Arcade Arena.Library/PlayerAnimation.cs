using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Library
{
    public class PlayerAnimation
    {
        public int XRecPos { get; set; }
        public int YRecPos { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public PlayerAnimation()
        {
            Width = 14;
            Height = 20;
        }
    }
}
