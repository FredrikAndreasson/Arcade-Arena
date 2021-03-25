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

        Texture2D ball;
        Texture2D targetDummy;
        Texture2D lava;

        Ball Player;
        TargetDummy target;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1900;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ball = Content.Load<Texture2D>("Class\\Ball");
            targetDummy = Content.Load<Texture2D>("TargetDummy");
            lava = Content.Load<Texture2D>("LavaSprite\\Lava");

            Player = new Ball(new Vector2(100, 100), ball, 20f, 0.0);
            target = new TargetDummy(new Vector2(200, 200), targetDummy);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            Player.Draw(spriteBatch);
            target.Draw(spriteBatch);

            spriteBatch.Draw(lava, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), null, Color.White, 0.0f, new Vector2(lava.Width / 2, lava.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
            

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
