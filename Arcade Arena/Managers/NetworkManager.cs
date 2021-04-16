using Lidgren.Network;
using System;
using System.Collections.Generic;
using Arcade_Arena.Library;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Arcade_Arena.Managers
{
    class NetworkManager
    {
        private int timer = 100;
        private bool isRetrieved = false;


        private NetClient client;
        public List<Player> Players { get; set; }

        public string Username { get; set; }

        public bool Active { get; set; }

        public NetworkManager()
        {
            Players = new List<Player>();
        }
       public NetConnectionStatus Status => client.ConnectionStatus;

        public bool Start()
        {
            var random = new Random();

            client = new NetClient(new NetPeerConfiguration("networkGame"));
            client.FlushSendQueue();
            client.Start();

            Username = "name_" + random.Next(0, 100);

            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.Login);
            outmsg.Write(Username);
            client.Connect("localhost", 14241, outmsg);
            return EstablishInfo();
        }

        private bool EstablishInfo()
        {
            var time = DateTime.Now;
            NetIncomingMessage inc;
            while (true)
            {
                

                if (DateTime.Now.Subtract(time).Seconds > 5)
                {
                    return false;
                }

                if ((inc = client.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        if (inc.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            Active = true;
                            return true;
                        }
                        break;
                }
            }
        }

        public void Update()
        {
            if (timer < 0 && !isRetrieved)
            {
                isRetrieved = true;
                var outmessage = client.CreateMessage();
                outmessage.Write((byte)PacketType.AllPlayers);
                client.SendMessage(outmessage, NetDeliveryMethod.ReliableOrdered);
            }else if(!isRetrieved) timer--;

            if (Players.Any(p => p.Username == Username))
            {
                var player = Players.FirstOrDefault(p => p.Username == Username);

                var outmessage = client.CreateMessage();
                outmessage.Write((byte)PacketType.Input);
                outmessage.Write(player.Username);
                outmessage.Write(player.XPosition);
                outmessage.Write(player.YPosition);
                client.SendMessage(outmessage, NetDeliveryMethod.ReliableOrdered);
            }



            NetIncomingMessage inc;
            while ((inc = client.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        Data(inc);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        StatusChanged(inc);
                        break;
                }
            }
        }

        private void StatusChanged(NetIncomingMessage inc)
        {
            switch ((NetConnectionStatus)inc.ReadByte())
            {
                case NetConnectionStatus.Disconnected:
                    Active = false;
                    break;
            }
        }

        private void Data(NetIncomingMessage inc)
        {
            var packageType = (PacketType)inc.ReadByte();
            switch (packageType)
            {
                case PacketType.PlayerPosition:
                    ReadPlayer(inc);
                    break;

                case PacketType.AllPlayers:
                    ReceiveAllPlayers(inc);
                    break;

                case PacketType.Kick:
                    ReceiveKick(inc);
                    break;

                default:
                    break;
            }
        }

        private void ReceiveAllPlayers(NetIncomingMessage inc)
        {
            var count = inc.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                ReadPlayer(inc);
            }
        }

        private void ReadPlayer(NetIncomingMessage inc)
        {
            var player = new Player();
            inc.ReadAllProperties(player);
            if (Players.Any(p => p.Username == player.Username))
            {
                var oldPlayer = Players.FirstOrDefault(p => p.Username == player.Username);
                oldPlayer.XPosition = player.XPosition;
                oldPlayer.YPosition = player.YPosition;
                oldPlayer.Animation = player.Animation;
                oldPlayer.Type = player.Type;
            }
            else
            {
                Players.Add(player);
            }
        }

        private void ReceiveKick(NetIncomingMessage inc)
        {
            var username = inc.ReadString();
            var player = Players.FirstOrDefault(p => p.Username == username);
            if (player != null)
                Players.Remove(player);
        }
    }
}
