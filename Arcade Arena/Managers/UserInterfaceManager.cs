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
        private Texture2D pixel;

        private Rectangle readyCheckRect;
        private Rectangle characterChangeRect;

        private List<Vector2> abilityPositions;
        private GameWindow window;

        private bool ready = false;

        private PlayerManager playerManager;

        public UserInterfaceManager(GameWindow window, PlayerManager playerManager)
        {
            this.window = window;

            cooldownFont = AssetManager.CooldownFont;
            wizardAbilites = AssetManager.WizardAbilityIconSheet;
            pixel = AssetManager.px;

            abilityPositions = new List<Vector2>();

            abilityPositions.Add(new Vector2(window.ClientBounds.Width / 2 - 32, window.ClientBounds.Height - 32));
            abilityPositions.Add(new Vector2(window.ClientBounds.Width / 2 + 32, window.ClientBounds.Height - 32));

            readyCheckRect = new Rectangle(window.ClientBounds.Width / 2 - 50, window.ClientBounds.Height / 2 - 100, 200, 50);
            characterChangeRect = new Rectangle(window.ClientBounds.Width / 2 - 50, window.ClientBounds.Height / 2 + 100, 200, 50);

            this.playerManager = playerManager;
        }

        public Rectangle ReadyCheckRect => readyCheckRect;
        public Rectangle CharacterChangeRect => characterChangeRect;

        public bool Ready { get { return ready; } set { ready = value; } }

        public void DrawLobby(SpriteBatch spriteBatch, NetworkManager networkManager)
        {
            if (ready)
            {
                spriteBatch.Draw(pixel, readyCheckRect, Color.Black);
                spriteBatch.DrawString(cooldownFont, "READY", new Vector2(readyCheckRect.X + 75, readyCheckRect.Y + 20), Color.Green);
            }
            else
            {
                spriteBatch.Draw(pixel, readyCheckRect, Color.Black);
                spriteBatch.DrawString(cooldownFont, "UNREADY", new Vector2(readyCheckRect.X + 70, readyCheckRect.Y + 20), Color.Red);
            }


            
            spriteBatch.Draw(pixel, characterChangeRect, Color.Black);
            spriteBatch.DrawString(cooldownFont, "CHANGE CHARACTER", new Vector2(characterChangeRect.X + 15, characterChangeRect.Y + 20), Color.White);

            for (int i = 0; i < networkManager.Players.Count; i++)
            {
                Vector2 pos = new Vector2(window.ClientBounds.Width - 150, (window.ClientBounds.Height/networkManager.Players.Count)*i+
                    (window.ClientBounds.Height/2)/networkManager.Players.Count);

                DrawConnectedPlayers(spriteBatch, pos, networkManager.Players[i].Username);
            }

            
        }

        public void UpdateGameplayLoop()
        {
            //
        }

        public void DrawGameplayLoop(SpriteBatch spriteBatch, NetworkManager networkManager)
        {
            Rectangle sourceOneRectangle = new Rectangle((int)abilityPositions[0].X, (int)abilityPositions[0].Y,
                64, (int)(32 * playerManager.clientPlayer.AbilityOneCooldown / playerManager.clientPlayer.AbilityOneMaxCooldown));
            Rectangle sourceTwoRectangle = new Rectangle((int)abilityPositions[1].X, (int)abilityPositions[1].Y,
                64, (int)(32 * playerManager.clientPlayer.AbilityTwoCooldown / playerManager.clientPlayer.AbilityTwoMaxCooldown));
            for (int i = 0; i < abilityPositions.Count; i++)
            {
                Rectangle destRectangle = new Rectangle((int)abilityPositions[i].X, (int)abilityPositions[i].Y, 64, 32);
                spriteBatch.Draw(pixel, destRectangle, Color.Black);
                if (i == 0)
                {spriteBatch.Draw(pixel, sourceOneRectangle, Color.White);}
                else
                {spriteBatch.Draw(pixel, sourceTwoRectangle, Color.White);}
                spriteBatch.DrawString(cooldownFont, string.Format("Ability {0}", i), abilityPositions[i], Color.Green);
            }
            

            for (int i = 0; i < networkManager.Players.Count; i++)
            {
                DrawScore(spriteBatch, new Vector2(i*(window.ClientBounds.Width/networkManager.Players.Count)+
                    ((window.ClientBounds.Width/2)/networkManager.Players.Count),20), networkManager.Players[i].Username, networkManager.Players[i].Score);
            }
        }

        private void DrawScore(SpriteBatch spriteBatch, Vector2 position, string username, int score)
        {
            spriteBatch.DrawString(cooldownFont, username + ": " + score, position, Color.White);
        }

        private void DrawConnectedPlayers(SpriteBatch spriteBatch, Vector2 position, string username)
        {
            spriteBatch.Draw(pixel, new Rectangle((int)position.X, (int)position.Y, (int)cooldownFont.MeasureString(username).X + 40, 25), Color.Black);
            spriteBatch.DrawString(cooldownFont, username, position + new Vector2(20, 5), Color.White);
            
        }


    }
}
