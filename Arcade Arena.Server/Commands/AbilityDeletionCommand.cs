using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Server.Commands
{
    class AbilityDeletionCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players)
        {
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

                        var outmsg = server.NetServer.CreateMessage();
                        outmsg.Write((byte)PacketType.DeleteAbility);
                        outmsg.Write(username);
                        outmsg.Write(ID);

                        server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
                    }
                }
            }
        }
    }
}
