using Arcade_Arena.Server.Commands;
using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System.Collections.Generic;
using System.Linq;
namespace Arcade_Arena.Server
{
    class InputCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players)
        {
            managerLogger.AddLogMessage("Server", "Recieved new input");
            var name = inc.ReadString();
            playerAndConnection = players.FirstOrDefault(p => p.Player.Username == name);
            if (playerAndConnection == null)
            {
                managerLogger.AddLogMessage("Server", string.Format("Didn't find player with name {0}", name));
                return;
            }
            playerAndConnection.Player.XPosition = inc.ReadInt32();
            playerAndConnection.Player.YPosition = inc.ReadInt32();
            playerAndConnection.Player.Animation.XRecPos = inc.ReadInt32();
            playerAndConnection.Player.Animation.YRecPos = inc.ReadInt32();
            playerAndConnection.Player.Animation.Height = inc.ReadInt32();
            playerAndConnection.Player.Animation.Width = inc.ReadInt32();
            playerAndConnection.Player.Health = inc.ReadSByte();
            playerAndConnection.Player.intersectingLava = inc.ReadBoolean();

            if (playerAndConnection == null)
            {
                managerLogger.AddLogMessage("Server", string.Format("Didn't find player with name {0}", name));
                return;
            }




            var command = new PlayerPositionCommand();
            command.Run(managerLogger, server, inc, playerAndConnection, players);
        }
    }
    
}

