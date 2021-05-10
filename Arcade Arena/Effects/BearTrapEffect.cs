using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Effects
{
    class BearTrapEffect : CharacterExclusiveEffect
    {
        double amount;

        public BearTrapEffect(double amount, DynamicObject owner, double timer) : base(owner, timer)
        {
            this.amount = amount;
            isStackable = true;
        }

        public override void OnGetEffect(DynamicObject dynamicObject, double timer)
        {
            base.OnGetEffect(dynamicObject, timer);
            ownerCharacter.GetCanWalkStoppingEffect();
        }

        public override void OnLossEffect()
        {
            base.OnLossEffect();
            ownerCharacter.UndoCanWalkStoppingEffect();
        }
    }
}