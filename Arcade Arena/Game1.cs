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

        static Random random = new Random();

        public static double elapsedGameTimeSeconds { get; private set; }
        public static double elapsedGameTimeMilliseconds { get; private set; }

        private static ObstacleManager obstacleManager = new ObstacleManager(); //it be here?

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

            gameState.Update(gameTime);
       
            base.Update(gameTime);
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        protected override void Draw(GameTime gameTime)
        {

            gameState.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
