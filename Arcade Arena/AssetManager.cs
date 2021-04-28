using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    static class AssetManager
    {

        public static Texture2D Ball { get; private set; }
        public static Texture2D TargetDummy { get; private set; }
        public static Texture2D Lava { get; private set; }

        public static Texture2D WizardSpriteSheet { get; private set; }
        public static Texture2D WizardIceBlock { get; private set; }
        public static Texture2D WizardWand { get; private set; }
        public static Texture2D WizardWandProjectile { get; private set; }

        

        public static void LoadTextures(ContentManager Content)
        {
            Ball = Content.Load<Texture2D>("Class\\Ball");
            TargetDummy = Content.Load<Texture2D>("TargetDummy");
            Lava = Content.Load<Texture2D>("LavaSprite\\Lava");
            WizardSpriteSheet = Content.Load<Texture2D>("Classes\\WizardSpriteSheet");
            WizardIceBlock = Content.Load<Texture2D>("Classes\\IceBlock");
            WizardWand = Content.Load<Texture2D>("Classes\\MageWand");
            WizardWandProjectile = Content.Load<Texture2D>("Classes\\WandProjectile");
        }
    }
}
