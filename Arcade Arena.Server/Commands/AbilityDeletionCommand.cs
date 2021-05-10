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
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players)
        {
            managerLogger.AddLogMessage("Deletion", "Received ability to delete");
            string username = inc.ReadString();
            byte ID = inc.ReadByte();

            var player = players.FirstOrDefault(p => p.Player.Username == username);
            if (player != null)
            {
                for (int i = 0; i < player.Player.abilities.Count; i++)
                {
                    if (player.Player.abilities[i].ID == ID)
                    {
                        player.Player.abilities.RemoveAt(i);
                        i--;

                        managerLogger.AddLogMessage("Deletion", "Sending ability to delete to clients");
                        var outmsg = server.NetServer.CreateMessage();
                        outmsg.Write((byte)PacketType.AbilityDelete);
                        outmsg.Write(username);
                        outmsg.Write(ID);
                        Debug.WriteLine("Delted");

                        server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
                    }
                }
            }
            else
            {
                managerLogger.AddLogMessage("Deletion", "Did not find a player associated with that ability");
            }
        }
    }
}
