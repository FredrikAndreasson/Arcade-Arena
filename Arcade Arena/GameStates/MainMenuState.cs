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

        private Rectangle startRect;
        private Rectangle settingsRect;
        private Rectangle quitRect;



        private float scale;

        public MainMenuState(GameWindow Window) : base(Window)
        {
            logoPos = SetAlignedPos(AssetManager.arcadeArenaLogo, (int)(Window.ClientBounds.Height * 0.05));
            startPos = SetAlignedPos(AssetManager.startButton, (int)(Window.ClientBounds.Height * 0.3));
            settingsPos = SetAlignedPos(AssetManager.settingsButton, (int)(Window.ClientBounds.Height * 0.52));
            quitPos = SetAlignedPos(AssetManager.quitButton, (int)(Window.ClientBounds.Height * 0.74));

            startRect = new Rectangle(startPos.ToPoint(), new Point(AssetManager.startButton.Width, AssetManager.startButton.Height));
            settingsRect = new Rectangle(startPos.ToPoint(), new Point(AssetManager.settingsButton.Width, AssetManager.settingsButton.Height));
            quitRect = new Rectangle(quitPos.ToPoint(), new Point(AssetManager.quitButton.Width, AssetManager.quitButton.Height));

            scale = 1f;

        }

        public override void Draw(SpriteBatch spriteBatch, States state)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(AssetManager.arcadeArenaLogo, logoPos, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            if (state != States.Pause)
                spriteBatch.Draw(AssetManager.startButton, startPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(AssetManager.resumeButton, startPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            spriteBatch.Draw(AssetManager.settingsButton, settingsPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(AssetManager.quitButton, quitPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);


            spriteBatch.End();

        }

        private Vector2 SetAlignedPos(Texture2D tex, int yPos)
        {
            Vector2 vector = new Vector2(Window.ClientBounds.Width / 2 - tex.Width / 2, yPos);
            return vector;
        }

        public override void Update(GameTime gameTime, ref States state)
        {
            if (MouseKeyboardManager.leftClick)
            {

                if (startRect.Contains(MouseKeyboardManager.mousePosition.ToPoint()))
                {
                    state = States.FFA;
                }
                else if (settingsRect.Contains(MouseKeyboardManager.mousePosition.ToPoint()))
                {
                    state = States.Settings;
                }
                else if (quitRect.Contains(MouseKeyboardManager.mousePosition.ToPoint()))
                {
                    if (state == States.Pause)
                        state = States.Menu;
                    else
                        state = States.Quit;

                }

            }

        }

    }
}