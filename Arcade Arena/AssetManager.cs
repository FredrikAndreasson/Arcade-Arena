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

        public static SpriteFont CooldownFont { get; private set; }
        public static Texture2D WizardAbilityIconSheet { get; private set; }

        public static Texture2D ogreSpriteSheet { get; private set; }

        public static Texture2D groundSmashCrackle { get; private set; }


        public static Texture2D arcadeArenaLogo { get; private set; }
        public static Texture2D quitButton { get; private set; }
        public static Texture2D settingsButton { get; private set; }
        public static Texture2D startButton { get; private set; }

        public static Texture2D resumeButton { get; private set; }


        public static void LoadTextures(ContentManager Content)
        {
            Ball = Content.Load<Texture2D>("Class\\Ball");
            TargetDummy = Content.Load<Texture2D>("TargetDummy");
            Lava = Content.Load<Texture2D>("LavaSprite\\Lava");
            WizardSpriteSheet = Content.Load<Texture2D>("Classes\\WizardSpriteSheet");
            WizardIceBlock = Content.Load<Texture2D>("Classes\\IceBlock");
            WizardWand = Content.Load<Texture2D>("Classes\\MageWand");
            WizardWandProjectile = Content.Load<Texture2D>("Classes\\WandProjectile");
            ogreSpriteSheet = Content.Load<Texture2D>("Classes\\OgreSpriteSheet");
            groundSmashCrackle = Content.Load<Texture2D>("Classes\\groundSmashCrackle");
            arcadeArenaLogo = Content.Load<Texture2D>("MainMenu\\arcadeArenaLogo");
            quitButton = Content.Load<Texture2D>("MainMenu\\quitButton");
            settingsButton = Content.Load<Texture2D>("MainMenu\\settingsButton");
            startButton = Content.Load<Texture2D>("MainMenu\\startButton");
            resumeButton = Content.Load<Texture2D>("MainMenu\\resumeButton");

            CooldownFont = Content.Load<SpriteFont>("Fonts\\CooldownFont");
            WizardAbilityIconSheet = Content.Load<Texture2D>("AbilityIcons\\WizardAbilityIconSheet");
        }
    }
}
