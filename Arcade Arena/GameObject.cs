using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class GameObject
    {
        public Vector2 position; // ATALAY HAR GJORT DENNA PUBLIC FOR TILLFELET

        public GameObject(Vector2 position)
        {
            this.position = position;
        }
    }
}
