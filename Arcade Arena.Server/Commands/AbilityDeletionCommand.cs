using Arcade_Arena.Library;
using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Server.Commands
{
    class AbilityDeletionCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc,
            PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players, List<AbilityOutline> abilities)
        {
            managerLogger.AddLogMessage("Deletion", "Received ability to delete");
            string username = inc.ReadString();
            byte ID = inc.ReadByte();

            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].ID == ID && abilities[i].Username == username)
                {
                    abilities.RemoveAt(i);
                    i--;

                    managerLogger.AddLogMessage("Deletion", "Sending ability to delete to clients");
                    var outmsg = server.NetServer.CreateMessage();
                    outmsg.Write((byte)PacketType.AbilityDelete);
                    outmsg.Write(username);
                    outmsg.Write(ID);

                    server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
                }
            }
        }
    }
}
