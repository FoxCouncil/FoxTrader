// game.cpp
// 2014-12-23T00:38:00-07:00

#include "game.h"
#include "renderer.h"
#include "tools.h"
#include "player.h"
#include "galaxy.h"

namespace FoxTrader
{
    // Game State Variables
    bool Game::m_isRunning = false;
    game_state_t Game::m_currentState = GameState::MainMenu;

    // Render Object
    Renderer *Game::m_renderer = NULL;

    // Player Object
    Player *Game::m_player = NULL;

    // Universe Itself
    Universe *Game::m_universe = NULL;

    void Game::Start()
    {
        Game::TriggerError(FoxTrader::Err_Information, "Here we go!");

        Game::m_renderer = new Renderer();

        Game::m_isRunning = true;

        SDL_Event event;

        while (Game::m_isRunning)
        {
            while(SDL_PollEvent(&event))
            {
                Game::ProcessEvent(&event);
            }

            Game::Tick();

            Game::m_renderer->Tick();
        }

        Game::Clean();
    }

    void Game::ProcessEvent(SDL_Event *c_event)
    {
        switch(c_event->type)
        {
            case SDL_QUIT:
            {
                Game::m_isRunning = false;
            }
            break;

            case SDL_KEYUP:
            {
                switch(c_event->key.keysym.sym)
                {
                    case SDLK_SPACE:
                    {
                        if (Game::GetCurrentState() == GameState::MainMenu)
                        {
                            Game::StateGame();
                        }
                    }
                    break;

                    case SDLK_ESCAPE:
                    {
                        if (Game::GetCurrentState() == GameState::GameScreen)
                        {
                            Game::StateMainMenu();
                        }
                    }
                    break;

                    case SDLK_q:
                    {
                        Game::m_isRunning = false;
                    }
                    break;
                }
            }
            break;
        }

        Game::m_renderer->HandleEvent(c_event);
    }

    void Game::Tick()
    {
        if (Game::m_currentState == GameState::GameScreen)
        {
            if (Game::m_universe != NULL)
            {
                Game::m_universe->Tick();
            }
        }
    }

    void Game::Clean()
    {
        if (Game::m_isRunning)
        {
            Game::m_isRunning = false;
        }
    }

    // Access Methods
    Universe* Game::GetUniverse()
    {
        return Game::m_universe;
        /*
        if (Game::m_universe != NULL)
        {

            uint32_t a_index = Game::m_player->GetGalaxyIndex();

            return Game::m_universe->m_galaxies[a_index].GetName();
        }

        return NULL;*/
    }

    Player* Game::GetPlayer()
    {

        return Game::m_player;
    }

    // Error Handling and Logging
    void Game::TriggerError(error_t c_error_type, std::string c_message)
    {
        uint32_t a_type = 0;
        std::string a_title = "";

        switch (c_error_type)
        {
            case FoxTrader::Err_Information:
            {
                a_type = SDL_MESSAGEBOX_INFORMATION;
                a_title = "Info";
            }
            break;

            case FoxTrader::Err_Warning:
            {
                a_type = SDL_MESSAGEBOX_WARNING;
                a_title = "Warning";
            }
            break;

            case FoxTrader::Err_Error:
            {
                a_type = SDL_MESSAGEBOX_ERROR;
                a_title = "Error";
            }
            break;
        }

        if (Game::m_renderer != NULL)
        {
            SDL_ShowSimpleMessageBox(a_type, a_title.c_str(), c_message.c_str(), Game::m_renderer->GetWindowHandle());
        }
        else
        {
            SDL_ShowSimpleMessageBox(a_type, a_title.c_str(), c_message.c_str(), NULL);
        }

        if (c_error_type == FoxTrader::Err_Error)
        {
            Game::Clean();
        }
    }

    // State Control Methods
    game_state_t Game::GetCurrentState()
    {
        return Game::m_currentState;
    }

    void Game::StateMainMenu()
    {
        delete Game::m_universe;
        delete Game::m_player;

        Game::m_currentState = GameState::MainMenu;
    }

    void Game::StateGame()
    {
        Game::m_universe = new Universe();
        Game::m_player   = new Player();

        Game::m_currentState = GameState::GameScreen;
    }
}
