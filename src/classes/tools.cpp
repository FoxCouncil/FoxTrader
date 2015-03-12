// tools.cpp
// 2014-10-14T22:37:10-07:00

#include "game.h"

namespace FoxTrader
{
    boost::random::mt19937 Tools::randomNumberGenerator(std::time(0));

    /* Random Funtions */
    uint32_t Tools::RandomNumber(uint32_t c_min, uint32_t c_max)
    {
        boost::random::uniform_int_distribution<> uniform(c_min, c_max);

        return uniform(Tools::randomNumberGenerator);
    }

    uint32_t Tools::RandomNumber(double_vec_t probabilities)
    {
        boost::random::discrete_distribution<> dist(probabilities.begin(), probabilities.end());

        return dist(Tools::randomNumberGenerator);
    }

    /* Randomness Randoms */
	std::string Tools::GenerateName()
	{
		std::string a_name = "";

    	// Add the prefix...
	    a_name.append(kNameGenPrefix[Tools::RandomNumber(0, 16)]);

	    // Add the suffix...
	    a_name.append(kNameGenSuffix[Tools::RandomNumber(0, 18)]);

	    // Add the stem...
	    a_name.append(kNameGenStems[Tools::RandomNumber(0, 29)]);

	    // Make the first letter capital...
	    a_name[0] = toupper(a_name[0]);

	    return a_name;
	}

	std::string Tools::GenerateFullName()
	{
	    std::string a_fullName = "";

	    a_fullName.append(Tools::GenerateName());

	    a_fullName.append(" ");

	    a_fullName.append(Tools::GenerateName());

	    return a_fullName;
	}

	std::string Tools::GenerateCatalogName()
	{
	    std::string a_catalogName = "";

	    a_catalogName.append(Tools::GenerateName());

	    a_catalogName.append("-");

	    a_catalogName.append(SSTR(rand() % 256));

	    return a_catalogName;
	}

	/* String Utilities */
    std::string Tools::Commafy(uint16_t c_rawNumber)
    {
        return Tools::Commafy(static_cast<uint32_t>(c_rawNumber));
    }

    std::string Tools::Commafy(uint32_t c_rawNumber)
    {
        return Tools::Commafy(static_cast<uint64_t>(c_rawNumber));
    }

    std::string Tools::Commafy(uint64_t c_rawNumber)
    {
        std::string numWithCommas = std::string(SSTR(c_rawNumber));

        int32_t insertPosition = numWithCommas.length() - 3;

        while (insertPosition > 0)
        {
            numWithCommas.insert(insertPosition, ",");

            insertPosition -= 3;
        }

        return numWithCommas;
    }

	/* Date and Time Stuffs */
	uint8_t Tools::DayOfTheWeek(uint16_t c_year, uint8_t c_month, uint8_t c_date)
	{
	    c_year -= c_month < 3;
	    return (c_year + c_year / 4 - c_year / 100 + c_year / 400 + kDayOfWeekTriggerTable[c_month - 1] + c_date) % 7;
	}

	uint32_t Tools::GetMilliseconds()
	{
        return SDL_GetTicks() & 1000;
	}

	// Colors
	const SDL_Color Tools::Colors::Transparent  = Tools::SDL_ColorMake(0x00, 0x00, 0x00, 0x00);

	const SDL_Color Tools::Colors::Black        = Tools::SDL_ColorMake(0x00, 0x00, 0x00);
	const SDL_Color Tools::Colors::White        = Tools::SDL_ColorMake(0xFF, 0xFF, 0xFF);

	const SDL_Color Tools::Colors::Control      = Tools::SDL_ColorMake(0xC0, 0xC0, 0xC0);

    const SDL_Color Tools::Colors::WinterWolf   = Tools::SDL_ColorMake(0xE0, 0xE0, 0xE0);
    const SDL_Color Tools::Colors::LightGrey    = Tools::SDL_ColorMake(0xCC, 0xCC, 0xCC);
    const SDL_Color Tools::Colors::Grey         = Tools::SDL_ColorMake(0x88, 0x88, 0x88);
    const SDL_Color Tools::Colors::DarkGrey     = Tools::SDL_ColorMake(0x33, 0x33, 0x33);
    const SDL_Color Tools::Colors::BlackWolf    = Tools::SDL_ColorMake(0x15, 0x15, 0x15);

	const SDL_Color Tools::Colors::Red          = Tools::SDL_ColorMake(0xFF, 0x00, 0x00);
	const SDL_Color Tools::Colors::Green        = Tools::SDL_ColorMake(0x00, 0xFF, 0x00);
	const SDL_Color Tools::Colors::Blue         = Tools::SDL_ColorMake(0x00, 0x00, 0xFF);
}
