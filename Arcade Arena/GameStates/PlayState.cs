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


        private Level currentLevel;
        private SpriteBatch spriteBatch;
        
        private Character player;


        public PlayState(GameWindow Window, SpriteBatch spriteBatch, Character player) : base (Window)
        {
            this.spriteBatch = spriteBatch;

            if(player is Wizard wizard)
            {this.player = wizard;}
            if (player is Ogre ogre)
            { this.player = ogre; }
            if (player is TimeTraveler timeTraveler)
            { this.player = timeTraveler; }
            if (player is Assassin assassin)
            { this.player = assassin; }

            currentLevel = CreateNewLevel();

            networkManager = new NetworkManager();
            

            playerManager = new PlayerManager(networkManager, player);
            abilityManager = new AbilityManager(networkManager, playerManager);

            userInterfaceManager = new UserInterfaceManager(networkManager, Window);

            networkManager.Start();
        }

        private Level CreateNewLevel()
        {
            return new Level(Window, spriteBatch, player);
        }


        public override void Update(GameTime gameTime, ref States state, ref Character unused)
        {
            if (InputManager.Clicked(Keys.P))
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
            InputManager.Update();

            currentLevel.Update();
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

                currentLevel.Draw(spriteBatch, networkManager);

                abilityManager.Draw(spriteBatch);
            }
            // spriteBatch.Draw(AssetManager.lava, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), null, Color.White, 0.0f, new Vector2(AssetManager.lava.Width / 2, AssetManager.lava.Height / 2), 1.0f, SpriteEffects.None, 1.0f);


            spriteBatch.End();

        }

    }
}