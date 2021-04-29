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

        public static Texture2D ball { get; private set; }
        public static Texture2D targetDummy { get; private set; }
        public static Texture2D lava { get; private set; }

        public static Texture2D wizardSpriteSheet { get; private set; }
        public static Texture2D wizardIceBlock { get; private set; }
        public static Texture2D wizardWand { get; private set; }
        public static Texture2D wizardWandProjectile { get; private set; }

        public static Texture2D ogreSpriteSheet {get; private set;}
        
        public static Texture2D groundSmashCrackle { get; private set; }

        public static void LoadTextures(ContentManager Content)
        {
            ball = Content.Load<Texture2D>("Class\\Ball");
            targetDummy = Content.Load<Texture2D>("TargetDummy");
            lava = Content.Load<Texture2D>("LavaSprite\\Lava");
            wizardSpriteSheet = Content.Load<Texture2D>("Classes\\WizardSpriteSheet");
            wizardIceBlock = Content.Load<Texture2D>("Classes\\IceBlock");
            wizardWand = Content.Load<Texture2D>("Classes\\MageWand");
            wizardWandProjectile = Content.Load<Texture2D>("Classes\\WandProjectile");
            ogreSpriteSheet = Content.Load<Texture2D>("Classes\\OgreSpriteSheet");
            groundSmashCrackle = Content.Load<Texture2D>("Classes\\groundSmashCrackle");
        }
    }
}
