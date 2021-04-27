using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System.Collections.Generic;

namespace Arcade_Arena.Server.Commands
{
    class AllPlayersCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players)
        {
            managerLogger.AddLogMessage("Server", "Sending full player list");
            var outmessage = server.NetServer.CreateMessage();
            outmessage.Write((byte)PacketType.AllPlayers);
            outmessage.Write(players.Count);
            foreach (var p in players)
            {
                outmessage.Write(p.Player.Username);
                outmessage.Write(p.Player.XPosition);
                outmessage.Write(p.Player.YPosition);
                outmessage.Write(p.Player.Animation.XRecPos);
                outmessage.Write(p.Player.Animation.YRecPos);
                outmessage.Write(p.Player.Animation.Height);
                outmessage.Write(p.Player.Animation.Width);
            }
            server.NetServer.SendToAll(outmessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
