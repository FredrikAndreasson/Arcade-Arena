using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Arcade_Arena
{
    public class DynamicObject : GameObject
    {
        protected float speed;
        protected double direction;
        protected Vector2 velocity;
        public double speedAlteration { get; set; } //för time zone
        protected List<Effect> EffectList = new List<Effect>();

        public DynamicObject(Vector2 position, float speed, double direction) : base(position)
        {
            SpeedAlteration = 1;
            this.speed = speed;
            this.direction = direction;
        }

        public void AddEffect(Effect newEffect, bool stackableEffect)
        {
            bool alreadyHasEffect = false;
            if (!stackableEffect)
            {
                foreach (Effect effect in EffectList)
                {
                    if (effect.GetType() == effect.GetType())
                    {
                        alreadyHasEffect = true;
                        effect.timer = newEffect.timer;
                    }
                }
            }
            if (!alreadyHasEffect)
            {
                EffectList.Add(newEffect);
            }
        }

        protected void UpdateEffects()
        {
            List<Effect> tempEffectList = new List<Effect>(EffectList);
            foreach (Effect effect in tempEffectList)
            {
                effect.Update();
            }
        }

        public void RemoveEffect(Effect effect)
        {
            EffectList.Remove(effect);
        }

        public bool HasEffect(Effect effect)
        {
            if (EffectList.Contains(effect))
            {
                return true;
            }
            return false;
        }
        
        public void RemoveAllEffects()
        {
            EffectList.Clear();
        }
    }
}
