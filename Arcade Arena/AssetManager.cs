using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade_Arena
{
    static class AssetManager
    {

        public static Texture2D ball;
        public static Texture2D targetDummy;
        public static Texture2D lava;

        

        public static void LoadTextures(ContentManager Content)
        {
            ball = Content.Load<Texture2D>("Class\\Ball");
            targetDummy = Content.Load<Texture2D>("TargetDummy");
            lava = Content.Load<Texture2D>("LavaSprite\\Lava");


        }
    }
}
