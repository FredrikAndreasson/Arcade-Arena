using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Effects
{
    class HoTEffect : CharacterExclusiveEffect
    {
        int healAmount;
        double healTimerMax = 1;
        double healTimer;

        public HoTEffect(int healAmount, double timer, DynamicObject owner) : base(owner, timer)
        {
            this.healAmount = healAmount;
            healTimer = healTimerMax;
        }

        public override void Update()
        {
            base.Update();
            healTimer -= Game1.elapsedGameTimeSeconds;
            if (healTimer <= 0)
            {
                ownerCharacter.Heal((sbyte)healAmount);
                healTimer = healTimerMax;
            }
        }
    }
}
