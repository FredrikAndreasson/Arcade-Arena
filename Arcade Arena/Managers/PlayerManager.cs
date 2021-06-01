using Arcade_Arena.Library;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Arcade_Arena.Managers
{
    class PlayerManager
    {
        private NetworkManager networkManager;
        private Level level; 
        public Character clientPlayer;


        public PlayerManager(NetworkManager networkManager, Character Player, Level level)
        {
            this.networkManager = networkManager;
            this.level = level;
           
            this.clientPlayer = Player;
        }

        public bool IsFirstPlayerHit { get; set; }

        public Level Level => level;

        public void UpdatePlayer()
        {
            if (networkManager.Players.Any(p => p.Username == networkManager.Username))
            {
                var player = networkManager.Players.FirstOrDefault(p => p.Username == networkManager.Username);

                //position
                player.XPosition = (short)clientPlayer.Position.X;
                player.YPosition = (short)clientPlayer.Position.Y;

                //source rectangle
                player.Animation.XRecPos = (short)clientPlayer.CurrentAnimation.Source.X;
                player.Animation.YRecPos = (short)clientPlayer.CurrentAnimation.Source.Y;
                player.Animation.Height = (short)clientPlayer.CurrentAnimation.Source.Height;
                player.Animation.Width = (short)clientPlayer.CurrentAnimation.Source.Width;
                player.Health = clientPlayer.Health;
                player.IntersectingLava = clientPlayer.IntersectingLava;
                player.isHit = clientPlayer.isHit;
                //

                player.OrbiterRotation = clientPlayer.OrbiterRotation;

                //add more code for updating player later...
            }


            CheckPlayerCollision();

            if (IsFirstPlayerHit)
            {
                LocalPlayerWin();
            }
            
        }

        public void LocalPlayerWin()
        {
            if (networkManager.Players.Count == 0)
            {
                return;
            }
            foreach (Player player in networkManager.Players)
            {
                if (player.Username != networkManager.Username)
                {
                    if (player.Health > 0)
                    {
                        return;
                    }
                }
            }
            networkManager.SendPlayerScore(networkManager.Username);
            IsFirstPlayerHit = false;
        }

        private void CheckPlayerCollision()
        {
            foreach (Obstacle obstacle in level.Obstacles)
            {
                if (obstacle.HitBox().Intersects(clientPlayer.shadow.Hitbox))
                {
                    clientPlayer.Blocked = true;
                    return;
                }
            }
            clientPlayer.Blocked = false;
        }
    }
}
