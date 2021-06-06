using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Effects
{
    class InvincibilityEffect : CharacterExclusiveEffect
    {
        public InvincibilityEffect(DynamicObject owner, double timer) : base(owner, timer)
        {
            isStackable = true;
            TryToAddEffect();
        }

        public override void OnGetEffect(DynamicObject dynamicObject, double timer)
        {
            base.OnGetEffect(dynamicObject, timer);
            ownerCharacter.AddInvincibleEffect();
        }

        public override void OnLossEffect()
        {
            base.OnLossEffect();
            ownerCharacter.RemoveInvincibleEffect();
        }
    }
}
