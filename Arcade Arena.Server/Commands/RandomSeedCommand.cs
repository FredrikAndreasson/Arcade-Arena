using Arcade_Arena.Library;
using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Server.Commands
{
    class RandomSeedCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc,
            PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players, List<AbilityOutline> abilities)
        {
            int seed = Server.random.Next(1000);

            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.Seed);
            outmsg.Write(seed);
            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);

            server.NetServer.FlushSendQueue();
        }
    }
}
