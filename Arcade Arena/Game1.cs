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
    enum States
    {
        Menu,
        FFA,
        Pause,
        Settings,
        Quit,
    }


    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Random random = new Random();

        public const float SCALE = 5.0f;

        public static double elapsedGameTimeSeconds { get; private set; }
        public static double elapsedGameTimeMilliseconds { get; private set; }

        private static ObstacleManager obstacleManager = new ObstacleManager(); //it be here?
        
        States state = States.Menu;

        FFAArenaState ffaArena;
        MainMenuState mainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.ApplyChanges();
        }

   
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager.LoadTextures(Content);

            ffaArena = new FFAArenaState(Window, spriteBatch);
            mainMenu = new MainMenuState(Window);

        }

        protected override void Initialize()
        {

            


            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseKeyboardManager.Update();

            elapsedGameTimeSeconds = gameTime.ElapsedGameTime.TotalSeconds;
            elapsedGameTimeMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;

            switch (state)
            {
                case States.Menu:
                    mainMenu.Update(gameTime, ref state);
                    break;
                case States.Quit:
                    Exit();
                    break;
                case States.FFA:
                    ffaArena.Update(gameTime, ref state);
                    break;
                case States.Pause:
                    mainMenu.Update(gameTime, ref state);
                    break;

                default:
                    break;

            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Game1.graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (state)
            {
                case (States.Menu):
                    mainMenu.Draw(spriteBatch, state);
                    break;
                case (States.FFA):
                    ffaArena.Draw(spriteBatch, state);
                    break;
                case States.Pause:
                    ffaArena.Draw(spriteBatch, state);
                    mainMenu.Draw(spriteBatch, state);
                    break;
                default:
                    break;

            }

            base.Draw(gameTime);
        }
    }
}
