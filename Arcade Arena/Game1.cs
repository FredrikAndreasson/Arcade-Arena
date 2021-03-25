using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Arcade_Arena
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

  

        Ball Player;
        TargetDummy target; // Test
        Lava lava;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1900;
            graphics.ApplyChanges();
        }

        bool DoesNotCollide(Ball g)
        {
            Color[] pixels = new Color[g.texture.Width * g.texture.Height];
            Color[] pixels2 = new Color[g.texture.Width * g.texture.Height];
            g.texture.GetData<Color>(pixels2);
            lava.renderTarget.GetData(0, new Rectangle(g.position.ToPoint(), new Point(g.texture.Width, g.texture.Height)), pixels, 0, pixels.Length);
            for (int i = 0; i < pixels.Length; ++i)
            {
                if (pixels[i].A > 0.0f && pixels2[i].A > 0.0f)
                    return false;
            }
            return true;
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager.LoadTextures(Content);

            lava = new Lava(GraphicsDevice, 200);

            Player = new Ball(new Vector2(Window.ClientBounds.Width/2, Window.ClientBounds.Height/2), AssetManager.ball, 3f, 0.0);
            target = new TargetDummy(new Vector2(200, 200), AssetManager.targetDummy);

            lava.DrawRenderTarget(spriteBatch);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Player.Update(gameTime);
            lava.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            lava.Draw(spriteBatch);

            if (DoesNotCollide(Player))
            {
                Player.Draw(spriteBatch);
            }
            target.Draw(spriteBatch);

           // spriteBatch.Draw(AssetManager.lava, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), null, Color.White, 0.0f, new Vector2(AssetManager.lava.Width / 2, AssetManager.lava.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
            

            spriteBatch.End();

            base.Draw(gameTime);
        }

       


    }
}
