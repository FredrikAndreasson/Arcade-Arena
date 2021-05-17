using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade_Arena
{
    static class AssetManager
    {

        public static Texture2D Ball { get; private set; }
        public static Texture2D TargetDummy { get; private set; }
        public static Texture2D Lava { get; private set; }
        public static Texture2D Arena { get; private set; }

        public static Texture2D HealthBar { get; private set; }

        public static Texture2D SmallBox { get; private set; }
        public static Texture2D LargeBox { get; private set; }

        public static Texture2D WizardSpriteSheet { get; private set; }
        public static Texture2D WizardHandSpriteSheet { get; private set; }
        public static Texture2D WizardIceBlock { get; private set; }
        public static Texture2D WizardWand { get; private set; }
        public static Texture2D WizardWandProjectile { get; private set; }

        public static Texture2D TimeTravelerSpriteSheet { get; private set; }
        public static Texture2D TimeTravelerHandSpriteSheet { get; private set; }
        public static Texture2D TimeTravelerTimeZone { get; private set; }
        public static Texture2D TimeTravelerRayGun { get; private set; }
        public static Texture2D TimeTravelerRayGunLaser { get; private set; }

        public static Texture2D HuntressSpriteSheet { get; private set; }
        public static Texture2D HuntressHandSpriteSheet { get; private set; }
        public static Texture2D HuntressLongBow { get; private set; }
        public static Texture2D HuntressArrow { get; private set; }
        public static Texture2D HuntressBearTrap { get; private set; }
        public static Texture2D HuntressBoar { get; private set; }
        public static Texture2D HuntressNotes { get; private set; }

        public static Texture2D KnightSpriteSheet { get; private set; }
        public static Texture2D KnightShield { get; private set; }

        public static SpriteFont CooldownFont { get; private set; }
        public static Texture2D WizardAbilityIconSheet { get; private set; }

        public static Texture2D ogreSpriteSheet { get; private set; }

        public static Texture2D groundSmashCrackle { get; private set; }

        public static Texture2D OgreShadow { get; private set; }

        public static Texture2D WizardShadow { get; private set; }

        public static Texture2D arcadeArenaLogo { get; private set; }
        public static Texture2D quitButton { get; private set; }
        public static Texture2D settingsButton { get; private set; }
        public static Texture2D startButton { get; private set; }

        public static Texture2D resumeButton { get; private set; }


        public static Texture2D selectOgre { get; private set; }

        public static Texture2D selectHuntress { get; private set; }
        public static Texture2D selectWizard { get; private set; }


        public static void LoadTextures(ContentManager Content)
        {
            Arena = Content.Load<Texture2D>("Arena");
            Ball = Content.Load<Texture2D>("Class\\Ball");
            TargetDummy = Content.Load<Texture2D>("TargetDummy");
            Lava = Content.Load<Texture2D>("LavaSprite\\Lava");
            HealthBar = Content.Load<Texture2D>("UI\\HealthBar");

            SmallBox = Content.Load<Texture2D>("Obstacles\\SmallBox");
            LargeBox = Content.Load<Texture2D>("Obstacles\\LargeBox");

            WizardSpriteSheet = Content.Load<Texture2D>("Classes\\Wizard\\WizardSpriteSheet");
            WizardIceBlock = Content.Load<Texture2D>("Classes\\Wizard\\IceBlock");
            WizardWand = Content.Load<Texture2D>("Classes\\Wizard\\MageWand");
            WizardWandProjectile = Content.Load<Texture2D>("Classes\\Wizard\\WandProjectile");
            WizardHandSpriteSheet = Content.Load<Texture2D>("Classes\\Wizard\\WizardHandSpriteSheet");

            TimeTravelerSpriteSheet = Content.Load<Texture2D>("Classes\\TimeTraveler\\TimeTravelerSpriteSheet");
            TimeTravelerHandSpriteSheet = Content.Load<Texture2D>("Classes\\TimeTraveler\\TimeTravelerHandSpriteSheet");
            TimeTravelerRayGun = Content.Load<Texture2D>("Classes\\TimeTraveler\\RayGun");
            TimeTravelerRayGunLaser = Content.Load<Texture2D>("Classes\\TimeTraveler\\Laser");
            TimeTravelerTimeZone = Content.Load<Texture2D>("Classes\\TimeTraveler\\TimeZone");

            HuntressSpriteSheet = Content.Load<Texture2D>("Classes\\Huntress\\HuntressSpriteSheet");
            HuntressHandSpriteSheet = Content.Load<Texture2D>("Classes\\Huntress\\HuntressHandSpriteSheet");
            HuntressLongBow = Content.Load<Texture2D>("Classes\\Huntress\\LongBow");
            HuntressArrow = Content.Load<Texture2D>("Classes\\Huntress\\Arrow");
            HuntressBearTrap = Content.Load<Texture2D>("Classes\\Huntress\\BearTrap");
            HuntressBoar = Content.Load<Texture2D>("Classes\\Huntress\\Boar");
            HuntressNotes = Content.Load<Texture2D>("Classes\\Huntress\\Notes");

            KnightSpriteSheet = Content.Load<Texture2D>("Classes\\Knight\\KnightSpriteSheet");
            KnightShield = Content.Load<Texture2D>("Classes\\Knight\\Shield");

            ogreSpriteSheet = Content.Load<Texture2D>("Classes\\OgreSpriteSheet");
            groundSmashCrackle = Content.Load<Texture2D>("Classes\\groundSmashCrackle");
            OgreShadow = Content.Load<Texture2D>("Classes\\ogreShadow");
            WizardShadow = Content.Load<Texture2D>("Classes\\wizardShadow");

            arcadeArenaLogo = Content.Load<Texture2D>("MainMenu\\arcadeArenaLogo");
            quitButton = Content.Load<Texture2D>("MainMenu\\quitButton");
            settingsButton = Content.Load<Texture2D>("MainMenu\\settingsButton");
            startButton = Content.Load<Texture2D>("MainMenu\\startButton");
            resumeButton = Content.Load<Texture2D>("MainMenu\\resumeButton");

            selectHuntress = Content.Load<Texture2D>("MainMenu\\selectHuntress");
            selectOgre = Content.Load<Texture2D>("MainMenu\\selectOgre");
            selectWizard = Content.Load<Texture2D>("MainMenu\\selectWizard");



            CooldownFont = Content.Load<SpriteFont>("Fonts\\CooldownFont");
            WizardAbilityIconSheet = Content.Load<Texture2D>("AbilityIcons\\WizardAbilityIconSheet");
        }
    }
}
