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
        private PlayerManager playerManager;

        public LobbyState(GameWindow Window, NetworkManager networkManager, ref Character player, PlayerManager playerManager) : base(Window)
        {
            this.networkManager = networkManager;
            userInterfaceManager = new UserInterfaceManager(Window, playerManager);

            networkManager.Start();
        }

        public override void Update(GameTime gameTime, ref States state, ref Character player)
        {
            networkManager.Update();

            if (userInterfaceManager.ReadyCheckRect.Contains(MouseKeyboardManager.MousePosition.ToPoint()) && MouseKeyboardManager.LeftClick)
            {
                userInterfaceManager.Ready = !userInterfaceManager.Ready;
                networkManager.SendReadyTag(userInterfaceManager.Ready);
            }
            bool start = true;
            for (int i = 0; i < networkManager.Players.Count; i++)
            {
                if (!networkManager.Players[i].Ready)
                {
                    start = false;
                    break;
                }
            }
            if (start) state = States.FFA;

            if (userInterfaceManager.CharacterChangeRect.Contains(MouseKeyboardManager.MousePosition.ToPoint()) && MouseKeyboardManager.LeftClick)
            {
                state = States.CharacterSelection;
                userInterfaceManager.Ready = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, States state)
        {
            Game1.graphics.GraphicsDevice.Clear(networkManager.Active ? Color.Green : Color.Red);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            userInterfaceManager.DrawLobby(spriteBatch, networkManager);

            spriteBatch.End();
        }
    }
}
