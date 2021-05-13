using Arcade_Arena.Classes;
using Arcade_Arena.Managers;
using Arcade_Arena.GameStates;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Arcade_Arena
{
    enum States
    {
        Menu,
        FFA,
        CharacterSelection,
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

        Character player;

        PlayState ffaArena;
        MainMenuState mainMenu;
        CharacterSelectionState characterSelection;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.ApplyChanges();
        }

   
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager.LoadTextures(Content);

            
            mainMenu = new MainMenuState(Window);
            characterSelection = new CharacterSelectionState(Window);

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
                    mainMenu.Update(gameTime, ref state, ref player);
                    break;
                case States.Quit:
                    Exit();
                    break;
                case States.FFA:
                    ffaArena.Update(gameTime, ref state, ref player);                    
                    break;
                case States.CharacterSelection:
                    characterSelection.Update(gameTime, ref state, ref player);
                    if(state == States.FFA)
                    {
                        ffaArena = new PlayState(Window, spriteBatch, (Wizard)player);
                    }
                    break;
                case States.Pause:
                    mainMenu.Update(gameTime, ref state, ref player);
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
                case States.CharacterSelection:
                    characterSelection.Draw(spriteBatch, state);
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
