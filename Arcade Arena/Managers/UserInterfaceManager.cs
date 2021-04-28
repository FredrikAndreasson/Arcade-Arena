using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Managers
{
    class UserInterfaceManager
    {
        private SpriteFont cooldownFont;
        private Texture2D wizardAbilites;

        private List<Vector2> abilityPositions;
        private NetworkManager networkManager;
        private GameWindow window;

        private int cooldown = 1000;

        public UserInterfaceManager(NetworkManager networkManager, GameWindow window)
        {
            this.networkManager = networkManager;
            this.window = window;

            cooldownFont = AssetManager.CooldownFont;
            wizardAbilites = AssetManager.WizardAbilityIconSheet;

            abilityPositions = new List<Vector2>();

            abilityPositions.Add(new Vector2(window.ClientBounds.Width / 2 - 32, window.ClientBounds.Height - 32));
            abilityPositions.Add(new Vector2(window.ClientBounds.Width / 2 + 32, window.ClientBounds.Height - 32));
        }

        public void Update(GameTime gameTime)
        {
            cooldown--;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < abilityPositions.Count; i++)
            {
                spriteBatch.DrawString(cooldownFont, cooldown.ToString(), abilityPositions[i]+ new Vector2(0, -16), Color.White);
                spriteBatch.Draw(wizardAbilites, abilityPositions[i], new Rectangle(i * 32, 0, 32, 32), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            }
        }
    }
}
