using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Effects
{
    public class CharacterExclusiveEffect : Effect
    {
        protected Character ownerCharacter;

        public CharacterExclusiveEffect(DynamicObject owner, double timer) : base (owner, timer)
        {

        }

        public override void OnGetEffect(DynamicObject owner, double timer)
        {
            if (owner is Character c)
            {
                base.OnGetEffect(owner, timer);
                ownerCharacter = c;
            }
            else
            {
                
                owner.RemoveEffect(this);
            }
        }
    }
}
