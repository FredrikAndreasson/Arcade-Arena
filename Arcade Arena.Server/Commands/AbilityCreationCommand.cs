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
    class AbilityCreationCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc,
            PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players, List<AbilityOutline> abilities)
        {
            managerLogger.AddLogMessage("Create", "Received ability");
            var name = inc.ReadString();
            Byte ID = inc.ReadByte();

            var ability = CreateAbility(name, ID, inc);

            abilities.Add(ability);

            //Send ability to all the clients...

            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.AbilityCreate);
            outmsg.Write(ability.Username);
            outmsg.Write(ability.ID);
            outmsg.Write((byte)ability.Type);
            outmsg.Write(ability.XPosition);
            outmsg.Write(ability.YPosition);
            outmsg.Write(ability.Animation.XRecPos);
            outmsg.Write(ability.Animation.YRecPos);
            outmsg.Write(ability.Animation.Width);
            outmsg.Write(ability.Animation.Height);

            managerLogger.AddLogMessage("Create", "Sending new ability to clients");

            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }

        private AbilityOutline CreateAbility(string name, byte ID, NetIncomingMessage inc)
        {
            var ability = new AbilityOutline();

            ability.Username = name;
            ability.ID = ID;
            ability.Type = (AbilityOutline.AbilityType)inc.ReadByte();
            ability.XPosition = inc.ReadInt32();
            ability.YPosition = inc.ReadInt32();
            ability.Animation.XRecPos = inc.ReadInt32();
            ability.Animation.YRecPos = inc.ReadInt32();
            ability.Animation.Width = inc.ReadInt32();
            ability.Animation.Height = inc.ReadInt32();

            return ability;
        }
    }
}
