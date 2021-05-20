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

        private UserInterfaceManager userInterfaceManager;

        public LobbyState(GameWindow Window) : base(Window)
        {

            userInterfaceManager = new UserInterfaceManager(Window);
        }

        public override void Update(GameTime gameTime, ref States state, ref Character player)
        {
            
        }

        public override void Draw(SpriteBatch spritebatch, States state)
        {
            
        }
    }
}
