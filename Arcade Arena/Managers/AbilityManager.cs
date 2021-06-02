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
                                        Rectangle rectangle = new Rectangle(new Point(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition),
                                        new Point(networkManager.ServerAbilities[i].Animation.Width * (int)Game1.SCALE, networkManager.ServerAbilities[i].Animation.Height * (int)Game1.SCALE));
                                        if (HitBoxIntersectsRotatedRectangle(rectangle, (float)networkManager.ServerAbilities[i].Direction, Vector2.Zero, playerRect))
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
                                        Rectangle rectangle = new Rectangle(new Point(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition),
                                        new Point(networkManager.ServerAbilities[i].Animation.Width * (int)Game1.SCALE, networkManager.ServerAbilities[i].Animation.Height * (int)Game1.SCALE));
                                        if (HitBoxIntersectsRotatedRectangle(rectangle, (float)networkManager.ServerAbilities[i].Direction, Vector2.Zero, playerRect))
                                        {
                                            if (!player.AbilitesHitBy.Contains(networkManager.ServerAbilities[i]))
                                            {
                                                playerManager.IsFirstPlayerHit = true;
                                                player.TakeDamage(networkManager.ServerAbilities[i].Damage);
                                                KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 30.0f, player, 1.2f);
                                                networkManager.DeleteProjectile(networkManager.ServerAbilities[i].ID, networkManager.ServerAbilities[i].Username);
                                            }
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
                                        Rectangle rectangle = new Rectangle(new Point(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition),
                                        new Point(networkManager.ServerAbilities[i].Animation.Width * (int)Game1.SCALE, networkManager.ServerAbilities[i].Animation.Height * (int)Game1.SCALE));
                                        if (HitBoxIntersectsRotatedRectangle(rectangle, (float)networkManager.ServerAbilities[i].Direction, Vector2.Zero, playerRect))
                                        {
                                            if (!player.AbilitesHitBy.Contains(networkManager.ServerAbilities[i]))
                                            {
                                                Debug.Print("hit by time traveler");
                                                playerManager.IsFirstPlayerHit = true;
                                                player.TakeDamage(networkManager.ServerAbilities[i].Damage);
                                                KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 50.0f, player, 2);
                                                player.AbilitesHitBy.Add(networkManager.ServerAbilities[i]);
                                            }
                                        }
                                        /*if (playerRect.Intersects(new Rectangle(new Point(networkManager.ServerAbilities[i].XPosition, networkManager.ServerAbilities[i].YPosition),
                                        new Point(networkManager.ServerAbilities[i].Animation.Width * (int)Game1.SCALE, networkManager.ServerAbilities[i].Animation.Height * (int)Game1.SCALE))))
                                        {
                                            if (!player.AbilitesHitBy.Contains(networkManager.ServerAbilities[i]))
                                            {
                                                playerManager.IsFirstPlayerHit = true;
                                                player.TakeDamage(networkManager.ServerAbilities[i].Damage);
                                                KnockbackEffect knockback = new KnockbackEffect(networkManager.ServerAbilities[i].Direction, 50.0f, player, 2);
                                                player.AbilitesHitBy.Add(networkManager.ServerAbilities[i]);
                                            }
                                        }*/
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

        private bool HitBoxIntersectsRotatedRectangle(Rectangle rectangle, float rotation, Vector2 origin, Rectangle nonRotatedRectangle)
        {
            List<Vector2> aRectangleAxis = new List<Vector2>();
            aRectangleAxis.Add(UpperRightCorner(rectangle, origin, rotation) - UpperLeftCorner(rectangle, origin, rotation));
            aRectangleAxis.Add(UpperRightCorner(rectangle, origin, rotation) - LowerRightCorner(rectangle, origin, rotation));
            aRectangleAxis.Add(new Vector2(nonRotatedRectangle.Left, nonRotatedRectangle.Top) - new Vector2(nonRotatedRectangle.Left, nonRotatedRectangle.Bottom));
            aRectangleAxis.Add(new Vector2(nonRotatedRectangle.Left, nonRotatedRectangle.Top) - new Vector2(nonRotatedRectangle.Right, nonRotatedRectangle.Top));

            //check for each axis if a collision occurs
            foreach (Vector2 aAxis in aRectangleAxis)
            {
                if (!IsAxisCollision(rectangle, rotation, origin, aAxis, nonRotatedRectangle))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsAxisCollision(Rectangle rectangle, float rotation, Vector2 origin, Vector2 aAxis, Rectangle otherRectangle)
        {
            //Project the corners of the Rectangle we are checking on to the Axis and
            //get a scalar value of that project we can then use for comparison
            List<int> aRectangleAScalars = new List<int>();
            aRectangleAScalars.Add(GenerateScalar(UpperLeftCorner(rectangle, origin, rotation), aAxis));
            aRectangleAScalars.Add(GenerateScalar(UpperRightCorner(rectangle, origin, rotation), aAxis));
            aRectangleAScalars.Add(GenerateScalar(LowerLeftCorner(rectangle, origin, rotation), aAxis));
            aRectangleAScalars.Add(GenerateScalar(LowerRightCorner(rectangle, origin, rotation), aAxis));

            //Project the corners of the current Rectangle on to the Axis and
            //get a scalar value of that project we can then use for comparison
            List<int> aRectangleBScalars = new List<int>();
            aRectangleBScalars.Add(GenerateScalar(new Vector2(otherRectangle.Left, otherRectangle.Top), aAxis));
            aRectangleBScalars.Add(GenerateScalar(new Vector2(otherRectangle.Right, otherRectangle.Top), aAxis));
            aRectangleBScalars.Add(GenerateScalar(new Vector2(otherRectangle.Left, otherRectangle.Bottom), aAxis));
            aRectangleBScalars.Add(GenerateScalar(new Vector2(otherRectangle.Right, otherRectangle.Bottom), aAxis));

            //Get the Maximum and Minium Scalar values for each of the Rectangles
            int aRectangleAMinimum = aRectangleAScalars.Min();
            int aRectangleAMaximum = aRectangleAScalars.Max();
            int aRectangleBMinimum = aRectangleBScalars.Min();
            int aRectangleBMaximum = aRectangleBScalars.Max();

            //If we have overlaps between the Rectangles (i.e. Min of B is less than Max of A)
            //then we are detecting a collision between the rectangles on this Axis
            if (aRectangleBMinimum <= aRectangleAMaximum && aRectangleBMaximum >= aRectangleAMaximum)
            {
                return true;
            }
            else if (aRectangleAMinimum <= aRectangleBMaximum && aRectangleAMaximum >= aRectangleBMaximum)
            {
                return true;
            }

            return false;
        }

        private int GenerateScalar(Vector2 rectangleCorner, Vector2 axis)
        {
            //projection
            float aNumerator = (rectangleCorner.X * axis.X) + (rectangleCorner.Y * axis.Y);
            float aDenominator = (axis.X * axis.X) + (axis.Y * axis.Y);
            float aDivisionResult = aNumerator / aDenominator;
            Vector2 aCornerProjected = new Vector2(aDivisionResult * axis.X, aDivisionResult * axis.Y);

            float aScalar = (axis.X * aCornerProjected.X) + (axis.Y * aCornerProjected.Y);
            return (int)aScalar;
        }

        //ger en vector2 av en punkt inlkusive rotation
        private Vector2 RotatePoint(Vector2 point, Vector2 origin, float rotation)
        {
            Vector2 aTranslatedPoint = new Vector2();
            aTranslatedPoint.X = (float)(origin.X + (point.X - origin.X) * Math.Cos(rotation)
                - (point.Y - origin.Y) * Math.Sin(rotation));
            aTranslatedPoint.Y = (float)(origin.Y + (point.Y - origin.Y) * Math.Cos(rotation)
                + (point.X - origin.X) * Math.Sin(rotation));
            return aTranslatedPoint;
        }

        //får vector2 av hörnen
        public Vector2 UpperLeftCorner(Rectangle rectangle, Vector2 origin, float rotation)
        {
            Vector2 aUpperLeft = new Vector2(rectangle.Left, rectangle.Top);
            aUpperLeft = RotatePoint(aUpperLeft, aUpperLeft + origin, rotation);
            return aUpperLeft;
        }

        public Vector2 UpperRightCorner(Rectangle rectangle, Vector2 origin, float rotation)
        {
            Vector2 aUpperRight = new Vector2(rectangle.Right, rectangle.Top);
            aUpperRight = RotatePoint(aUpperRight, aUpperRight + new Vector2(-origin.X, origin.Y), rotation);
            return aUpperRight;
        }

        public Vector2 LowerLeftCorner(Rectangle rectangle, Vector2 origin, float rotation)
        {
            Vector2 aLowerLeft = new Vector2(rectangle.Left, rectangle.Bottom);
            aLowerLeft = RotatePoint(aLowerLeft, aLowerLeft + new Vector2(origin.X, -origin.Y), rotation);
            return aLowerLeft;
        }

        public Vector2 LowerRightCorner(Rectangle rectangle, Vector2 origin, float rotation)
        {
            Vector2 aLowerRight = new Vector2(rectangle.Right, rectangle.Bottom);
            aLowerRight = RotatePoint(aLowerRight, aLowerRight + new Vector2(-origin.X, -origin.Y), rotation);
            return aLowerRight;
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
                        AbilityObstacleCollisionHandling(i);
                    }
                }
            }
        }

        private void AbilityObstacleCollisionHandling(int index)
        {
            switch (networkManager.ServerAbilities[index].Type)
            {
                case AbilityOutline.AbilityType.Projectile:
                    RemoveAbilityFromHitList(index);
                    networkManager.DeleteLocalAbility(networkManager.ServerAbilities[index].ID);
                    break;
                default:
                    break;
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
            for (int j = playerManager.clientPlayer.AbilitesHitBy.Count - 1; j >= 0; j--)
            {
                try
                {
                    if (playerManager.clientPlayer.AbilitesHitBy[j].Username == abilities[i].Username
                    && playerManager.clientPlayer.AbilitesHitBy[j].ID == abilities[i].ID)
                    {
                        try
                        {
                            playerManager.clientPlayer.AbilitesHitBy.RemoveAt(j);
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            Debug.Print(e.Message);
                        }
                    }
                } catch (ArgumentOutOfRangeException a)
                {
                    Debug.Print(a.Message);
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
                        Debug.Print("draw time traveler projectile");
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
