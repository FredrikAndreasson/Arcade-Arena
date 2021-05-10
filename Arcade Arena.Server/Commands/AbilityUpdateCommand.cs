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

            for (int i = 0; i < abilities.Count; i++)
            {
                managerLogger.AddLogMessage("Update", string.Format("ability user: {0}, ID: {1}, xposition: {2}, yposition: {3}", 
                    abilities[i].Username, abilities[i].ID, abilities[i].XPosition, abilities[i].YPosition));
            }

            SendAbility(server, ability, managerLogger);
        }
        private AbilityOutline ReadAbility(NetIncomingMessage inc, List<AbilityOutline> abilities, byte ID, string name)
        {
            var ability = abilities.FirstOrDefault(a => a.Username == name && a.ID == ID);
            ability.XPosition = inc.ReadInt32();
            ability.YPosition = inc.ReadInt32();
            ability.Animation.XRecPos = inc.ReadInt32();
            ability.Animation.YRecPos = inc.ReadInt32();
            ability.Animation.Width = inc.ReadInt32();
            ability.Animation.Height = inc.ReadInt32();

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
