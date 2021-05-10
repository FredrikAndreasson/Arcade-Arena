using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Arcade_Arena.Managers
{
    public class ObstacleManager
    {
        private List<Obstacle> obstacles = new List<Obstacle>();

        private int minObstacles = 0;
        private int maxObstacles = 10;

        private List<Texture2D> obstacleTextures = new List<Texture2D>();

        private Rectangle obstacleBoundary = new Rectangle(0, 0, 1000, 1000);

        public ObstacleManager()
        {
            AddObstacleTextures();
        }

        public void GenerateObstacles()
        {
            int nrObstacles = Game1.random.Next(minObstacles, maxObstacles + 1);
            int loop = 0;
            for (int i = 0; i < nrObstacles; i++)
            {
                bool successfulBoxPlacement = false;
                Texture2D obstacleTexture = obstacleTextures[Game1.random.Next(0, obstacleTextures.Count)];
                Obstacle obstacle = new Obstacle(new Vector2(0, 0), obstacleTexture);
                while (successfulBoxPlacement == false)
                {
                    int xValue = Game1.random.Next(obstacleBoundary.X, obstacleBoundary.Width + 1);
                    int yValue = Game1.random.Next(obstacleBoundary.Y, obstacleBoundary.Height+ 1);
                    obstacle.MoveObstacle(new Vector2(xValue, yValue));
                    if (true) //om den inte kolliderar med något
                    {
                        successfulBoxPlacement = true;
                        obstacles.Add(obstacle);
                    }
                    loop++;
                    if (loop >= 700)
                    {
                        successfulBoxPlacement = true; //end if too many loops
                    }
                }
            }
        }

        private void AddObstacleTextures()
        {
            obstacleTextures.Add(AssetManager.SmallBox);
            obstacleTextures.Add(AssetManager.LargeBox);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Obstacle obstacle in obstacles)
            {
                obstacle.Draw(spriteBatch);
            }
        }
    }
}
