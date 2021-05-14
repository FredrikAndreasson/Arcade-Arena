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
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc,
            PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players, List<AbilityOutline> abilities)
        {
            managerLogger.AddLogMessage("Update", "Received ability");

            var name = inc.ReadString();
            Byte ID = inc.ReadByte();
            var ability = ReadAbility(inc, abilities, ID, name);
            if (ability != null)
            {
                SendAbility(server, ability, managerLogger);
            }
        }
        private AbilityOutline ReadAbility(NetIncomingMessage inc, List<AbilityOutline> abilities, byte ID, string name)
        {
            var ability = abilities.FirstOrDefault(a => a.Username == name && a.ID == ID);
            if (ability != null)
            {
                ability.XPosition = inc.ReadInt16();
                ability.YPosition = inc.ReadInt16();
                ability.Animation.XRecPos = inc.ReadInt16();
                ability.Animation.YRecPos = inc.ReadInt16();
                ability.Animation.Width = inc.ReadInt16();
                ability.Animation.Height = inc.ReadInt16();
            }
            return ability;
        }

        private void SendAbility(Server server, AbilityOutline ability, ManagerLogger managerLogger)
        {
            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.AbilityUpdate);
            outmsg.Write(ability.Username);
            outmsg.Write(ability.ID);
            outmsg.Write(ability.XPosition);
            outmsg.Write(ability.YPosition);
            outmsg.Write(ability.Animation.XRecPos);
            outmsg.Write(ability.Animation.YRecPos);
            outmsg.Write(ability.Animation.Width);
            outmsg.Write(ability.Animation.Height);

            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
