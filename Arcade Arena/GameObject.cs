using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcade_Arena
{
    public class GameObject
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
