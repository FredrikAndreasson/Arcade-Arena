using Arcade_Arena.Server.Commands;
using System;

namespace Arcade_Arena.Server
{
    class PacketFactory
    {
        public static ICommand GetCommand(PacketType packetType)
        {
            switch (packetType)
            {
                case PacketType.Login:
                    return new LoginCommand();
                case PacketType.PlayerPosition:
                    return new PlayerPositionCommand();
                case PacketType.CharacterSelect:
                    break;
                case PacketType.AllPlayers:
                    return new AllPlayersCommand();
                case PacketType.Input:
                    return new InputCommand();
                case PacketType.AbilityCreate:
                    return new AbilityCreationCommand();
                case PacketType.AbilityUpdate:
                    return new AbilityUpdateCommand();
                case PacketType.AbilityDelete:
                    return new AbilityDeletionCommand();
                    //case PacketType.Kick:
                    //    return new KickPlayerCommand();
            }
            throw new ArgumentOutOfRangeException("packetType");
        }
    }
}
