using Arcade_Arena.Library;
using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Arcade_Arena.Library.Player;

namespace Arcade_Arena.Server.Commands
{
    class ClassChangeCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc,
            PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players, List<AbilityOutline> abilities)
        {
            managerLogger.AddLogMessage("Class Change", "Recieved a class change");
            string name = inc.ReadString();

            var player = players.FirstOrDefault(p => p.Player.Username == name);
            if (player == null)
            {
                managerLogger.AddLogMessage("Class Change", "Didnt find a player associated with that name");
            }
            player.Player.Type = (ClassType)inc.ReadByte();
            managerLogger.AddLogMessage("Class Change", "Class type succesfully changed");

            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.ClassChange);
            outmsg.Write(name);
            outmsg.Write((byte)player.Player.Type);
            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
