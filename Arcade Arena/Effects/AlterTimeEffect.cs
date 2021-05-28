using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Effects
{
    class AlterTimeEffect : Effect
    {
        double amount;

        public AlterTimeEffect(double amount, DynamicObject owner, double timer) : base (owner, timer)
        {
            this.amount = amount;
            TryToAddEffect();
        }

        public override void OnGetEffect(DynamicObject dynamicObject, double timer)
        {
            base.OnGetEffect(dynamicObject, timer);
            owner.speedAlteration += amount;
        }

        public override void OnLossEffect()
        {
            owner.speedAlteration -= amount;
            base.OnLossEffect();
        }
    }
}