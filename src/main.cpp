// main.cpp
// 2014-10-13T02:02:44-07:00

#include "classes/game.h"
#include "classes/universe.h"

namespace FoxTrader
{
	Universe *m_universe;
	Game *m_game;

	int main(void)
	{
	    m_game = new Game();

	    m_game->Start();

	    return 0;

        /*SDL_SetRenderDrawColor( gRenderer, 0x00, 0x00, 0x00, 0xFF );

        //We'll render the string "TTF fonts are cool!" in white
        //Color is in RGBA format
        SDL_Color color = { 255, 0, 0, 255 };

        SDL_Texture *image = renderText("FoxTrader v0.1", "test.ttf", color, 64, gRenderer);

        if (image == nullptr)
        {
            TTF_Quit();
            SDL_Quit();
            return 1;
        }

        //Get the texture w/h so we can center it in the screen
        int iW, iH;
        SDL_QueryTexture(image, NULL, NULL, &iW, &iH);
        int x = SCREEN_WIDTH / 2 - iW / 2;
        int y = SCREEN_HEIGHT / 2 - iH / 2;

        //Note: This is within the program's main loop
        SDL_RenderClear(gRenderer);
        //We can draw our message as we do any other texture, since it's been
        //rendered to a texture
        renderTexture(image, gRenderer, x, y);
        SDL_RenderPresent(gRenderer);
        */

        return 0;

		while (false)
		{
	#ifdef _WIN32
			system("cls");
	#else
			system("clear");
	#endif
			// std::cout << " Simple Name: " << Tools::GenerateName() << std::endl;
			// std::cout << "   Full Name: " << Tools::GenerateFullName() << std::endl;
			// std::cout << "Catalog Name: " << Tools::GenerateCatalogName() << std::endl << std::endl;

            using namespace boost::assign;

            double_vec_t probabilities;
            probabilities += 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.1;

            for (uint32_t n=0; n<20; ++n)
                std::cout << Tools::RandomNumber(probabilities) << ' ';

			getchar();
		}

		std::cout << "FoxTrader Time V0.1!\n";
	#ifdef _WIN32
		std::cout << "Detected Windows 32-bit!\n";
	#else
		std::cout << "Detected POSIX 32-bit!\n";
	#endif

        m_universe = new Universe();

		std::cout << std::endl;

		while(true)
		{
			m_universe->Tick();

			std::cout << m_universe->GetSpaceTime() << " " << static_cast<int>(Tools::DayOfTheWeek(m_universe->GetSpaceYear(), m_universe->GetSpaceMonth(), m_universe->GetSpaceDay()));
			std::cout << "-" << kDayOfWeekName[Tools::DayOfTheWeek(m_universe->GetSpaceYear(), m_universe->GetSpaceMonth(), m_universe->GetSpaceDay())];
			std::cout << " " << static_cast<int>(m_universe->GetSpaceDay());
			std::cout << "/" << static_cast<int>(m_universe->GetSpaceMonth()) << "/" << m_universe->GetSpaceYear();
			std::cout << " " << static_cast<int>(m_universe->GetSpaceHour()) << ":00 UTC" << "\r";

	#ifdef _WIN32
	  		Sleep(1);		// sleep for 1 milliseconds
	#else
	  		usleep(1000);	// sleep for 1 milliseconds
	#endif
		}

		std::cout << std::endl;

		return 0;
	}
}

int main(int argc, char *argv[]) {
     return FoxTrader::main();
}
