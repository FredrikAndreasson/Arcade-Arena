using Arcade_Arena.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.GameStates
{
    class LobbyState : GameState
    {
        private NetworkManager networkManager;
        private UserInterfaceManager userInterfaceManager;

        public LobbyState(GameWindow Window, NetworkManager networkManager, ref Character player) : base(Window)
        {
            this.networkManager = networkManager;
            userInterfaceManager = new UserInterfaceManager(Window);

            networkManager.Start();
        }

        public override void Update(GameTime gameTime, ref States state, ref Character player)
        {
            
        }

        public override void Draw(SpriteBatch spritebatch, States state)
        {
            
        }
    }
}
