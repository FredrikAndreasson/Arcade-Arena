using System.Linq;

namespace Arcade_Arena.Managers
{
    class PlayerManager
    {
        private NetworkManager networkManager;
        public Character clientPlayer;


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
        }
    }
}
