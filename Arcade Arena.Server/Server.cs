using System;
using System.Collections.Generic;
using Arcade_Arena.Library;
using Arcade_Arena.Server.Commands;
using Arcade_Arena.Server.Managers;
using Arcade_Arena.Server.MyEventArgs;
using Lidgren.Network;

namespace Arcade_Arena.Server
{
    class Server
    {

        public event EventHandler<NewPlayerEventArgs> NewPlayer;
        private readonly ManagerLogger managerLogger;
        private List<PlayerAndConnection> players;
        private List<AbilityOutline> abilities;
        private NetPeerConfiguration config;
        public NetServer NetServer { get; private set; }

        public DateTime lavaTimer = DateTime.Now.Subtract(TimeSpan.FromSeconds(2));
        public int lavaRadius = 400;

        public static Random random;

        public Server(ManagerLogger managerLogger)
        {
            this.managerLogger = managerLogger;
            players = new List<PlayerAndConnection>();
            abilities = new List<AbilityOutline>();
            config = new NetPeerConfiguration("networkGame") { Port = 14241 };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            NetServer = new NetServer(config);

            random = new Random();
        }

        public void Run()
        {
            NetServer.Start();
            managerLogger.AddLogMessage("Server", "Server Started...");
            while (true)
            {
                NetIncomingMessage inc;
                if ((inc = NetServer.ReadMessage()) == null) continue;
                switch (inc.MessageType)
                {
                   
                    case NetIncomingMessageType.ConnectionApproval:
                        var login = new LoginCommand();
                        login.Run(managerLogger, this, inc, null, players, abilities);
                        break;
                    case NetIncomingMessageType.Data:
                        Data(inc);
                        break;
                }

                if (DateTime.Now.Subtract(lavaTimer).Seconds > 3)
                {
                    SendLavaRadius();
                }
            }
        }

        private void SendLavaRadius()
        {
            lavaTimer = DateTime.Now;
            NetOutgoingMessage outmsg = NetServer.CreateMessage();
            lavaRadius -= 5;
            outmsg.Write((byte)PacketType.ShrinkLava);
            outmsg.Write(lavaRadius);
            NetServer.SendToAll(outmsg, NetDeliveryMethod.Unreliable);
            managerLogger.AddLogMessage("Server", $"LAVA UPDATED {lavaTimer}  {DateTime.Now}");
        }

        private void Data(NetIncomingMessage inc)
        {
            var packetType = (PacketType)inc.ReadByte();
            if (packetType == PacketType.Score) lavaRadius = 400;
            var command = PacketFactory.GetCommand(packetType);
            command.Run(managerLogger, this, inc, null, players, abilities);
        }

        public void SendNewPlayerEvent(string username)
        {
            if (NewPlayer != null)
                NewPlayer(this, new NewPlayerEventArgs(username));
        }
    }
}
