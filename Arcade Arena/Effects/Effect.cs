using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    public class Effect
    {
        public double timer;
        protected bool isStackable = false;
        protected DynamicObject owner;

        public Effect(DynamicObject owner, double timer)
        {
            OnGetEffect(owner, timer);
        }

        public virtual void Update()
        {
            timer -= Game1.elapsedGameTimeSeconds;
            if (timer <= 0)
            {
                OnLossEffect();
            }
        }

        public virtual void OnGetEffect(DynamicObject owner, double timer)
        {
            this.owner = owner;
            this.timer = timer;
            owner.AddEffect(this, isStackable);
        }

        public virtual void OnLossEffect()
        {
            owner.RemoveEffect(this);
        }
    }
}
