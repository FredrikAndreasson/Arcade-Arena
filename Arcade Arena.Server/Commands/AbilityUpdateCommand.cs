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
    class AbilityUpdateCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players)
        {
            managerLogger.AddLogMessage("Update", "Received ability");
            var nmbrOfAbilities = inc.ReadInt32();
            for (int i = 0; i < nmbrOfAbilities; i++)
            {
                var name = inc.ReadString();
                playerAndConnection = players.FirstOrDefault(p => p.Player.Username == name);
                if (playerAndConnection == null)
                {
                    managerLogger.AddLogMessage("Update", string.Format("Didn't find player associated with that name: {0}", name));
                    return;
                }

                Byte ID = inc.ReadByte();

                if (playerAndConnection.Player.abilities.FirstOrDefault(a => a.UserName == name) == null)
                {
                    return;
                }
                var ability = ReadAbility(inc, playerAndConnection, ID, name);

                SendAbility(server, ability, managerLogger);
            }
        }
        private AbilityOutline ReadAbility(NetIncomingMessage inc, PlayerAndConnection playerAndConnection, byte ID, string name)
        {
            var ability = playerAndConnection.Player.abilities.FirstOrDefault(a => a.UserName == name);
            

            if (ability.ID == ID)
            {
                ability.XPosition = inc.ReadInt32();
                ability.YPosition = inc.ReadInt32();
                ability.Animation.XRecPos = inc.ReadInt32();
                ability.Animation.YRecPos = inc.ReadInt32();
                ability.Animation.Width = inc.ReadInt32();
                ability.Animation.Height = inc.ReadInt32();
            }
            return ability;
        }

        private void SendAbility(Server server, AbilityOutline ability, ManagerLogger managerLogger)
        {
            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.AbilityUpdate);
            outmsg.Write(ability.UserName);
            outmsg.Write(ability.ID);
            outmsg.Write(ability.XPosition);
            outmsg.Write(ability.YPosition);
            outmsg.Write(ability.Animation.XRecPos);
            outmsg.Write(ability.Animation.YRecPos);
            outmsg.Write(ability.Animation.Width);
            outmsg.Write(ability.Animation.Height);

            managerLogger.AddLogMessage("Update", "Sending ability update to clients");

            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
