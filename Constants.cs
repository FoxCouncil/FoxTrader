using System;

namespace FoxTrader
{
    public static class Constants
    {
        public const long kDefaultPlayerMoney = 1000; // Kayne West, eat your heart out!
        public const int kDefaultTimeDilation = 100;  // 88 Gigawatts
        public const int kDefaultFluxCapacitance = 500;  // Marty!
        public const int kUniverseSizeMax = 255;  // In All Directions
        public const int kGalaxiesMax = 256;  // Total Galaxies Max
        public const int kGalaxySizeMax = 2048;  // In All Directions
        public const int kSystemsMax = 42;   // Total Systems Max
        public const int kSystemSizeMax = 255;  // In All Directions
        public const int kPlanetoidsMax = 16;   // 16 + 1 Sun
        public const int kPlanetoidSizeMax = 16;   // In Two Directions

        public const string kDefaultPlayerName = "Fox";

        public enum ContextState
        {
            Bumpers,
            MainMenu,
            Options,
            NewGame,
            Game,
            Paused,
            LoadGame,
            SaveGame,
            Loader,
            MAX
        }

        public enum MapZoomState
        {
            Universe,
            Galaxy,
            System
        }

        public enum SpaceShipType
        {
            Shuttle,
            MAX
        }

        public enum PlanetoidType
        {
            Star,           // 0
            BlackHole,      // 1
            Astroid,        // 2
            Technology,     // 3
            Orphan,         // 4
            GasGiant,       // 5
            Planet,         // 6
            Starbase,       // 7
            MAX
        }

        public static readonly string[] kNameGenPrefix = new string[] {
            "",		// who said we need to add a prefix?
            "bel",	// "The beautiful"
            "nar",	// "The narcasist"
            "xan",	// "The evil"
            "bell",	// "The good"
            "natr",	// "The neutral/natral"
            "ev",	// "The electrifying"
            "pr",	// "The perverted"
            "wof",	// "The dog"
            "spec", // "The star"
            "ler",  // "The slow"
            "dom",  // "The dungeon master"
            "mik",  // "The tall"
            "vit",  // "The nutricious"
            "sol",  // "The bright"
            "zum",	// "The drunk"
            "qr"	// "The british"
        };

        public static readonly string[] kNameGenSuffix = {
            "", "us", "ix", "ox", "ith",
            "ath", "um", "ator", "or", "axia",
            "imus", "ais", "itur", "orex", "o",
            "y", "er", "alt", "etrot"
        };

        public static readonly string[] kNameGenStems = {
            "adur", "aes", "anim", "apoll", "imac",
            "educ", "equis", "extr", "guius", "hann",
            "equi", "amora", "hum", "iace", "ille",
            "inept", "iuv", "obe", "ocul", "orbis",
            "allon", "oguod", "attum", "ayip", "olmar",
            "ackmet", "urgoe", "ywert", "iger", "ipto"
        };

        public const int kMaxVertices = 1024;
        public const int kMaxUIControlSize = 4096; // What's 1024 times 2?
        public const int kDefaultWinWidth = 1024; // Old skool
        public const int kDefaultWinHeight = 768;  // Old skool
        public const int kMaxTableRowColumns = 5;
        public const int kMaxMouseButtons = 5;

        // Fonts
        public const string kNormalUIFontName = "Arial";
        public const int kNormalUIFontSize = 12;
        public static readonly string[] kDefaultGameFonts = {
            kNormalUIFontName + ',' + kNormalUIFontSize,
            "Arial,16",
            "Arial,20",
            "pix Chicago,18",
            "pix Chicago,14",
            "pix Chicago,12",
            "pix Chicago,10",
            "pix Chicago,8",
            "pix Chicago,6",
        };
        public static readonly string kDefaultGameFontName = kDefaultGameFonts[0];

        // Main Menu
        public const int kMainMenuButtonWidth = 420;
        public const int kMainMenuButtonHeight = 32;

        public const float kKeyRepeatDelay = .5f;
        public const float kMouseDoubleClickSpeed = .5f;

        public const string kDefaultDebugLanguage = "qps-ploc";

        [Flags]
        public enum Pos
        {
            None = 0,
            Left = 1 << 1,
            Right = 1 << 2,
            Top = 1 << 3,
            Bottom = 1 << 4,
            CenterV = 1 << 5,
            CenterH = 1 << 6,
            Fill = 1 << 7,
            Center = CenterV | CenterH,
        }

        public enum LoggingType
        {
            Information,
            Warning,
            Error
        }
    }
}
