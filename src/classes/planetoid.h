// planetoid.h
// 2014-10-12T23:33:08-07:00

#ifndef PLANETOID_H_INCLUDED
#define PLANETOID_H_INCLUDED

namespace FoxTrader
{
    class System;
    class Planetoid
    {
        std::string m_name;

        System *m_system;
        uint32_t m_index;

        planetoid_t p_type;

        vec3_t m_position;

        public:
            Planetoid(System *a_system, uint32_t a_index);
            ~Planetoid();

            // SpaceTime
            void Tick();
    };
}

#endif // PLANETOID_H_INCLUDED
