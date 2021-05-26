using Arcade_Arena.Classes;
using Arcade_Arena.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using SharpNoise;
using SharpNoise.Modules;
using Microsoft.Xna.Framework.Input;
using static Arcade_Arena.Obstacle;

namespace Arcade_Arena
{
    class Level
    {
        private List<Obstacle> obstacles;

        private Character player;
        public static Lava lava;

        private Perlin perlin;
        private GameWindow window;


        public Level(GameWindow Window, SpriteBatch spriteBatch, Character player)
        {
            obstacles = new List<Obstacle>();

            lava = new Lava(Game1.graphics.GraphicsDevice, Window);
            lava.DrawRenderTarget(spriteBatch);

            this.player = player;

            this.perlin = new Perlin();
            this.window = Window;

            InitiateLevel();
        }

        public List<Obstacle> Obstacles => obstacles;

        /// <summary>
        /// Initiating the level means creating obstacles and adding other stuff such as powerupplatforms and potential player spawn areas
        /// </summary>
        private void InitiateLevel()
        {
            double max = 0;
            int twoThirdsWidth = (window.ClientBounds.Width * 2) / 3;
            int twoThirdsHeight = (window.ClientBounds.Height * 2) / 3;
            bool[,] isFilled = new bool[twoThirdsWidth / 64, twoThirdsHeight / 64];
            perlin.Seed = 10;
            for (double x = 0; x < twoThirdsWidth; x++)
            {
                for (double y = 0; y < twoThirdsHeight; y++)
                {
                    double value = perlin.GetValue(x/100, y/100, 0.95521);
                    if (value > max)
                    {
                        max = value;
                    }
                    Tiling(value, x, y, ref isFilled);
                }
            }
            for (int x = 0; x < isFilled.GetLength(0); x++)
            {
                for (int y = 0; y < isFilled.GetLength(1); y++)
                {
                    var obstacle = obstacles.Find(o => o.xIndex == x && o.yIndex == y);
                    if (obstacle != null)
                    {
                        obstacle.UpdateOrientation(RelativePositionCalc(ref isFilled, x, y));
                    }
                    
                }
            }
            System.Diagnostics.Debug.WriteLine(max);
        }

        private void Tiling(double value, double x, double y, ref bool[,] isFilled)
        {
            int xNormalized = (int)(x / 64);
            int yNormalized = (int)(y / 64);
            if (xNormalized == isFilled.GetLength(0) || yNormalized == isFilled.GetLength(1))
            {
                return;
            }
            if (value >= 0.9)
            {
                if (!isFilled[xNormalized, yNormalized])
                {
                    isFilled[xNormalized, yNormalized] = true;
                    obstacles.Add(new Obstacle(new Vector2(xNormalized * 64 + (window.ClientBounds.Width / 6),
                        yNormalized * 64 +(window.ClientBounds.Height / 6)), RelativePosition.single, xNormalized, yNormalized));
                    
                }
            }
        }

        private RelativePosition RelativePositionCalc(ref bool[,] isFilled, int x, int y)
        {
            bool bottom = false, left = false, right = false, top = false;

            if (x > 0)
            {
                if (isFilled[x - 1, y] == true)
                {left = true;}
            }
            if (x < (isFilled.GetLength(0) - 1))
            {
                if (isFilled[x + 1, y] == true)
                { right = true; }
            }
            if (y > 0)
            {
                if (isFilled[x, y - 1] == true)
                { top = true; }
            }
            if (y < (isFilled.GetLength(1) - 1))
            {
                if (isFilled[x, y + 1] == true)
                { bottom = true; }
            }


            if (bottom)
            {
                if (left)
                {
                    if (right)
                    {
                        if (top)
                        {
                            return RelativePosition.middle;
                        }
                        return RelativePosition.top;
                    }
                    if (top)
                    {
                        return RelativePosition.right;
                    }
                    return RelativePosition.topRightCorner;
                }
                if (right)
                {
                    if (top)
                    {
                        return RelativePosition.left;
                    }
                    return RelativePosition.topLeftCorner;
                }
                if (top)
                {
                    return RelativePosition.horizontalTunnel;
                }
                return RelativePosition.topSingle;
            }
            else if (left)
            {
                if (right)
                {
                    if (top)
                    {
                        return RelativePosition.bottom;
                    }
                    return RelativePosition.verticalTunnel;
                }
                if (top)
                {
                    return RelativePosition.bottomRightCorner;
                }
                return RelativePosition.rightSingle;
            }
            else if (right)
            {
                if (top)
                {
                    return RelativePosition.bottomLeftCorner;
                }
                return RelativePosition.leftSingle;
            }
            else if (top)
            {
                return RelativePosition.bottomSingle;
            }

            return RelativePosition.single;
        }

        public void Update()
        {
            player.Update();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                obstacles.Clear();
                InitiateLevel();
            }
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
                            SpriteEffects spritEffect;

                            if (player.OrbiterRotation >= 1.53269 || player.OrbiterRotation <= -1.547545)
                            {
                                spritEffect = SpriteEffects.FlipHorizontally;
                            }
                            else
                            {
                                spritEffect = SpriteEffects.None;
                            }

                            switch (player.Type)
                            {
                                case Library.Player.ClassType.Wizard:
                                    spriteBatch.Draw(AssetManager.WizardSpriteSheet, new Vector2(player.XPosition, player.YPosition), source,
                                        Color.White, 0f, Vector2.Zero, Game1.SCALE, spritEffect, 1.0f);
                                    break;
                                case Library.Player.ClassType.Ogre:
                                    spriteBatch.Draw(AssetManager.ogreSpriteSheet, new Vector2(player.XPosition, player.YPosition), source,
                                        Color.White, 0f, Vector2.Zero, Game1.SCALE, spritEffect, 1.0f);
                                    break;
                                case Library.Player.ClassType.Huntress:
                                    spriteBatch.Draw(AssetManager.HuntressSpriteSheet, new Vector2(player.XPosition, player.YPosition), source,
                                        Color.White, 0f, Vector2.Zero, Game1.SCALE, spritEffect, 1.0f);
                                    break;
                                case Library.Player.ClassType.TimeTraveler:
                                    spriteBatch.Draw(AssetManager.TimeTravelerSpriteSheet, new Vector2(player.XPosition, player.YPosition), source,
                                        Color.White, 0f, Vector2.Zero, Game1.SCALE, spritEffect, 1.0f);
                                    break;
                                case Library.Player.ClassType.Assassin:
                                    //spriteBatch.Draw(AssetManager., new Vector2(player.XPosition, player.YPosition), source,
                                    //    Color.White, 0f, Vector2.Zero, Game1.SCALE, spritEffect, 1.0f);
                                    break;
                                case Library.Player.ClassType.Knight:
                                    spriteBatch.Draw(AssetManager.KnightSpriteSheet, new Vector2(player.XPosition, player.YPosition), source,
                                        Color.White, 0f, Vector2.Zero, Game1.SCALE, spritEffect, 1.0f);
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
