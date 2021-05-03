using Arcade_Arena.Classes;
using Arcade_Arena.Managers;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arcade_Arena
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
//ability-and-server-sync

        //multiplayer
        private NetworkManager networkManager;
        private PlayerManager playerManager;
        private AbilityManager abilityManager;
        
        private UserInterfaceManager userInterfaceManager;
//=======
        static Random random = new Random();
//main

        public static double elapsedGameTimeSeconds { get; private set; }
        public static double elapsedGameTimeMilliseconds { get; private set; }



        FFAArenaState gameState;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.ApplyChanges();
        }

   
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager.LoadTextures(Content);

            gameState = new FFAArenaState(Window, spriteBatch);

//ability-and-server-sync
            player = new Wizard(new Vector2(Window.ClientBounds.Width/2, Window.ClientBounds.Height/2), AssetManager.WizardSpriteSheet, 3f, 0.0);
            lava = new Lava(Game1.graphics.GraphicsDevice, 400);

            playerManager = new PlayerManager(networkManager, player);
            abilityManager = new AbilityManager(networkManager, playerManager);
            userInterfaceManager = new UserInterfaceManager(networkManager, Window);

            lava.DrawRenderTarget(spriteBatch);
//=======
// main

        }

        protected override void Initialize()
        {

            


            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            elapsedGameTimeSeconds = gameTime.ElapsedGameTime.TotalSeconds;
            elapsedGameTimeMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;

//ability-and-server-sync
            networkManager.Active = networkManager.Status == NetConnectionStatus.Connected;

            networkManager.Update();
            playerManager.UpdatePlayer();
            abilityManager.Update();
            userInterfaceManager.Update(gameTime);

//=======
// main
            MouseKeyboardManager.Update();

            gameState.Update(gameTime);
       
            base.Update(gameTime);
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        protected override void Draw(GameTime gameTime)
        {
//ability-and-server-sync
            GraphicsDevice.Clear(networkManager.Active ? Color.Green : Color.Red);

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
                    abilityManager.Draw(spriteBatch);
                }
            }
           // spriteBatch.Draw(AssetManager.lava, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), null, Color.White, 0.0f, new Vector2(AssetManager.lava.Width / 2, AssetManager.lava.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
            
//=======
//main

            gameState.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
