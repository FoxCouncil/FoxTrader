// galaxy.h
// 2014-10-12T23:27:26-07:00

#ifndef GALAXY_H_INCLUDED
#define GALAXY_H_INCLUDED

#include "universe.h"
#include "system.h"

namespace FoxTrader
{
    class Galaxy
    {
        Universe *m_universe;
        uint32_t m_index;
        std::string m_name;
        vec3_t m_position;

        // Gravity
        std::vector<System> m_systems;

        public:
            Galaxy(Universe *a_universe, uint32_t a_index);
            ~Galaxy();

            // Get Methods
            std::vector<System>* GetSystems();
            uint32_t GetIndex();
            std::string GetName();

            // SpaceTime
            void Tick();
    };
}

#endif // GALAXY_H_INCLUDED
