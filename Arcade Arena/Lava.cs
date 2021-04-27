using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Arcade_Arena
{
    public class Lava
    {
        private const int radiusShrinkRate = 1;
        private const int timerTime = 500;
        const int rectHeight = 20;
        public RenderTarget2D renderTarget; // ATALAY HAR GJORT DENNA PUBLIC FOR TILLFELLET
        GraphicsDevice graphicsDevice;
        int radius;
        Color[] originalLava;
        int middleWidth;
        int middleHeight;
        bool shrink;

        public Lava(GraphicsDevice graphicsDevice, int radius)
        {
            this.graphicsDevice = graphicsDevice;
            renderTarget = new RenderTarget2D(graphicsDevice, 1920, 1080);
            originalLava = new Color[renderTarget.Width * renderTarget.Height];
            middleHeight = AssetManager.lava.Height / 2;
            middleWidth = AssetManager.lava.Width / 2;
           
            this.radius = radius;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
        }

        public void DrawRenderTarget(SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(AssetManager.lava, Vector2.Zero, Color.White);
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
            renderTarget.GetData<Color>(originalLava);
        }

        public void ShrinkPlatform(int radius)
        {
            List<Rectangle> testRects = new List<Rectangle>();
            
            int j = -1;

            for (int i = 90; i < 270; i++)
            {

                j++;
                if ((int)(Math.Cos(MathHelper.ToRadians(90 - j)) * radius) + middleWidth - (int)((Math.Cos(MathHelper.ToRadians(i)) * radius) + middleWidth) < 2)
                {
                    continue;
                }

                if (i < 180)
                {
                    testRects.Add(new Rectangle((int)(Math.Cos(MathHelper.ToRadians(i)) * radius) + middleWidth, (int)(Math.Sin(MathHelper.ToRadians(i)) * radius) + middleHeight, (int)(Math.Cos(MathHelper.ToRadians(90 - j)) * radius) * 2, rectHeight));
                }
                else if (i < 270)
                {
                    testRects.Add(new Rectangle((int)(Math.Cos(MathHelper.ToRadians(i)) * radius) + middleWidth, (int)(Math.Sin(MathHelper.ToRadians(i)) * radius) + middleHeight, (int)(Math.Cos(MathHelper.ToRadians(90 - j)) * radius) * 2, rectHeight));

                }

            }

            Color[] transparentField = new Color[renderTarget.Width * renderTarget.Height];
            renderTarget.GetData<Color>(transparentField);
            for (int i = 0; i < transparentField.Length; i++)
            {
                transparentField[i] = Color.Transparent;
            }

            renderTarget.SetData<Color>(originalLava);

            foreach (Rectangle lavaRects in testRects)
            {
                renderTarget.SetData(0, lavaRects, transparentField, 0, lavaRects.Width * lavaRects.Height);
            }
        }
    }
}
