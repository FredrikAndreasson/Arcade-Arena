using Arcade_Arena.Abilites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Arcade_Arena.Classes
{
    public class Huntress : ProjectileCharacter
    {

        double bearTrapCooldown;
        double bearTrapMaxCooldown = 10;

        double boarCooldown;
        double boarMaxCooldown = 10;

        List<BearTrap> bearTraps = new List<BearTrap>();

        public Huntress(Vector2 position, Texture2D texture, float speed, double direction) : base(position, speed, direction)
        {
            
        }

        private void UpdateCooldowns()
        {
            bearTrapCooldown -= Game1.elapsedGameTimeSeconds;
            boarCooldown -= Game1.elapsedGameTimeSeconds;
        }

        private void BearTrapAbility()
        {
            bearTrapCooldown = bearTrapMaxCooldown;
        }

        private void BoarAbility()
        {

        }

        public void DespawnBearTrap(BearTrap trap)
        {
            bearTraps.Remove(trap);
        }
    }
}
