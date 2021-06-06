using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Effects
{
    public class KnockbackEffect : CharacterExclusiveEffect
    {
        double direction;
        float speed;

        public KnockbackEffect(double direction, float speed, DynamicObject owner, double timer) : base (owner, timer)
        {
            this.direction = direction;
            this.speed = speed;
            isStackable = true;
            TryToAddEffect();
        }
        public override void OnGetEffect(DynamicObject dynamicObject, double timer)
        {
            base.OnGetEffect(dynamicObject, timer);
            if (ownerCharacter != null)
            {
                if (!ownerCharacter.Invincible)
                {
                    ownerCharacter.StartKnockback();
                    ownerCharacter.AddStunEffect();
                }
                else
                {
                    ownerCharacter.RemoveEffect(this);
                }
            }
            else
            {
                owner.RemoveEffect(this);
            }
        }

        public override void Update()
        {
            ownerCharacter.UpdateVelocity(direction, speed);
            
            speed = speed / 2 + (float)timer / 4;
            if (speed < 0.2f)
            {
                OnLossEffect();
            }
            else
            {
                base.Update();
            }
            Debug.Print("updates");
        }

        public override void OnLossEffect()
        {
            ownerCharacter.EndKnockback();
            ownerCharacter.RemoveStunEffect();
            Debug.Print("lost kb effect");
            base.OnLossEffect();
        }
    }
}