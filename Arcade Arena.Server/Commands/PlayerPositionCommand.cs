using Arcade_Arena.Library;
using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System.Collections.Generic;

namespace Arcade_Arena.Server.Commands
{
    class PlayerPositionCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc,
            PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players, List<AbilityOutline> abilities)
        {
            //managerLogger.AddLogMessage("Server", "Sending out new player position and animation");
            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.PlayerPosition);
            outmsg.Write(playerAndConnection.Player.Username);
            outmsg.Write(playerAndConnection.Player.XPosition);
            outmsg.Write(playerAndConnection.Player.YPosition);
            outmsg.Write(playerAndConnection.Player.Animation.XRecPos);
            outmsg.Write(playerAndConnection.Player.Animation.YRecPos);
            outmsg.Write(playerAndConnection.Player.Animation.Height);
            outmsg.Write(playerAndConnection.Player.Animation.Width);
            outmsg.Write(playerAndConnection.Player.Health);
            outmsg.Write(playerAndConnection.Player.IntersectingLava);
            outmsg.Write((byte)playerAndConnection.Player.Type);
            outmsg.Write(playerAndConnection.Player.OrbiterRotation);
            managerLogger.AddLogMessage("Server", playerAndConnection.Player.Type.ToString());

            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
