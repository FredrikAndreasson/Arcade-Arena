using Arcade_Arena.Effects;
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
                networkManager.SendAbilityUpdate(ability);
            }

            for (int i = 0; i < playerManager.clientPlayer.abilityBuffer.Count; i++)
            {
                CreateAbility(playerManager.clientPlayer.abilityBuffer[i]);
                playerManager.clientPlayer.abilityBuffer.RemoveAt(i);
                i--;
            }

            // Rect to check collision between player and projectile, will be moved to Character or removed all together 2 be replaced with pixel perfect

            Rectangle playerRect = new Rectangle(player.Position.ToPoint(), new Point((int)player.CurrentAnimation.FrameSize.X * 5, (int)player.CurrentAnimation.FrameSize.Y * 5));

            for (int i = 0; i < networkManager.ServerAbilities.Count; i++)
            {
                if (networkManager.ServerAbilities[i].Username != networkManager.Username)
                {
                    if (playerRect.Intersects(new Rectangle(new Point((int)networkManager.ServerAbilities[i].XPosition, (int)networkManager.ServerAbilities[i].YPosition),
                            new Point((int)networkManager.ServerAbilities[i].Animation.Width * 5, (int)networkManager.ServerAbilities[i].Animation.Height * 5))))
                    {
                        KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 20.0f, player, 1);
                        player.AddEffect(knockback, true);
                        player.TakeDamage(networkManager.ServerAbilities[i].Username, 5);
                        networkManager.DeleteProjectile(networkManager.ServerAbilities[i].ID, networkManager.ServerAbilities[i].Username);
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
            for (int i = 0; i < networkManager.ServerAbilities.Count; i++)
            {
                Player player = networkManager.Players.FirstOrDefault(p => p.Username == networkManager.ServerAbilities[i].Username);
                DrawAbility(spriteBatch, networkManager.ServerAbilities[i], player.Type);
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
                        spriteBatch.DrawString(AssetManager.CooldownFont, $"{ability.Username} - {ability.ID}", new Vector2(ability.XPosition, ability.YPosition + 5), Color.White);
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
