using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Managers
{
    class UserInterfaceManagerHealth : UserInterfaceManager
    {
        public static Rectangle healthBarRectangle;
        private PlayerManager playerManager;

        public UserInterfaceManagerHealth(GameWindow window, PlayerManager playerManager) : base(window, playerManager)
        {
            this.playerManager = playerManager;
            healthBarRectangle = new Rectangle(0, 0, AssetManager.HealthBarOverlay.Width, AssetManager.HealthBarOverlay.Height);
        }


        public void DrawHealth(SpriteBatch spriteBatch)
        {

            healthBarRectangle.Width = (int)(AssetManager.HealthBarOverlay.Width * playerManager.clientPlayer.health / playerManager.clientPlayer.maxHealth);

            if (this.playerManager.clientPlayer.Health > 0)
            {
                spriteBatch.Draw(AssetManager.HealthBar, new Vector2(this.playerManager.clientPlayer.Position.X, this.playerManager.clientPlayer.Position.Y - 40), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                spriteBatch.Draw(AssetManager.HealthBarOverlay, new Vector2(this.playerManager.clientPlayer.Position.X, this.playerManager.clientPlayer.Position.Y - 40), healthBarRectangle, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            }
        }


    }
}
