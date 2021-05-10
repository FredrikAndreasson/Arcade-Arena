﻿using System;
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
            isStackable = true;
        }
        public override void OnGetEffect(DynamicObject dynamicObject, double timer)
        {
            base.OnGetEffect(dynamicObject, timer);
            if (ownerCharacter != null)
            {
                ownerCharacter.StartKnockback();
            }
        }

        public override void Update()
        {
            ownerCharacter.UpdateVelocity(direction, speed);
            speed = speed / 2 + (float)timer / 4; //idk
            base.Update();
        }

        public override void OnLossEffect()
        {
            ownerCharacter.EndKnockback();
            base.OnLossEffect();
        }
    }
}