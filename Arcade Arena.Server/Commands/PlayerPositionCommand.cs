using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System.Collections.Generic;

namespace Arcade_Arena.Server.Commands
{
    class PlayerPositionCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players)
        {
            managerLogger.AddLogMessage("Server", "Sending out new player position and animation");
            var outmessage = server.NetServer.CreateMessage();
            outmessage.Write((byte)PacketType.PlayerPosition);
            outmessage.Write(playerAndConnection.Player.Username);
            outmessage.Write(playerAndConnection.Player.XPosition);
            outmessage.Write(playerAndConnection.Player.YPosition);
            outmessage.Write(playerAndConnection.Player.Animation.XRecPos);
            outmessage.Write(playerAndConnection.Player.Animation.YRecPos);
            outmessage.Write(playerAndConnection.Player.Animation.Height);
            outmessage.Write(playerAndConnection.Player.Animation.Width);
            outmessage.Write(playerAndConnection.Player.Health);
            outmessage.Write(playerAndConnection.Player.intersectingLava);
            server.NetServer.SendToAll(outmessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
