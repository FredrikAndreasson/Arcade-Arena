﻿using System.Linq;

namespace Arcade_Arena.Managers
{
    class PlayerManager
    {
        private NetworkManager networkManager;
        private Character clientPlayer;


        public PlayerManager(NetworkManager networkManager, Character Player)
        {
            this.networkManager = networkManager;
           
            this.clientPlayer = Player;
        }

        public void UpdatePlayer()
        {
            if (networkManager.Players.Any(p => p.Username == networkManager.Username))
            {
                var player = networkManager.Players.FirstOrDefault(p => p.Username == networkManager.Username);

                //position
                player.XPosition = (int)clientPlayer.position.X;
                player.YPosition = (int)clientPlayer.position.Y;

                //source rectangle
                player.Animation.XRecPos = clientPlayer.CurrentAnimation.Source.X;
                player.Animation.YRecPos = clientPlayer.CurrentAnimation.Source.Y;
                player.Animation.Height = clientPlayer.CurrentAnimation.Source.Height;
                player.Animation.Width = clientPlayer.CurrentAnimation.Source.Width;

                

                //add more code for updating player later...
            }
        }
    }
}