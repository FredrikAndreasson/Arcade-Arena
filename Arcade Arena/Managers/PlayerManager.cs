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


        public PlayerManager(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }

        public bool IsFirstPlayerHit { get; set; }

        public Level Level { get { return level; } set { level = value; } }
        public Character ClientPlayer { get { return clientPlayer; } set { clientPlayer = value; } }

        public void UpdatePlayer()
        {
            if (networkManager.Players.Any(p => p.Username == networkManager.Username))
            {
                var player = networkManager.Players.FirstOrDefault(p => p.Username == networkManager.Username);

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

                player.OrbiterRotation = clientPlayer.OrbiterRotation;
            }


            CheckPlayerCollision();

            if (IsFirstPlayerHit)
            {
                LocalPlayerWin();
            }
            
        }

        public bool GameOverCheck()
        {
            if (networkManager.Players.Count == 0)
            {
                return false;
            }
            foreach (Player player in networkManager.Players)
            {
                if (player.Score >= 3)
                {
                    return true;
                }
            }
            return false;
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
