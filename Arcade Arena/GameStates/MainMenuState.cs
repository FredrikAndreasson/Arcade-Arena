using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    class MainMenuState : GameState
    {
        private Vector2 logoPos;
        private Vector2 startPos;
        private Vector2 settingsPos;
        private Vector2 quitPos;
        private float scale;

        public MainMenuState(GameWindow Window) : base(Window)
        {
            logoPos = SetAlignedPos(AssetManager.arcadeArenaLogo, (int)(Window.ClientBounds.Height * 0.05));
            startPos = SetAlignedPos(AssetManager.startButton, (int)(Window.ClientBounds.Height * 0.3));
            settingsPos = SetAlignedPos(AssetManager.settingsButton, (int)(Window.ClientBounds.Height * 0.52));
            quitPos = SetAlignedPos(AssetManager.quitButton, (int)(Window.ClientBounds.Height * 0.74));

            scale = 1f;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Game1.graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(AssetManager.arcadeArenaLogo, logoPos, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            spriteBatch.Draw(AssetManager.startButton, startPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(AssetManager.settingsButton, settingsPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(AssetManager.quitButton, quitPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            spriteBatch.End();

        }

        private Vector2 SetAlignedPos(Texture2D tex, int yPos)
        {
            Vector2 vector = new Vector2(Window.ClientBounds.Width / 2 - tex.Width / 2, yPos);
            return vector;
        }

        public override void Update(GameTime gameTime)
        {

        }

    }
}