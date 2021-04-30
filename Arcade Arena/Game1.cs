using Arcade_Arena.Classes;
using Arcade_Arena.Managers;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arcade_Arena
{
    enum States
    {
        Menu,
        FFA
    }


    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static double elapsedGameTimeSeconds { get; private set; }
        public static double elapsedGameTimeMilliseconds { get; private set; }

        States state = States.Menu;

        FFAArenaState ffaArena;
        MainMenuState mainMenu;



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

            elapsedGameTimeSeconds = gameTime.ElapsedGameTime.TotalSeconds;
            elapsedGameTimeMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;

            MouseKeyboardManager.Update();

            switch (state)
            {
                case (States.Menu):
                    mainMenu.Update(gameTime);
                    break;
                case (States.FFA):
                    ffaArena.Update(gameTime);
                    break;
                default:
                    break;

            }



       
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            switch (state)
            {
                case (States.Menu):
                    mainMenu.Draw(spriteBatch);
                    break;
                case (States.FFA):
                    ffaArena.Draw(spriteBatch);
                    break;
                default:
                    break;

            }

            base.Draw(gameTime);
        }

       


    }
}
