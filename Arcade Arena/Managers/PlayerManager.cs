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
                //
                

                //add more code for updating player later...
            }

            if (clientPlayer.IsDead && clientPlayer.LastToDamage != "")
            {
                networkManager.SendPlayerScore(clientPlayer.LastToDamage);
                clientPlayer.LastToDamage = "";
            }


            PlayerCollision();
        }

        private void PlayerCollision()
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
