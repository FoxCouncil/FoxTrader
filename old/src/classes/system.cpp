// system.cpp
// 2014-10-12T23:33:39-07:00

#include "system.h"

namespace FoxTrader
{
    System::System(Galaxy *a_galaxy, uint32_t a_index)
    {
        this->m_galaxy = a_galaxy;
        this->m_index = a_index;

        this->m_name = Tools::GenerateName();
        this->m_position = vec3_t::Random(kSystemSizeMax);

        /* Put a ring on it */
        this->m_planetoids.reserve(kPlanetoidsMax);
        for (uint32_t i = 0; i < kPlanetoidsMax; ++i)
        {
            this->m_planetoids.push_back(Planetoid(this, i));
        }
    }

    System::~System()
    {
        this->m_galaxy = NULL;
        this->m_planetoids.clear();
    }

    std::string System::GetName()
    {
        return this->m_name;
    }

    vec3_t System::GetPosition()
    {
        return this->m_position;
    }

    void System::Tick()
    {
        for (uint32_t i = 0; i < kPlanetoidsMax; ++i)
        {
            this->m_planetoids[i].Tick();
        }
    }
}
