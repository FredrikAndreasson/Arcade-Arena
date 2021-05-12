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
    class ScoreCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players, List<AbilityOutline> abilities)
        {
            managerLogger.AddLogMessage("Score", "Recieved score");

            string name = inc.ReadString();

            playerAndConnection = players.FirstOrDefault(p => p.Player.Username == name);
            if (playerAndConnection == null) return;
            playerAndConnection.Player.Score++;

            managerLogger.AddLogMessage("Score", "Sending out score");
            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.Score);
            outmsg.Write(playerAndConnection.Player.Username);
            outmsg.Write(playerAndConnection.Player.Score);
            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
