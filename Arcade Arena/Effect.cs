using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    class Effect
    {
        public double timer;

        public Effect()
        {
        }

        public virtual void Update(Character character)
        {
            timer -= Game1.elapsedGameTimeSeconds;
            if (timer <= 0)
            {
                OnLossEffect(character);
            }
        }

        public virtual void OnGetEffect(Character character, double timer)
        {
            this.timer = timer;
            character.EffectList.Add(this);
        }

        public virtual void OnLossEffect(Character character)
        {
            character.EffectList.Remove(this);
        }
    }
}
