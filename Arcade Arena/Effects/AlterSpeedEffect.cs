using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Effects
{
    class AlterSpeedEffect : CharacterExclusiveEffect
    {
        float amount;

        public AlterSpeedEffect(float amount, double timer, DynamicObject owner) : base(owner, timer)
        {
            this.amount = amount;
            isStackable = true;
            TryToAddEffect();
        }

        public override void OnGetEffect(DynamicObject dynamicObject, double timer)
        {
            Debug.Print("added");
            base.OnGetEffect(dynamicObject, timer);
            ownerCharacter.ChangeSpeed(amount);
        }

        public override void OnLossEffect()
        {
            Debug.Print("Lossed");
            ownerCharacter.ChangeSpeed(-amount);
            base.OnLossEffect();
        }
    }
}
