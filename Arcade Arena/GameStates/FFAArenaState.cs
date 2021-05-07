using Arcade_Arena.Classes;
using Arcade_Arena.Managers;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Arcade_Arena
{
    class FFAArenaState : GameState
    {

        //multiplayer
        private NetworkManager networkManager;
        private PlayerManager playerManager;
        private AbilityManager abilityManager;

        private UserInterfaceManager userInterfaceManager;


        private Wizard player;
        public static Lava lava;



        public FFAArenaState(GameWindow Window, SpriteBatch spriteBatch) : base (Window)
        {
     

            networkManager = new NetworkManager();

            player = new Wizard(new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), AssetManager.WizardSpriteSheet, 3f, 0.0);
            lava = new Lava(Game1.graphics.GraphicsDevice, Window);
            playerManager = new PlayerManager(networkManager, player);
            abilityManager = new AbilityManager(networkManager, playerManager);

            userInterfaceManager = new UserInterfaceManager(networkManager, Window);
            lava.DrawRenderTarget(spriteBatch);

            networkManager.Start();

        }

        public override void Update(GameTime gameTime, ref States state)
        {
            if (MouseKeyboardManager.Clicked(Keys.P))
            {
                if (state == States.Pause)
                    state = States.FFA;
                else
                    state = States.Pause;

            }

            networkManager.Active = networkManager.Status == NetConnectionStatus.Connected;
            
            networkManager.Update();
            playerManager.UpdatePlayer();
            abilityManager.Update();
            userInterfaceManager.Update(gameTime);

            MouseKeyboardManager.Update();
            player.Update();

            player.CheckLavaCollision(lava);
        }

        public override void Draw(SpriteBatch spriteBatch, States state)
        {
            Game1.graphics.GraphicsDevice.Clear(networkManager.Active ? Color.Green : Color.Red);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (networkManager.Active)
            {
                lava.Draw(spriteBatch);
                userInterfaceManager.Draw(spriteBatch);

                foreach (var player in networkManager.Players)
                {

                    if (player.Username != networkManager.Username && player != null)
                    {
                        Rectangle source = new Rectangle(player.Animation.XRecPos, player.Animation.YRecPos, player.Animation.Width, player.Animation.Height);

                        if (!player.intersectingLava)
                        {
                            switch (player.Type)
                            {
                                case Library.Player.ClassType.Wizard:
                                    spriteBatch.Draw(AssetManager.WizardSpriteSheet, new Vector2(player.XPosition, player.YPosition), source,
                                        Color.White, 0f, Vector2.Zero, 5.0f, SpriteEffects.None, 1.0f);
                                    break;
                                case Library.Player.ClassType.Ogre:
                                    spriteBatch.Draw(AssetManager.ogreSpriteSheet, new Vector2(player.XPosition, player.YPosition), source,
                                        Color.White, 0f, Vector2.Zero, 5.0f, SpriteEffects.None, 1.0f);
                                    break;
                                case Library.Player.ClassType.Huntress:
                                    break;
                                case Library.Player.ClassType.TimeTraveler:
                                    break;
                                case Library.Player.ClassType.Assassin:
                                    break;
                                case Library.Player.ClassType.Knight:
                                    break;
                            }

                        }
                        
                        //spriteBatch.DrawString(font, player.Username, new Vector2(player.XPosition - 10, player.YPosition - 10), Color.Black);
                    }
                    else
                    {
                        if (!this.player.intersectingLava)
                        {
                            this.player.Draw(spriteBatch);

                        }
                    }
                }
                abilityManager.Draw(spriteBatch);
            }
            // spriteBatch.Draw(AssetManager.lava, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), null, Color.White, 0.0f, new Vector2(AssetManager.lava.Width / 2, AssetManager.lava.Height / 2), 1.0f, SpriteEffects.None, 1.0f);


            spriteBatch.End();

        }

    }
}