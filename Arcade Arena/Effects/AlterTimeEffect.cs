using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    class AlterTimeEffect : Effect
    {
        double amount;

        public AlterTimeEffect(double amount)
        {
            this.amount = amount;
        }
        public override void OnGetEffect(Character character, double timer)
        {
            base.OnGetEffect(character, timer);
            character.speedAlteration += amount;
        }

        public override void OnLossEffect(Character character)
        {
            character.speedAlteration -= amount;
            base.OnLossEffect(character);
        }
    }
}