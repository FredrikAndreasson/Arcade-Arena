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
        protected DynamicObject owner;
        protected bool isStackable;
        bool shouldAffEffect = false;

        public Effect(DynamicObject owner, double timer)
        {
            this.timer = timer;
            this.owner = owner;
        }

        protected void TryToAddEffect()
        {
            if (owner.AddEffect(this, isStackable))
            {
                OnGetEffect(owner, timer);
            }
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
            
        }

        public virtual void OnLossEffect()
        {
            owner.RemoveEffect(this);
        }
    }
}
