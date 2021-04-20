using System;
using System.Collections.Generic;
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
        private NetPeerConfiguration config;
        public NetServer NetServer { get; private set; }

        public DateTime lavaTimer = DateTime.Now;
        public int LavaRadius = 400;


        public Server(ManagerLogger managerLogger)
        {
            this.managerLogger = managerLogger;
            players = new List<PlayerAndConnection>();
            config = new NetPeerConfiguration("networkGame") { Port = 14241 };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            NetServer = new NetServer(config);
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
                        login.Run(managerLogger, this, inc, null, players);
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
            NetOutgoingMessage om = NetServer.CreateMessage();
            LavaRadius -= 5;
            om.Write((byte)PacketType.ShrinkLava);
            om.Write(LavaRadius);
            NetServer.SendToAll(om, NetDeliveryMethod.Unreliable);
            managerLogger.AddLogMessage("Server", $"LAVA UPDATED {lavaTimer}  {DateTime.Now}");
        }

        private void Data(NetIncomingMessage inc)
        {
            foreach (var player in players)
            {
                managerLogger.AddLogMessage("Server", string.Format("player X rec: {0} player Y rec: {1}",player.Player.Animation.XRecPos, player.Player.Animation.YRecPos));
            }
            
            var packetType = (PacketType)inc.ReadByte();
            var command = PacketFactory.GetCommand(packetType);
            command.Run(managerLogger, this, inc, null, players);
        }

        public void SendNewPlayerEvent(string username)
        {
            if (NewPlayer != null)
                NewPlayer(this, new NewPlayerEventArgs(username));
        }
    }
}
