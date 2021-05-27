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

        public Effect(DynamicObject owner, double timer)
        {
            this.owner = owner;

            if (CheckIfEffectAdded())
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

        protected virtual bool CheckIfEffectAdded()
        {
            if (owner.AddEffect(this, isStackable))
            {
                return true;
            }
            return false;
        }

        public virtual void OnGetEffect(DynamicObject owner, double timer)
        {
            this.owner = owner;
            this.timer = timer;
        }

        public virtual void OnLossEffect()
        {
            owner.RemoveEffect(this);
        }
    }
}
