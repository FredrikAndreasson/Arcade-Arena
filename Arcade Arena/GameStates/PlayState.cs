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
    class PlayState : GameState
    {

        //multiplayer
        private NetworkManager networkManager;
        private PlayerManager playerManager;
        private AbilityManager abilityManager;

        private UserInterfaceManager userInterfaceManager;


        
        public static Lava lava;
        private var player;


        public PlayState(GameWindow Window, SpriteBatch spriteBatch, Wizard player) : base (Window)
        {
            if(player is Wizard)
            {
                this.player = player;

            }



            networkManager = new NetworkManager();
            lava = new Lava(Game1.graphics.GraphicsDevice, Window);
            playerManager = new PlayerManager(networkManager, player);
            abilityManager = new AbilityManager(networkManager, playerManager);

            userInterfaceManager = new UserInterfaceManager(networkManager, Window);
            lava.DrawRenderTarget(spriteBatch);

            networkManager.Start();

        }


        public override void Update(GameTime gameTime, ref States state, ref Character unused)
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
            abilityManager.Update(player);
            userInterfaceManager.Update(gameTime);

            MouseKeyboardManager.Update();
            player.Update();

            //player.CheckLavaCollision(lava);
        }

        public override void Draw(SpriteBatch spriteBatch, States state)
        {
            Game1.graphics.GraphicsDevice.Clear(networkManager.Active ? Color.Green : Color.Red);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (networkManager.Active)
            {
                //lava.Draw(spriteBatch);
                userInterfaceManager.Draw(spriteBatch);

                foreach (var player in networkManager.Players)
                {

                    if (player.Username != networkManager.Username && player != null)
                    {
                        Rectangle source = new Rectangle(player.Animation.XRecPos, player.Animation.YRecPos, player.Animation.Width, player.Animation.Height);
                        if (!player.IntersectingLava) 
                        { 

                            if (player.Health > 0)
                            {
                                switch (player.Type)
                                {
                                    case Library.Player.ClassType.Wizard:
                                        spriteBatch.Draw(AssetManager.WizardSpriteSheet, new Vector2(player.XPosition, player.YPosition), source,
                                            Color.White, 0f, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
                                        break;
                                    case Library.Player.ClassType.Ogre:
                                        spriteBatch.Draw(AssetManager.ogreSpriteSheet, new Vector2(player.XPosition, player.YPosition), source,
                                            Color.White, 0f, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
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

                                spriteBatch.DrawString(AssetManager.CooldownFont, $"{player.Username}", new Vector2(player.XPosition, player.YPosition - 5), Color.White);
                                spriteBatch.DrawString(AssetManager.CooldownFont, $"{player.Health}", new Vector2(player.XPosition, player.YPosition - 20), Color.White);


                            }
                    }

                        //spriteBatch.DrawString(font, player.Username, new Vector2(player.XPosition - 10, player.YPosition - 10), Color.Black);
                    }
                    else
                    {
                        if (!this.player.IntersectingLava || player.Health > 0)
                        {
                            this.player.Draw(spriteBatch);
                            spriteBatch.DrawString(AssetManager.CooldownFont, $"{networkManager.Username}", new Vector2(player.XPosition, player.YPosition - 5), Color.White);
                            spriteBatch.DrawString(AssetManager.CooldownFont, $"{this.player.Health}", new Vector2(player.XPosition, player.YPosition - 20), Color.White);

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