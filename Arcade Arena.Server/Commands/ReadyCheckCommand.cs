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
    class ReadyCheckCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc,
            PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players, List<AbilityOutline> abilities)
        {
            string name = inc.ReadString();
            bool ready = inc.ReadBoolean();

            managerLogger.AddLogMessage("Ready", "Recieved ready check");

            var player = players.FirstOrDefault(p => p.Player.Username == name);
            player.Player.Ready = ready;

            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.ReadyCheck);
            outmsg.Write(name);
            outmsg.Write(ready);
            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered);

            server.NetServer.FlushSendQueue();

            bool allReady =  true;
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].Player.Ready)
                {
                    allReady = false;
                }
            }
            if (allReady)
            {
                RandomSeedCommand randomSeedCommand = new RandomSeedCommand();
                randomSeedCommand.Run(managerLogger, server, inc, playerAndConnection, players, abilities);
            }
        }
    }
}
