using Arcade_Arena.Library;
using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System.Collections.Generic;

namespace Arcade_Arena.Server.Commands
{
    class AllPlayersCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc,
            PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players, List<AbilityOutline> abilities)
        {
            managerLogger.AddLogMessage("Server", "Sending full player list");
            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.AllPlayers);
            outmsg.Write(players.Count);
            foreach (var p in players)
            {
                outmsg.Write(p.Player.Username);
                outmsg.Write(p.Player.XPosition);
                outmsg.Write(p.Player.YPosition);
                outmsg.Write(p.Player.Animation.XRecPos);
                outmsg.Write(p.Player.Animation.YRecPos);
                outmsg.Write(p.Player.Animation.Height);
                outmsg.Write(p.Player.Animation.Width);
                outmsg.Write(p.Player.intersectingLava);
            }
            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
