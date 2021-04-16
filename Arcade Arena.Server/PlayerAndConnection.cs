using Arcade_Arena.Library;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Server
{
    class PlayerAndConnection
    {
        public Player Player{ get; set; }

        public NetConnection Connection { get; set; }

        public PlayerAndConnection(Player player, NetConnection connection)
        {
            Player = player;
            Connection = connection;
        }
    }
}
