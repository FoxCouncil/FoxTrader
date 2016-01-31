// system.h
// 2014-10-12T23:33:39-07:00

#ifndef SYSTEM_H_INCLUDED
#define SYSTEM_H_INCLUDED

#include "planetoid.h"

namespace FoxTrader
{
    class Galaxy;
    class System
    {
        Galaxy *m_galaxy;
        uint32_t m_index;

        std::string m_name;
        vec3_t m_position;
        std::vector<Planetoid> m_planetoids;

        public:
            System(Galaxy *a_galaxy, uint32_t a_index);
            ~System();

            // Get Methods
            std::string GetName();
            vec3_t GetPosition();

            // SpaceTime
            void Tick();
    };
}

#endif // SYSTEM_H_INCLUDED
