using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Effects
{
    class BurningEffect : CharacterExclusiveEffect
    {
        int damage;
        double burnDamageTimerMax = 1;
        double burnDamageTimer;

        public BurningEffect(int damage, double timer, DynamicObject owner) : base (owner, timer)
        {
            this.damage = damage;
            burnDamageTimer = burnDamageTimerMax;
        }

        public override void Update()
        {
            base.Update();
            burnDamageTimer -= Game1.elapsedGameTimeSeconds;
            if  (burnDamageTimer <= 0)
            {
                ownerCharacter.TakeDamage(damage);
                burnDamageTimer = burnDamageTimerMax;
            }
        }
    }
}
