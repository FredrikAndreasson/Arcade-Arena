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
using Arcade_Arena.Library;

namespace Arcade_Arena
{
    enum States
    {
        Menu,
        FFA,
        CharacterSelection,
        Pause,
        Settings,
        Lobby,
        Quit,
        Win
    }


    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Random random = new Random();
        public static int seed = 0;
        public static bool LocalWin;

        public const float SCALE = 5.0f;


        private NetworkManager networkManager;
        private PlayerManager playerManager;


        public static double elapsedGameTimeSeconds { get; private set; }
        public static double elapsedGameTimeMilliseconds { get; private set; }
        
        States state = States.Menu;

        Character player;

        PlayState ffaArena;
        MainMenuState mainMenu;
        CharacterSelectionState characterSelection;
        LobbyState lobby;
        WinState gameOver;

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
            Library.Player tempPlayer = new Library.Player
            {Type = Library.Player.ClassType.Wizard};
            networkManager = new NetworkManager(tempPlayer);
            playerManager = new PlayerManager(networkManager);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager.LoadTextures(Content);

            
            mainMenu = new MainMenuState(Window);
            characterSelection = new CharacterSelectionState(Window, networkManager);
            lobby = new LobbyState(Window, networkManager, ref player, playerManager);
            gameOver = new WinState(Window);

        }

        protected override void Initialize()
        {

            


            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || gameOver.Quit)
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
                    if (state == States.FFA)
                    {
                        ffaArena = new PlayState(Window, spriteBatch, player, networkManager, playerManager);
                    }
                    break;
                case States.Pause:
                    mainMenu.Update(gameTime, ref state, ref player);
                    break;
                case States.Lobby: 
                    lobby.Update(gameTime, ref state, ref player);
                    if (state == States.FFA)
                    {
                        ffaArena = new PlayState(Window, spriteBatch, player, networkManager, playerManager);
                    }
                    break;
                case States.Win:
                    gameOver.Won = LocalWin;
                    gameOver.Update(gameTime, ref state, ref player);
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
                case States.Lobby:
                    lobby.Draw(spriteBatch, state);
                    break;
                case States.Win:
                    gameOver.Draw(spriteBatch, state);
                    break;
                default:
                    break;

            }

            base.Draw(gameTime);
        }
    }
}
