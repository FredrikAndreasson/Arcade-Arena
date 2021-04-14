using Arcade_Arena.Server.Commands;
using Arcade_Arena.Server.Managers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            var player = playerAndConnection.Player;

            var command = new PlayerPositionCommand();
            command.Run(managerLogger, server, inc, playerAndConnection, players);
        }
    }
    
}

