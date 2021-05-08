using Lidgren.Network;
using System;
using System.Collections.Generic;
using Arcade_Arena.Library;
using System.Linq;

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
            outmsg.Write((byte)Player.ClassType.Wizard);
            client.Connect("localhost", 14241, outmsg);
            return EstablishInfo();

            //var outmsg1 = client.CreateMessage();
            //outmsg1.Write(PacketType.ability);
            //outmsg1.Write(hitplayer);
            //outmsg1.Write(Ability);
            //outmsg1.Write(ability.UserName);

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
                var outmsg = client.CreateMessage();
                outmsg.Write((byte)PacketType.AllPlayers);
                client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
            }else if(!isRetrieved) timer--;

            if (Players.Any(p => p.Username == Username))
            {
                var player = Players.FirstOrDefault(p => p.Username == Username);

                var outmsg = client.CreateMessage();
                outmsg.Write((byte)PacketType.Input);
                outmsg.Write(player.Username);
                outmsg.Write(player.XPosition);
                outmsg.Write(player.YPosition);
                outmsg.Write(player.Animation.XRecPos);
                outmsg.Write(player.Animation.YRecPos);
                outmsg.Write(player.Animation.Height);
                outmsg.Write(player.Animation.Width);
                outmsg.Write(player.intersectingLava);
                client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
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

                //case PacketType.Kick:
                //    ReceiveKick(inc);
                //    break;

                case PacketType.ShrinkLava:
                    UpdateLava(inc);
                    break;

                case PacketType.AbilityCreate:
                    RecieveAbilityCreate(inc);
                    break;

                case PacketType.AbilityUpdate:
                    RecieveAbilityUpdate(inc);
                    break;

                case PacketType.AbilityDelete:
                    ReadAbilityToDelete(inc);
                    break;


                default:
                    break;
            }
        }

        public void SendAbility(Ability ability, byte ID)
        {
            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.AbilityCreate);
            outmsg.Write(Username);
            outmsg.Write(ID);
            outmsg.Write((byte)(ability.Type));
            outmsg.Write((int)ability.Position.X);
            outmsg.Write((int)ability.Position.Y);
            outmsg.Write(ability.CurrentAnimation.Source.X);
            outmsg.Write(ability.CurrentAnimation.Source.Y);
            outmsg.Write(ability.CurrentAnimation.Source.Width);
            outmsg.Write(ability.CurrentAnimation.Source.Height);

            client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
        public void SendAbilityUpdate(Ability ability, byte ID)
        {
            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.AbilityUpdate);
            outmsg.Write(Username);
            outmsg.Write(ID);
            outmsg.Write((int)ability.Position.X);
            outmsg.Write((int)ability.Position.Y);
            outmsg.Write(ability.CurrentAnimation.Source.X);
            outmsg.Write(ability.CurrentAnimation.Source.Y);
            outmsg.Write(ability.CurrentAnimation.Source.Width);
            outmsg.Write(ability.CurrentAnimation.Source.Height);

            client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        }

        //Deletes the client ability and then sends a message to the server to also delete that ability
        public void DeleteLocalAbility(byte ID)
        {
            DeleteAbility(ID, Username);

            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.AbilityDelete);
            outmsg.Write(Username);
            outmsg.Write(ID);

            client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        }



        private void DeleteAbility(byte ID, string username)
        {
            var player = Players.FirstOrDefault(p => p.Username == username);
            if (player != null)
            {
                for (int i = 0; i < player.abilities.Count; i++)
                {
                    if (player.abilities[i].ID == ID)
                    {
                        player.abilities.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        private void ReadAbilityToDelete(NetIncomingMessage inc)
        {
            string username = inc.ReadString();
            byte ID = inc.ReadByte();
            DeleteAbility(ID, username);
        }

        private void RecieveAbilityCreate(NetIncomingMessage inc)
        {
            var name = inc.ReadString();
            byte ID = inc.ReadByte();
            if (Players.Any(p => p.Username == name))
            {
                var player = Players.FirstOrDefault(p => p.Username == name);
                if (!player.abilities.Any(a => a.ID == ID))
                {
                    var newAbility = new AbilityOutline();
                    newAbility.UserName = name;
                    newAbility.ID = ID;
                    newAbility.Type = (AbilityOutline.AbilityType)inc.ReadByte();
                    newAbility.XPosition = inc.ReadInt32();
                    newAbility.YPosition = inc.ReadInt32();
                    newAbility.Animation.XRecPos = inc.ReadInt32();
                    newAbility.Animation.YRecPos = inc.ReadInt32();
                    newAbility.Animation.Width = inc.ReadInt32();
                    newAbility.Animation.Height = inc.ReadInt32();

                    player.abilities.Add(newAbility);
                }

            }
        }

        public void RecieveAbilityUpdate(NetIncomingMessage inc)
        {
            var name = inc.ReadString();
            byte ID = inc.ReadByte();
            if (Players.Any(p => p.Username == name))
            {
                var oldPlayer = Players.FirstOrDefault(p => p.Username == name);
                if (oldPlayer.abilities.Any(a => a.ID == ID))
                {
                    var oldAbility = oldPlayer.abilities.FirstOrDefault(a => a.ID == ID);
                    oldAbility.UserName = name;
                    oldAbility.ID = ID;
                    oldAbility.XPosition = inc.ReadInt32();
                    oldAbility.YPosition = inc.ReadInt32();
                    oldAbility.Animation.XRecPos = inc.ReadInt32();
                    oldAbility.Animation.YRecPos = inc.ReadInt32();
                    oldAbility.Animation.Width = inc.ReadInt32();
                    oldAbility.Animation.Height = inc.ReadInt32();
                }
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

        private void UpdateLava(NetIncomingMessage inc)
        {
            FFAArenaState.lava.ShrinkPlatform(inc.ReadInt16());
        }
        

        private void ReadPlayer(NetIncomingMessage inc)
        {
            var name = inc.ReadString();
            if (Players.Any(p => p.Username == name))
            {
                
                var oldPlayer = Players.FirstOrDefault(p => p.Username == name);
                oldPlayer.XPosition = inc.ReadInt32();
                oldPlayer.YPosition = inc.ReadInt32();
                oldPlayer.Animation.XRecPos = inc.ReadInt32();
                oldPlayer.Animation.YRecPos = inc.ReadInt32();
                oldPlayer.Animation.Height = inc.ReadInt32();
                oldPlayer.Animation.Width = inc.ReadInt32();
                oldPlayer.intersectingLava = inc.ReadBoolean();
            }
            else
            {
                var player = new Player();
                player.Username = name;
                player.XPosition = inc.ReadInt32();
                player.YPosition = inc.ReadInt32();
                player.Animation.XRecPos = inc.ReadInt32();
                player.Animation.YRecPos = inc.ReadInt32();
                player.Animation.Height = inc.ReadInt32();
                player.Animation.Width = inc.ReadInt32();
                player.intersectingLava = inc.ReadBoolean();
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
