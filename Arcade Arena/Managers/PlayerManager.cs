using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Managers
{
    class PlayerManager
    {
        private NetworkManager networkManager;


        public PlayerManager(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }

        public void UpdatePlayer(Vector2 playerPosition)
        {
            if (networkManager.Players.Any(p => p.Username == networkManager.Username))
            {
                var player = networkManager.Players.FirstOrDefault(p => p.Username == networkManager.Username);

                player.XPosition = (int)playerPosition.X;
                player.YPosition = (int)playerPosition.Y;

                //add more code that updated player in the future;
            }
        }
    }
}
