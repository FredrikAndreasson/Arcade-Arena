using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena.Managers
{
    class AbilityManager
    {
        public List<Ability> abilities;

        public AbilityManager()
        {
            abilities = new List<Ability>();

        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void CreateAbility(Ability ability)
        {
            abilities.Add(ability);

            
        }
    }
}
