// game.h
// 2014-12-23T00:38:00-07:00

#ifndef GAME_H
#define GAME_H

#include "renderer\renderer.h"
#include "universe.h"
#include "player.h"

namespace FoxTrader
{
    using namespace GameState;
    using namespace LoggingType;
    class Game
    {
        public:
            // Flow Control
            static void Start();
            static void ProcessEvent(SDL_Event *c_event);
            static void Tick();
            static void Clean();

            // Access Methods
            static Universe* GetUniverse();
            static Player* GetPlayer();

            // State Control
            static game_state_t GetCurrentState();
            static void StateMainMenu();
            static void StateGame();

            // Error Handling and Logging
            static void TriggerError(error_t c_error_type, std::string c_message);

        private:
            // Game State Variables
            static bool m_isRunning;
            static game_state_t m_currentState;

            // Render Object
            static Renderer* m_renderer;

            // Player Object
            static Player* m_player;

            // Universe Itself
            static Universe* m_universe;
    };
}

#endif // GAME_H
