using Arcade_Arena.Classes;
using Arcade_Arena.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    class Level
    {
        private List<Obstacle> obstacles;

        private Character player;
        public static Lava lava;


        public Level(GameWindow Window, SpriteBatch spriteBatch, Character player)
        {
            obstacles = new List<Obstacle>();

            lava = new Lava(Game1.graphics.GraphicsDevice, Window);
            lava.DrawRenderTarget(spriteBatch);

            this.player = player;
            InitiateLevel();
        }

        /// <summary>
        /// Initiating the level means creating obstacles and adding other stuff such as powerupplatforms and potential player spawn areas
        /// </summary>
        private void InitiateLevel()
        {
            obstacles.Add(new Obstacle(new Vector2(100, 100), AssetManager.LargeBox));
        }

        public void Update()
        {
            player.Update();
        }

        public void Draw(SpriteBatch spriteBatch, NetworkManager networkManager)
        {
            foreach (Obstacle obstacle in obstacles)
            {
                obstacle.Draw(spriteBatch);
            }
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
                        if (this.player is Wizard)
                        {
                            Wizard tempPlayer = (Wizard)this.player;
                            tempPlayer.Draw(spriteBatch);
                        }
                        else if(this.player is Ogre)
                        {
                            Ogre tempPlayer = (Ogre)this.player;
                            tempPlayer.Draw(spriteBatch);
                        }
                        else if (this.player is Huntress)
                        {
                            Huntress tempPlayer = (Huntress)this.player;
                            tempPlayer.Draw(spriteBatch);
                        }
                        else if (this.player is Knight)
                        {
                            Knight tempPlayer = (Knight)this.player;
                            tempPlayer.Draw(spriteBatch);
                        }
                        else if(this.player is TimeTraveler)
                        {
                            TimeTraveler tempPlayer = (TimeTraveler)this.player;
                            tempPlayer.Draw(spriteBatch);
                        }
                        
                        spriteBatch.DrawString(AssetManager.CooldownFont, $"{networkManager.Username}", new Vector2(player.XPosition, player.YPosition - 5), Color.White);
                        spriteBatch.DrawString(AssetManager.CooldownFont, $"{this.player.Health}", new Vector2(player.XPosition, player.YPosition - 20), Color.White);

                    }
                }

            }
        }
    }
}
