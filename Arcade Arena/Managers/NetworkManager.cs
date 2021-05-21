using Lidgren.Network;
using System;
using System.Collections.Generic;
using Arcade_Arena.Library;
using System.Linq;
using System.Diagnostics;
using Arcade_Arena.Classes;

namespace Arcade_Arena.Managers
{
    class NetworkManager
    {
        private int timer = 100;
        private bool isRetrieved = false;


        private NetClient client;
        public List<Player> Players { get; set; }

        public List<AbilityOutline> ServerAbilities { get; set; }

        public string Username { get; set; }

        public bool Active { get; set; }

        private Player.ClassType classType;

        public NetworkManager(Character playerCharacter)
        {
            Players = new List<Player>();
            ServerAbilities = new List<AbilityOutline>();

            if(playerCharacter is Wizard)
            {
                classType = Player.ClassType.Wizard;
            }
            else if(playerCharacter is Ogre)
            {
                classType = Player.ClassType.Ogre;
            }
            else if(playerCharacter is Huntress)
            {
                classType = Player.ClassType.Huntress;
            }
            else if(playerCharacter is TimeTraveler)
            {
                classType = Player.ClassType.TimeTraveler;
            }
            else if (playerCharacter is Knight)
            {
                classType = Player.ClassType.Knight;
            }
            //else if (playerCharacter is Assassin)
            //{
            //    classType = Player.ClassType.Assassin;
            //}

        }
       public NetConnectionStatus Status => client.ConnectionStatus;

        public bool Start()
        {
            var random = new Random();

            client = new NetClient(new NetPeerConfiguration("networkGame"));
            client.Start();

            Username = "name_" + random.Next(0, 100);

            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.Login);
            outmsg.Write(Username);
            outmsg.Write((byte)classType);
            //client.Connect("85.228.136.154", 14241, outmsg);
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
                outmsg.Write(player.Health);
                outmsg.Write(player.IntersectingLava);
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

                case PacketType.AbilityDelete:
                    ReadAbilityToDelete(inc);
                    break;
                
                case PacketType.AbilityUpdate:
                    RecieveAbilityUpdate(inc);
                    break;
                case PacketType.Score:
                    RecieveScore(inc);
                    break;
                case PacketType.Login:
                    RecieveLogin(inc);
                    break;

                    


                default:
                    break;
            }
        }


        private void RecieveLogin(NetIncomingMessage inc)
        {
            
        }

        private void RecieveScore(NetIncomingMessage inc)
        {
            string name = inc.ReadString();
            sbyte score = inc.ReadSByte();

            var player = Players.FirstOrDefault(p => p.Username == name);
            if (player != null) player.Score = score;
        }

        public void SendPlayerScore(string Username)
        {
            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.Score);
            outmsg.Write(Username);
            client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
        public void SendAbility(Ability ability, byte ID)
        {
            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.AbilityCreate);
            outmsg.Write(Username);
            outmsg.Write(ID);
            outmsg.Write((byte)(ability.Type));
            outmsg.Write((short)ability.Position.X);
            outmsg.Write((short)ability.Position.Y);
            outmsg.Write(ability.Direction);

            client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        }
        public void SendAbilityUpdate(Ability ability)
        {
            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.AbilityUpdate);
            outmsg.Write(ability.Username);
            outmsg.Write(ability.ID);
            outmsg.Write((short)ability.Position.X);
            outmsg.Write((short)ability.Position.Y);
            outmsg.Write((short)ability.CurrentAnimation.Source.X);
            outmsg.Write((short)ability.CurrentAnimation.Source.Y);
            outmsg.Write((short)ability.CurrentAnimation.Source.Width);
            outmsg.Write((short)ability.CurrentAnimation.Source.Height);

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


        public void DeleteProjectile(byte ID, string ownerUsername)
        {

            DeleteAbility(ID, ownerUsername);

            var outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketType.AbilityDelete);
            outmsg.Write(ownerUsername);
            outmsg.Write(ID);

            client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
        }



        private void DeleteAbility(byte ID, string username)
        {

            for (int i = 0; i < ServerAbilities.Count; i++)
            {
                if (ServerAbilities[i].ID == ID && ServerAbilities[i].Username == username)
                {
                    ServerAbilities.RemoveAt(i);
                    i--;
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
            if (!ServerAbilities.Any(a => a.ID == ID && a.Username == Username))
            {
                var newAbility = new AbilityOutline();
                newAbility.Username = name;
                newAbility.ID = ID;
                newAbility.Type = (AbilityOutline.AbilityType)inc.ReadByte();
                newAbility.XPosition = inc.ReadInt16();
                newAbility.YPosition = inc.ReadInt16();
                newAbility.Direction = inc.ReadDouble();

                ServerAbilities.Add(newAbility);
            }
        }

        public void RecieveAbilityUpdate(NetIncomingMessage inc)
        {

            var name = inc.ReadString();
            byte ID = inc.ReadByte();
            
            var oldAbility = ServerAbilities.FirstOrDefault(a => a.ID == ID && a.Username == name);
            if (oldAbility != null)
            {
                oldAbility.Username = name;
                oldAbility.ID = ID;
                oldAbility.XPosition = inc.ReadInt16();
                oldAbility.YPosition = inc.ReadInt16();
                oldAbility.Animation.XRecPos = inc.ReadInt16();
                oldAbility.Animation.YRecPos = inc.ReadInt16();
                oldAbility.Animation.Width = inc.ReadInt16();
                oldAbility.Animation.Height = inc.ReadInt16();
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
            Level.lava.ShrinkPlatform(inc.ReadInt16());
        }
        

        private void ReadPlayer(NetIncomingMessage inc)
        {
            var name = inc.ReadString();
            if (Players.Any(p => p.Username == name))
            {
                
                var oldPlayer = Players.FirstOrDefault(p => p.Username == name);
                oldPlayer.XPosition = inc.ReadInt16();
                oldPlayer.YPosition = inc.ReadInt16();
                oldPlayer.Animation.XRecPos = inc.ReadInt16();
                oldPlayer.Animation.YRecPos = inc.ReadInt16();
                oldPlayer.Animation.Height = inc.ReadInt16();
                oldPlayer.Animation.Width = inc.ReadInt16();
                oldPlayer.Health = inc.ReadSByte();
                oldPlayer.IntersectingLava = inc.ReadBoolean();
                oldPlayer.Type = (Player.ClassType)inc.ReadByte();
            }
            else
            {
                var player = new Player();
                player.Username = name;
                player.XPosition = inc.ReadInt16();
                player.YPosition = inc.ReadInt16();
                player.Animation.XRecPos = inc.ReadInt16();
                player.Animation.YRecPos = inc.ReadInt16();
                player.Animation.Height = inc.ReadInt16();
                player.Animation.Width = inc.ReadInt16();
                player.Health = inc.ReadSByte();
                player.IntersectingLava = inc.ReadBoolean();
                player.Type = (Player.ClassType)inc.ReadByte();
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
