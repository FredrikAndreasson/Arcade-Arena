using Arcade_Arena.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Managers
{
    class AbilityManager
    {
        NetworkManager networkManager;
        PlayerManager playerManager;

        public List<Ability> abilities;


        public AbilityManager(NetworkManager networkManager, PlayerManager playerManager)
        {
            this.networkManager = networkManager;
            this.playerManager = playerManager;
            abilities = new List<Ability>();
            
        }

        public void Update()
        {
            foreach (Ability ability in abilities)
            {
                ability.Update();
                networkManager.SendAbilityUpdate(ability, (byte)(abilities.Count-1));
            }

            for (int i = 0; i < playerManager.clientPlayer.abilityBuffer.Count; i++)
            {
                CreateAbility(playerManager.clientPlayer.abilityBuffer[i]);
                playerManager.clientPlayer.abilityBuffer.RemoveAt(i);
                i--;
            }

            AbilityDeletionCheck();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Ability ability in abilities)
            {
                ability.Draw(spriteBatch);
            }
            for (int i = 0; i < networkManager.Players.Count; i++)
            {
                if (networkManager.Players[i].Username != networkManager.Username)
                {
                    Player player = networkManager.Players[i];
                    for (int x = 0; x < player.abilities.Count; x++)
                    {
                        DrawAbility(spriteBatch, player.abilities[x], player.Type);
                    }
                }
            }
        }

        private void AbilityDeletionCheck()
        {
            for (int i = abilities.Count-1; i >= 0; i--)
            {
                if (abilities[i].IsDead)
                {
                    abilities.RemoveAt(i);
                    networkManager.DeleteLocalAbility((byte)i);
                }
            }
        }
        public void CreateAbility(Ability ability)
        {
            ability.Username = networkManager.Username;
            abilities.Add(ability);
            networkManager.SendAbility(ability, (byte)(abilities.Count-1));
        }

        private void DrawAbility(SpriteBatch spriteBatch, AbilityOutline ability, Player.ClassType playerType)
        {
            Rectangle source = new Rectangle(ability.Animation.XRecPos, ability.Animation.YRecPos, ability.Animation.Width, ability.Animation.Height);
            switch (playerType)
            {
                case Player.ClassType.Wizard:
                    if (ability.Type == AbilityOutline.AbilityType.AbilityOne)
                    {
                        spriteBatch.Draw(AssetManager.WizardIceBlock, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, 0.0f,
                            Vector2.Zero, 5.0f, SpriteEffects.None, 1.0f);
                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityTwo)
                    {
                        spriteBatch.Draw(AssetManager.WizardSpriteSheet, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, 0.0f,
                            Vector2.Zero, 5.0f, SpriteEffects.None, 1.0f);
                    }
                    break;
                case Player.ClassType.Ogre:
                    if (ability.Type == AbilityOutline.AbilityType.AbilityOne)
                    {

                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityTwo)
                    {

                    }
                    break;
                case Player.ClassType.Huntress:
                    if (ability.Type == AbilityOutline.AbilityType.AbilityOne)
                    {

                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityTwo)
                    {

                    }
                    break;
                case Player.ClassType.TimeTraveler:
                    if (ability.Type == AbilityOutline.AbilityType.AbilityOne)
                    {

                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityTwo)
                    {

                    }
                    break;
                case Player.ClassType.Assassin:
                    if (ability.Type == AbilityOutline.AbilityType.AbilityOne)
                    {

                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityTwo)
                    {

                    }
                    break;
                case Player.ClassType.Knight:
                    if (ability.Type == AbilityOutline.AbilityType.AbilityOne)
                    {

                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityTwo)
                    {

                    }
                    break;
            }
        }
    }
}
