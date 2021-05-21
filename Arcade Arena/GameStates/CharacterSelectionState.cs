﻿using Arcade_Arena.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.GameStates
{
    class CharacterSelectionState : GameState
    {
        private Vector2 ogrePos;
        private Vector2 wizardPos;
        private Vector2 huntressPos;
        private Vector2 travelerPos;
        private Vector2 knightPos;

        private Rectangle ogreRect;
        private Rectangle wizardRect;
        private Rectangle huntressRect;
        private Rectangle travelerRect;
        private Rectangle knightRect;

        public CharacterSelectionState(GameWindow Window) : base(Window)
        {
            wizardPos = SetAlignedXPos(AssetManager.selectWizard, (int)(Window.ClientBounds.Width * 0.1));
            ogrePos = SetAlignedXPos(AssetManager.selectOgre, (int)(Window.ClientBounds.Width * 0.4));
            huntressPos = SetAlignedXPos(AssetManager.selectHuntress, (int)(Window.ClientBounds.Width * 0.7));

            wizardRect = new Rectangle(wizardPos.ToPoint(), new Point(AssetManager.selectWizard.Width, AssetManager.selectWizard.Height));
            ogreRect = new Rectangle(ogrePos.ToPoint(), new Point(AssetManager.selectOgre.Width, AssetManager.selectOgre.Height));
            huntressRect = new Rectangle(huntressPos.ToPoint(), new Point(AssetManager.selectHuntress.Width, AssetManager.selectHuntress.Height));
        }

        public override void Draw(SpriteBatch spriteBatch, States state)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(AssetManager.selectOgre, ogrePos, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            spriteBatch.Draw(AssetManager.selectWizard, wizardPos, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            spriteBatch.Draw(AssetManager.selectHuntress, huntressPos, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime, ref States state, ref Character player)
        {
            if (MouseKeyboardManager.LeftClick)
            {

                if (wizardRect.Contains(MouseKeyboardManager.MousePosition.ToPoint()))
                {
                    player = new Wizard(new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), 3f, 0.0);
                    state = States.Lobby;
                }
                else if (ogreRect.Contains(MouseKeyboardManager.MousePosition.ToPoint()))
                {
                    player = new Ogre(new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), 3f, 0.0);
                    state = States.Lobby;
                    
                }
                else if (huntressRect.Contains(MouseKeyboardManager.MousePosition.ToPoint()))
                {

                }
            }


        }

    }
}
