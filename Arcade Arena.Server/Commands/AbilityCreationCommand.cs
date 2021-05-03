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
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players)
        {
            managerLogger.AddLogMessage("Create", "Received ability");
            var name = inc.ReadString();
            playerAndConnection = players.FirstOrDefault(p => p.Player.Username == name);
            if (playerAndConnection == null)
            {
                managerLogger.AddLogMessage("Create", string.Format("Didn't find player associated with that name: {0}", name));
                return;
            }
            Byte ID = inc.ReadByte();

            //this ability will be sent out to all the clients 
            var ability = new AbilityOutline();

            //Check if the ability already exists
            var existingAbility = playerAndConnection.Player.abilities.FirstOrDefault(a => a.UserName == name);
            if (existingAbility == null || existingAbility.ID != ID) // if it doesnt exist go ahead and create a new one
            {
                ability.UserName = name;
                ability.ID = ID;
                ability.Type = (AbilityOutline.AbilityType)inc.ReadByte();
                ability.XPosition = inc.ReadInt32();
                ability.YPosition = inc.ReadInt32();
                ability.Animation.XRecPos = inc.ReadInt32();
                ability.Animation.YRecPos = inc.ReadInt32();
                ability.Animation.Width = inc.ReadInt32();
                ability.Animation.Height = inc.ReadInt32();

                playerAndConnection.Player.abilities.Add(ability);

                managerLogger.AddLogMessage("Create", "Added new ability");
            }

            //Send ability to all the clients...

            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.AbilityCreate);
            outmsg.Write(ability.UserName);
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
    }
}
