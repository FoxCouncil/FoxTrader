// constants.h
// 2014-10-13T00:10:31-07:00

namespace FoxTrader
{
    /* Window Defaults */
    const int SCREEN_WIDTH = 1024;
    const int SCREEN_HEIGHT = 576;

    /* SDL Defaults */
    const uint32_t SDL_IMG_FLAGS = IMG_INIT_PNG;

	/* Standard 'Thing' Sizes */
	const uint8_t  kUniverseSizeMax		= 255;	// In All Directions
	const uint8_t  kGalaxiesMax			= 128;  // Total Galaxies Max
	const uint8_t  kGalaxySizeMax		= 255;	// In All Directions
	const uint8_t  kSystemsMax			= 42;	// Total Systems Max
	const uint8_t  kSystemSizeMax		= 255;	// In All Directions
	const uint8_t  kPlanetoidsMax 		= 16;	// 16 + 1 Sun
	const uint8_t  kPlanetoidSizeMax	= 16;	// In Two Directions
	const uint16_t kHoursInAYear		= 8760;

	// Basic Enums
	namespace LoggingType
	{
        enum error_t {
            Err_Information,
            Err_Warning,
            Err_Error
        };
	}

    // Planetoid Type Constants
    enum planetoid_t {
        Star,           // 0
        BlackHole,      // 1
        Astroid,        // 2
        Technology,     // 3
        Orphan,         // 4
        GasGiant,       // 5
        Planet,         // 6
        Starbase        // 7
    };

    namespace GameState
    {
        enum game_state_t {
            MainMenu,   // 0
            GameScreen  // 1
        };
    }

    namespace HorizontalAlign
    {
        enum horizontal_align_t {
            Left,
            Center,
            Right
        };
    }

    namespace VerticalAlign
    {
        enum vertical_align_t {
            Top,
            Middle,
            Bottom
        };
    }

	// Time & Date Constants
	const std::string kDayOfWeekName[] = {
		"Sunday",       // 0
		"Monday",       // 1
		"Tuesday",      // 2
		"Wednesday",    // 3
		"Thursday",     // 4
		"Friday",       // 5
		"Saturday"      // 6
	};

	const std::string kDayOfWeekNameUpper[] = {
		"SUNDAY",       // 0
		"MONDAY",       // 1
		"TUESDAY",      // 2
		"WEDNESDAY",    // 3
		"THURSDAY",     // 4
		"FRIDAY",       // 5
		"SATURDAY"      // 6
	};

	const std::string kMonthNames[] = {
		"",
		"January",
		"February",
		"March",
		"April",
		"May",
		"June",
		"July",
		"August",
		"September",
		"October",
		"November",
		"December"
	};

	const uint16_t kMonthLengths[13]			= {0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365};
	const uint8_t  kDayOfWeekTriggerTable[13]	= {0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4};

	// Random Name Generator Constants
	const std::string kNameGenPrefix[] = {
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

	const std::string kNameGenSuffix[] = {
		"", "us", "ix", "ox", "ith",
		"ath", "um", "ator", "or", "axia",
		"imus", "ais", "itur", "orex", "o",
		"y", "er", "alt", "etrot"
	};

	const std::string kNameGenStems[] = {
		"adur", "aes", "anim", "apoll", "imac",
		"educ", "equis", "extr", "guius", "hann",
		"equi", "amora", "hum", "iace", "ille",
		"inept", "iuv", "obe", "ocul", "orbis",
		"allon", "oguod", "attum", "ayip", "olmar",
		"ackmet", "urgoe", "ywert", "iger", "ipto"
	};

}
