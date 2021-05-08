using Arcade_Arena.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Managers
{
    class AbilityManager
    {
        NetworkManager networkManager;
        PlayerManager playerManager;


        //The server will treat any and all projectiles as abilities
        public List<Ability> abilities;


        private byte IDsum;

        public AbilityManager(NetworkManager networkManager, PlayerManager playerManager)
        {
            this.networkManager = networkManager;
            this.playerManager = playerManager;
            abilities = new List<Ability>();
            
        }

        public void Update(Character player)
        {
            foreach (Ability ability in abilities)
            {
                ability.Update();
                networkManager.SendAbilityUpdate(ability, ability.ID);
            }

            for (int i = 0; i < playerManager.clientPlayer.abilityBuffer.Count; i++)
            {
                CreateAbility(playerManager.clientPlayer.abilityBuffer[i]);
                playerManager.clientPlayer.abilityBuffer.RemoveAt(i);
                i--;
            }

            // Rect to check collision between player and projectile, will be moved to Character or removed all together 2 be replaced with pixel perfect

            Rectangle playerRect = new Rectangle(player.Position.ToPoint(), new Point((int)player.CurrentAnimation.FrameSize.X * 5, (int)player.CurrentAnimation.FrameSize.Y * 5));

            for (int i = 0; i < networkManager.Players.Count; i++)
            {
                if (networkManager.Players[i].Username != networkManager.Username)
                {
                    Player players = networkManager.Players[i];
                    for (int x = 0; x < players.abilities.Count; x++)
                    {
                        if (playerRect.Intersects(new Rectangle(new Point(players.abilities[x].XPosition, players.abilities[x].YPosition), new Point((int)players.abilities[x].Animation.Width * 5, (int)players.abilities[x].Animation.Height* 5))))
                        {
                            player.TakeDamage();
                            networkManager.DeleteProjectile(players.abilities[x].ID, players.abilities[x].UserName);
                            Debug.WriteLine($"hmm {player.GetHealth()}");
                        }
                    }
                }
            }

            AbilityDeletionCheck();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Ability ability in abilities)
            {
                //ability.Draw(spriteBatch);
            }
            for (int i = 0; i < networkManager.Players.Count; i++)
            {
                //if (networkManager.Players[i].Username != networkManager.Username)
                //{
                    Player player = networkManager.Players[i];
                    for (int x = 0; x < player.abilities.Count; x++)
                    {
                        DrawAbility(spriteBatch, player.abilities[x], player.Type);
                    }
                //}
            }
        }

        private void AbilityDeletionCheck()
        {
            for (int i = abilities.Count-1; i >= 0; i--)
            {
                if (abilities[i].IsDead)
                {
                    networkManager.DeleteLocalAbility(abilities[i].ID);
                    abilities.RemoveAt(i);
                }
            }
        }
        public void CreateAbility(Ability ability)
        {
            ability.Username = networkManager.Username;
            ability.ID = IDGenerator();
            abilities.Add(ability);
            networkManager.SendAbility(ability, ability.ID);
        }
        public byte IDGenerator()
        {
            if (IDsum >= 255)
            { IDsum = 0; }
            else { IDsum++; }
            return IDsum;
        }

        private void DrawAbility(SpriteBatch spriteBatch, AbilityOutline ability, Player.ClassType playerType)
        {
            Rectangle source = new Rectangle(ability.Animation.XRecPos, ability.Animation.YRecPos, ability.Animation.Width, ability.Animation.Height);
            switch (playerType)
            {
                case Player.ClassType.Wizard:
                    if (ability.Type == AbilityOutline.AbilityType.Projectile)
                    {
                        spriteBatch.Draw(AssetManager.WizardWandProjectile, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, 0.0f,
                            Vector2.Zero, 6.0f, SpriteEffects.None, 1.0f);
                        spriteBatch.DrawString(AssetManager.CooldownFont, $"{ability.UserName} - {ability.ID}", new Vector2(ability.XPosition, ability.YPosition + 5), Color.White);
                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityOne)
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
