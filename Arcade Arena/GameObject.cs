using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    class GameObject
    {
        public Vector2 position; // ATALAY HAR GJORT DENNA PUBLIC FOR TILLFELET
        public Texture2D texture; // ATALAY HAR GJORT DENNA PUBLIC FOR TILLFELET

        public GameObject(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }
    }
}
