using Arcade_Arena.Library;
using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace Arcade_Arena.Server.Commands
{
    class LoginCommand :  ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc,
            PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players, List<AbilityOutline> abilities)
        {
            managerLogger.AddLogMessage("Server", "New connection...");
            var data = inc.ReadByte();
            if (data == (byte)PacketType.Login)
            {
                managerLogger.AddLogMessage("Server", "...connection accepted");
                playerAndConnection = CreatePlayer(inc, players);
                inc.SenderConnection.Approve();
                var outmsg = server.NetServer.CreateMessage();
                outmsg.Write((byte)PacketType.Login);
                outmsg.Write(true);
                outmsg.Write(players.Count);
                for (int n = 0; n < players.Count - 1; n++)
                {
                    outmsg.WriteAllProperties(players[n].Player);

                }
                server.NetServer.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                var command = new PlayerPositionCommand();
                command.Run(managerLogger, server, inc, playerAndConnection, players, abilities);

                server.SendNewPlayerEvent(playerAndConnection.Player.Username);
            }
            else
            {
                inc.SenderConnection.Deny("Didn't send correct information.");
            }
        }
        private PlayerAndConnection CreatePlayer(NetIncomingMessage inc, List<PlayerAndConnection> players)
        {
            var random = new Random();
            var player = new Player
            {
                Username = inc.ReadString(),
                Type = (Player.ClassType)inc.ReadByte(),
                XPosition = random.Next(0, 750), 
                YPosition = random.Next(0, 400)
                
            };

            var playerAndConnection = new PlayerAndConnection(player, inc.SenderConnection);
            players.Add(playerAndConnection);
            return playerAndConnection;
        }
    }
}
