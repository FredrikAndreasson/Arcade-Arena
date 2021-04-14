using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Server.Commands
{
    class AllPlayersCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players)
        {
            managerLogger.AddLogMessage("Server", "Sending full player list");
            var outmessage = server.NetServer.CreateMessage();
            outmessage.Write((byte)PacketType.AllPlayers);
            outmessage.Write(players.Count);
            foreach (var p in players)
            {
                outmessage.WriteAllProperties(p.Player);
            }
            server.NetServer.SendToAll(outmessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
