using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.GameStates
{
    class WinState : GameState
    {
        Rectangle quitPos;
        
        public WinState(GameWindow window) : base(window)
        {
            quitPos = new Rectangle(window.ClientBounds.Width / 2 - 100, window.ClientBounds.Height / 2 + 100, 200, 100);
            Quit = false;
        }

        public bool Quit { get; set; }
        public bool Won { get; set; }

        public override void Update(GameTime gameTime, ref States state, ref Character player)
        {
            if (MouseKeyboardManager.LeftClick && quitPos.Contains(MouseKeyboardManager.MousePosition))
            {
                Quit = true;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, States states)
        {
            Color color = Won ? Color.CornflowerBlue : Color.Red;
            spriteBatch.GraphicsDevice.Clear(color);

            spriteBatch.Begin();

            spriteBatch.Draw(AssetManager.arcadeArenaLogo, new Vector2(Window.ClientBounds.Width/2- AssetManager.arcadeArenaLogo.Width/2,
                Window.ClientBounds.Height/2- AssetManager.arcadeArenaLogo.Height), Color.White);
            spriteBatch.Draw(AssetManager.quitButton, quitPos, Color.White);

            spriteBatch.End();
        }
    }
}
