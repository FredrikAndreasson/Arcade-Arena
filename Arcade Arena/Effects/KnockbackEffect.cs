using System;
using System.Collections.Generic;
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
            TryToAddEffect();
        }
        public override void OnGetEffect(DynamicObject dynamicObject, double timer)
        {
            if (ownerCharacter != null)
            {
                base.OnGetEffect(dynamicObject, timer);
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
            //ownerCharacter.LastPosition = ownerCharacter.Position;
            speed = speed / 2 + (float)timer / 4; //idk
            base.Update();
            if (speed < 0.2f)
            {
                OnLossEffect();
            }
        }

        public override void OnLossEffect()
        {
            ownerCharacter.EndKnockback();
            ownerCharacter.RemoveStunEffect();
            base.OnLossEffect();
        }
    }
}