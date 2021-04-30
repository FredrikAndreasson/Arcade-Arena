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
    class AbilityCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players)
        {
            managerLogger.AddLogMessage("Server", "Received ability");
            var name = inc.ReadString();
            playerAndConnection = players.FirstOrDefault(p => p.Player.Username == name);
            if (playerAndConnection == null)
            {
                managerLogger.AddLogMessage("Server", string.Format("Didn't find player associated with that name: {0}", name));
                return;
            }
            Byte ID = inc.ReadByte();

            //this ability will be sent out to all the clients 
            var outAbility = new AbilityOutline();

            //Check if the ability already exists
            var existingAbility = playerAndConnection.Player.abilities.FirstOrDefault(a => a.UserName == name);
            if ( existingAbility == null || existingAbility.ID != ID) // if it doesnt exist go ahead and create a new one
            {
                var ability = new AbilityOutline();
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
                outAbility = ability;
            }
            else // if it did exist go ahead and update it
            {
                existingAbility.Type = (AbilityOutline.AbilityType)inc.ReadByte();
                existingAbility.XPosition = inc.ReadInt32();
                existingAbility.YPosition = inc.ReadInt32();
                existingAbility.Animation.XRecPos = inc.ReadInt32();
                existingAbility.Animation.YRecPos = inc.ReadInt32();
                existingAbility.Animation.Width = inc.ReadInt32();
                existingAbility.Animation.Height = inc.ReadInt32();
                outAbility = existingAbility;
            }



            //Send ability to all the clients...
            
            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.Ability);
            outmsg.Write(outAbility.UserName);
            outmsg.Write(outAbility.ID);
            outmsg.Write((byte)outAbility.Type);
            outmsg.Write(outAbility.XPosition);
            outmsg.Write(outAbility.YPosition);
            outmsg.Write(outAbility.Animation.XRecPos);
            outmsg.Write(outAbility.Animation.YRecPos);
            outmsg.Write(outAbility.Animation.Width);
            outmsg.Write(outAbility.Animation.Height);

            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
