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


            for (int j = 0; j < networkManager.Players.Count; j++)
            {
                for (int i = 0; i < networkManager.ServerAbilities.Count; i++)
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
                                    new Point(networkManager.ServerAbilities[i].Animation.Width * (int)Game1.SCALE, networkManager.ServerAbilities[i].Animation.Height * (int)Game1.SCALE))))
                                        {
                                            //player.AddEffect(knockback, true);
                                            playerManager.IsFirstPlayerHit = true;
                                            player.TakeDamage(networkManager.ServerAbilities[i].Damage);
                                            KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 20.0f, player, 1);
                                            networkManager.DeleteProjectile(networkManager.ServerAbilities[i].ID, networkManager.ServerAbilities[i].Username);
                                        }
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
                                    new Point(networkManager.ServerAbilities[i].Animation.Width * (int)Game1.SCALE, networkManager.ServerAbilities[i].Animation.Height * (int)Game1.SCALE))))
                                        {
                                            playerManager.IsFirstPlayerHit = true;
                                            player.TakeDamage(networkManager.ServerAbilities[i].Damage);
                                            KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 30.0f, player, 1.2f);
                                            networkManager.DeleteProjectile(networkManager.ServerAbilities[i].ID, networkManager.ServerAbilities[i].Username);
                                        }
                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityOne)
                                    {
                                        if (playerRect.Intersects(new Rectangle(new Point(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition),
                                                new Point(networkManager.ServerAbilities[i].Animation.Width * (int)Game1.SCALE, networkManager.ServerAbilities[i].Animation.Height * (int)Game1.SCALE))))
                                        {
                                            playerManager.IsFirstPlayerHit = true;
                                            if (!player.AbilitesHitBy.Contains(networkManager.ServerAbilities[i]))
                                            {
                                                player.TakeDamage(networkManager.ServerAbilities[i].Damage);
                                                KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 50.0f, player, 2);
                                                player.AbilitesHitBy.Add(networkManager.ServerAbilities[i]);
                                            }
                                        }
                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityTwo)
                                    {
                                        if (playerRect.Intersects(new Rectangle(new Point(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition),
                                                new Point(networkManager.ServerAbilities[i].Animation.Width * (int)Game1.SCALE, networkManager.ServerAbilities[i].Animation.Height * (int)Game1.SCALE))))
                                        {
                                            playerManager.IsFirstPlayerHit = true;
                                            if (!player.AbilitesHitBy.Contains(networkManager.ServerAbilities[i]))
                                            {
                                                player.TakeDamage(networkManager.ServerAbilities[i].Damage);
                                                player.AbilitesHitBy.Add(networkManager.ServerAbilities[i]);
                                                BearTrapEffect bearTrapEffect = new BearTrapEffect(player, 3);
                                            }
                                        }
                                    }
                                    break;
                                case Player.ClassType.TimeTraveler:
                                    if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.Projectile)
                                    {
                                        if (playerRect.Intersects(new Rectangle(new Point(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition),
                                        new Point(networkManager.ServerAbilities[i].Animation.Width * (int)Game1.SCALE, networkManager.ServerAbilities[i].Animation.Height * (int)Game1.SCALE))))
                                        {
                                            if (!player.AbilitesHitBy.Contains(networkManager.ServerAbilities[i]))
                                            {
                                                playerManager.IsFirstPlayerHit = true;
                                                player.TakeDamage(networkManager.ServerAbilities[i].Damage);
                                                KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 50.0f, player, 2);
                                                player.AbilitesHitBy.Add(networkManager.ServerAbilities[i]);
                                            }
                                        }
                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityOne)
                                    {

                                    }
                                    else if (networkManager.ServerAbilities[i].Type == AbilityOutline.AbilityType.AbilityTwo)
                                    {
                                        Vector2 circleCenter = new Vector2(networkManager.ServerAbilities[i].XPosition + AssetManager.TimeTravelerTimeZone.Width * Game1.SCALE / 2,
                                            networkManager.ServerAbilities[i].YPosition + AssetManager.TimeTravelerTimeZone.Height * Game1.SCALE / 2);
                                        if (HitBoxIntersectsCircle(playerRect, circleCenter, AssetManager.TimeTravelerTimeZone.Width * Game1.SCALE / 2))
                                        {
                                            playerManager.IsFirstPlayerHit = true;
                                            AlterTimeEffect timeZoneEffect = new AlterTimeEffect(-0.15f, player, 0.2f);
                                        }
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

        private bool HitBoxIntersectsCircle(Rectangle hitBox, Vector2 circleCenter, float circleRadius)
        {
            float rW = (hitBox.Width) / 2;
            float rH = (hitBox.Height) / 2;

            float distX = Math.Abs(circleCenter.X - (hitBox.Left + rW));
            float distY = Math.Abs(circleCenter.Y - (hitBox.Top + rH));

            if (distX >= circleRadius + rW || distY >= circleRadius + rH)
            {
                return false;
            }
            if (distX < rW || distY < rH)
            {
                return true;
            }

            distX -= rW;
            distY -= rH;

            if (distX * distX + distY * distY < circleRadius * circleRadius)
            {
                return true;
            }
            return false;
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
            for (int i = 0; i < networkManager.ServerAbilities.Count; i++)
            {
                Player player = networkManager.Players.FirstOrDefault(p => p.Username == networkManager.ServerAbilities[i].Username);
                DrawAbility(spriteBatch, networkManager.ServerAbilities[i], player.Type);
            }
        }

        private void AbilityObstacleCollision()
        {
            for (int i = 0; i < networkManager.ServerAbilities.Count; i++)
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
                    RemoveAbilityFromHitList(i);
                    networkManager.DeleteLocalAbility(abilities[i].ID);
                    abilities.RemoveAt(i);
                    
                }
            }
        }

        private void RemoveAbilityFromHitList(int i)
        {
            for (int j = playerManager.clientPlayer.AbilitesHitBy.Count; j > 0; j--)
            {
                if (playerManager.clientPlayer.AbilitesHitBy[j].Username == abilities[i].Username
                    && playerManager.clientPlayer.AbilitesHitBy[j].ID == abilities[i].ID)
                {
                    playerManager.clientPlayer.AbilitesHitBy.RemoveAt(j);
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
                            Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityTwo)
                    {
                        spriteBatch.Draw(AssetManager.WizardSpriteSheet, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, 0.0f,
                            Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
                    }
                    break;
                case Player.ClassType.Ogre:
                    if (ability.Type == AbilityOutline.AbilityType.AbilityOne)
                    {
                        spriteBatch.Draw(AssetManager.groundSmashCrackle, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, 0.0f,
                            Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
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
                            Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
                    }
                    else if (ability.Type == AbilityOutline.AbilityType.AbilityTwo)
                    {
                        spriteBatch.Draw(AssetManager.HuntressBearTrap, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, 0.0f,
                            Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
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
                        spriteBatch.Draw(AssetManager.TimeTravelerTimeZone, new Vector2(ability.XPosition, ability.YPosition), source, Color.White, 0.0f,
                            Vector2.Zero, Game1.SCALE, SpriteEffects.None, 1.0f);
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
