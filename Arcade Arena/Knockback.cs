using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    class Knockback : Effect
    {
        double direction;
        float speed;
        public Knockback(double direction, float speed)
        {
            this.direction = direction;
            this.speed = speed;
        }
        public override void OnGetEffect(Character character, double timer)
        {
            base.OnGetEffect(character, timer);
            character.StartKnockback();
        }

        public override void Update(Character character)
        {
            character.UpdateVelocity(direction, speed);
            speed = speed / 2 + (float)timer / 4; //idk
            base.Update(character);
        }

        public override void OnLossEffect(Character character)
        {
            character.EndKnockback();
            base.OnLossEffect(character);
        }
    }
}
