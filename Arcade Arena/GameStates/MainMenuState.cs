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
            logoPos = SetAlignedYPos(AssetManager.arcadeArenaLogo, (int)(Window.ClientBounds.Height * 0.05));
            startPos = SetAlignedYPos(AssetManager.startButton, (int)(Window.ClientBounds.Height * 0.3));
            settingsPos = SetAlignedYPos(AssetManager.settingsButton, (int)(Window.ClientBounds.Height * 0.52));
            quitPos = SetAlignedYPos(AssetManager.quitButton, (int)(Window.ClientBounds.Height * 0.74));

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

      

        public override void Update(GameTime gameTime, ref States state, ref Character player)
        {
            if (MouseKeyboardManager.LeftClick)
            {
                if (startRect.Contains(MouseKeyboardManager.MousePosition.ToPoint()))
                {
                    state = States.CharacterSelection;
                }
                else if (settingsRect.Contains(MouseKeyboardManager.MousePosition.ToPoint()))
                {
                    state = States.Settings;
                }
                else if (quitRect.Contains(MouseKeyboardManager.MousePosition.ToPoint()))
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