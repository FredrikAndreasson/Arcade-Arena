using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Server.Commands
{
    class PlayerPositionCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players)
        {
            managerLogger.AddLogMessage("Server", "Sending out new player position and animation");
            var outmessage = server.NetServer.CreateMessage();
            outmessage.Write((byte)PacketType.PlayerPosition);
            outmessage.WriteAllProperties(playerAndConnection.Player);
            server.NetServer.SendToAll(outmessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
