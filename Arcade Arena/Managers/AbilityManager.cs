using Arcade_Arena.Abilites;
using Arcade_Arena.Classes;
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
            
            Rectangle playerRect = new Rectangle(player.Position.ToPoint(), new Point((int)player.CurrentAnimation.FrameSize.X * 5, (int)player.CurrentAnimation.FrameSize.Y * 5));

            foreach (Ability ability in abilities)
            {
                ability.Update();
                networkManager.SendAbilityUpdate(ability);
            }

            for (int i = 0; i < playerManager.clientPlayer.abilityBuffer.Count; i++)
            {
                CreateAbility(playerManager.clientPlayer.abilityBuffer[i]);
                CheckAbilityConditions(i);
                playerManager.clientPlayer.abilityBuffer.RemoveAt(i);
                i--;
            }            // Rect to check collision between player and projectile, will be moved to Character or removed all together 2 be replaced with pixel perfect


            for (int i = networkManager.ServerAbilities.Count - 1; i > -1; i--)
            {
                for (int j = networkManager.Players.Count - 1; j > -1 ; j--)
                {
                    if (networkManager.ServerAbilities[i].Username != networkManager.Username)
                    {
                        if (networkManager.ServerAbilities[i].Username == networkManager.Players[j].Username)
                        {

                            switch (networkManager.Players[j].Type)
                            {
                                case Player.ClassType.Wizard:
                                    if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.Projectile)
                                    {
                                        if (playerRect.Intersects(new Rectangle(new Point(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition),
                                    new Point(networkManager.ServerAbilities[i].Animation.Width * 5, networkManager.ServerAbilities[i].Animation.Height * 5))))
                                        {
                                            KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 20.0f, player, 1);
                                            player.AddEffect(knockback, true);
                                            player.TakeDamage(10, networkManager.ServerAbilities[i].Username, 5);
                                            //networkManager.DeleteProjectile(networkManager.ServerAbilities[i].ID, networkManager.ServerAbilities[i].Username);
                                        }
                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityOne)
                                    {
   
                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityTwo)
                                    {

                                    }
                                    break;
                                case Player.ClassType.Ogre:
                                    if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityOne)
                                    {

                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityTwo)
                                    {

                                    }
                                    break;
                                case Player.ClassType.Huntress:
                                    if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.Projectile)
                                    {
                                        if (playerRect.Intersects(new Rectangle(new Point(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition),
                                    new Point(networkManager.ServerAbilities[i].Animation.Width * 5, networkManager.ServerAbilities[i].Animation.Height * 5))))
                                        {
                                            KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 20.0f, player, 1);
                                            player.AddEffect(knockback, true);
                                            player.TakeDamage(10, networkManager.ServerAbilities[i].Username, 5);
                                            //networkManager.DeleteProjectile(networkManager.ServerAbilities[i].ID, networkManager.ServerAbilities[i].Username);
                                        }
                                    }
                                    if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityOne)
                                    {
                                    

                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityTwo)
                                    {
                                 

                                    }
                                    break;
                                case Player.ClassType.TimeTraveler:
                                    if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.Projectile)
                                    {
                                        if (playerRect.Intersects(new Rectangle(new Point(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition),
                                    new Point(networkManager.ServerAbilities[i].Animation.Width * 5, networkManager.ServerAbilities[i].Animation.Height * 5))))
                                        {
                                            KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 20.0f, player, 1);
                                            player.AddEffect(knockback, true);
                                            player.TakeDamage(10, networkManager.ServerAbilities[i].Username, 5);
                                            //networkManager.DeleteProjectile(networkManager.ServerAbilities[i].ID, networkManager.ServerAbilities[i].Username);
                                        }
                                    }
                                    if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityOne)
                                    {

                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityTwo)
                                    {

                                    }
                                    break;
                                case Player.ClassType.Assassin:
                                    if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityOne)
                                    {

                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityTwo)
                                    {

                                    }
                                    break;
                                case Player.ClassType.Knight:
                                    if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityOne)
                                    {

                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityTwo)
                                    {

                                    }
                                    break;
                            }
                        }  
                    }
                }
            }

            AbilityObstacleCollision();
            AbilityDeletionCheck();

        }

        private void CheckAbilityConditions(int i)
        {
            if (playerManager.clientPlayer.abilityBuffer[i] is TeleportAbility teleport)
            {
                if (HitBoxIntersectsObstacle(teleport.HitBox))
                {
                    ((Wizard)(playerManager.clientPlayer)).CancelTeleportStart();
                    abilities.Remove(playerManager.clientPlayer.abilityBuffer[i]);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = networkManager.ServerAbilities.Count - 1; i > -1; i--)
            {
                Player player = networkManager.Players.FirstOrDefault(p => p.Username == networkManager.ServerAbilities[i].Username);
                DrawAbility(spriteBatch, networkManager.ServerAbilities[i], player.Type);
            }
        }

        private void AbilityObstacleCollision()
        {
            for (int i = networkManager.ServerAbilities.Count-1; i > -1 ; i--)
            {
                Rectangle tempHitbox = new Rectangle(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition,
                    networkManager.ServerAbilities[i].Animation.Width, networkManager.ServerAbilities[i].Animation.Height);
                foreach (Obstacle obstacle in playerManager.Level.Obstacles)
                {
                    if (tempHitbox.Intersects(obstacle.HitBox()) && networkManager.ServerAbilities.Count > 0)
                    {

                        networkManager.DeleteLocalAbility(networkManager.ServerAbilities[i].ID);
                    }
                }
            }
        }

        public bool HitBoxIntersectsObstacle(Rectangle hitBox)
        {
            foreach (Obstacle obstacle in playerManager.Level.Obstacles)
            {
                if (hitBox.Intersects(obstacle.HitBox()))
                {
                    return true;
                }
            }
            return false;
        }

        private void AbilityDeletionCheck()
        {
            for (int i = abilities.Count - 1; i >= 0; i--)
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
                        spriteBatch.Draw(AssetManager.WizardWandProjectile, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, (float)ability.Direction,
                            Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
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
                        spriteBatch.Draw(AssetManager.groundSmashCrackle, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, 0.0f,
                            Vector2.Zero, 5.0f, SpriteEffects.None, 1.0f);
                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityTwo)
                    {

                    }
                    break;
                case Player.ClassType.Huntress:
                    if (ability.Type == AbilityOutline.AbilityType.Projectile)
                    {
                        spriteBatch.Draw(AssetManager.HuntressArrow, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, (float)ability.Direction,
                            Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
                    }
                    if (ability.Type == AbilityOutline.AbilityType.AbilityOne)
                    {
                        spriteBatch.Draw(AssetManager.HuntressBoar, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, 0.0f,
                            Vector2.Zero, 5.0f, SpriteEffects.None, 1.0f);
                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityTwo)
                    {
                        spriteBatch.Draw(AssetManager.HuntressBearTrap, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, 0.0f,
                            Vector2.Zero, 5.0f, SpriteEffects.None, 1.0f);
                    }
                    break;
                case Player.ClassType.TimeTraveler:
                    if (ability.Type == AbilityOutline.AbilityType.Projectile)
                    {
                        spriteBatch.Draw(AssetManager.TimeTravelerRayGunLaser, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, (float)ability.Direction,
                            Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
                    }
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
