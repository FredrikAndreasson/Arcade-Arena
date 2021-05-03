using Arcade_Arena.Classes;
using Arcade_Arena.Managers;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        private UserInterfaceManager userInterfaceManager;


        private Wizard player;
        public static Lava lava;



        public FFAArenaState(GameWindow Window, SpriteBatch spriteBatch)
        {
     

            networkManager = new NetworkManager();

            player = new Wizard(new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), AssetManager.WizardSpriteSheet, 3f, 0.0);
            lava = new Lava(Game1.graphics.GraphicsDevice, 400);
            playerManager = new PlayerManager(networkManager, player);
            userInterfaceManager = new UserInterfaceManager(networkManager, Window);
            lava.DrawRenderTarget(spriteBatch);

            Intitialize();

        }

       

        bool DoesNotCollide(Wizard g)
        {
            Color[] pixels = new Color[g.texture.Width * g.texture.Height];
            Color[] pixels2 = new Color[g.texture.Width * g.texture.Height];
            g.texture.GetData<Color>(pixels2);
            lava.renderTarget.GetData(0, new Rectangle(g.position.ToPoint(), new Point(g.texture.Width, g.texture.Height)), pixels, 0, pixels.Length);
            for (int i = 0; i < pixels.Length; ++i)
            {
                if (pixels[i].A > 0.0f && pixels2[i].A > 0.0f)
                    return false;
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {


            networkManager.Active = networkManager.Status == NetConnectionStatus.Connected;

            networkManager.Update();
            playerManager.UpdatePlayer();
            userInterfaceManager.Update(gameTime);

            MouseKeyboardManager.Update();

            player.Update();
        }

        private void Intitialize()
        {
            networkManager.Start();
        }


        public override void Draw(SpriteBatch spriteBatch)
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
                        System.Diagnostics.Debug.WriteLine(" XrecPos game1: " + player.Animation.XRecPos);
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
                        //spriteBatch.DrawString(font, player.Username, new Vector2(player.XPosition - 10, player.YPosition - 10), Color.Black);
                    }
                    else
                    {
                        this.player.Draw(spriteBatch);
                        if (DoesNotCollide(this.player))
                        {

                        }
                    }
                }
            }
            // spriteBatch.Draw(AssetManager.lava, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), null, Color.White, 0.0f, new Vector2(AssetManager.lava.Width / 2, AssetManager.lava.Height / 2), 1.0f, SpriteEffects.None, 1.0f);


            spriteBatch.End();

        }

    }
}