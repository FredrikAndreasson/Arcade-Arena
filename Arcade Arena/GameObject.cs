using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    public class GameObject
    {
        protected Vector2 position; // ATALAY HAR GJORT DENNA PUBLIC FOR TILLFELET
        protected Texture2D texture; // ATALAY HAR GJORT DENNA PUBLIC FOR TILLFELET

        public GameObject(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }

        public Vector2 Position => position;
        public Texture2D Texture => texture;
    }
}
