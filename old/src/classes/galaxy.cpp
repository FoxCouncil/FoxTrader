// galaxy.cpp
// 2014-10-12T23:27:26-07:00

#include "../pch.h"
#include "galaxy.h"

namespace FoxTrader
{
    Galaxy::Galaxy(Universe *a_universe, uint32_t a_index)
    {
        this->m_universe = a_universe;
        this->m_index = a_index;

        this->m_name = Tools::GenerateName();
        this->m_position = vec3_t::Random(kGalaxySizeMax);

        /* Law of attraction */
        this->m_systems.reserve(kSystemsMax);
        for (uint32_t i = 0; i < kSystemsMax; ++i)
        {
            this->m_systems.push_back(System(this, i));
        }
    }

    Galaxy::~Galaxy()
    {
        this->m_universe = NULL;
        this->m_systems.clear();
    }

    std::vector<System>* Galaxy::GetSystems()
    {
        return &this->m_systems;
    }

    uint32_t Galaxy::GetIndex()
    {
        return this->m_index;
    }

    std::string Galaxy::GetName()
    {
        return this->m_name;
    }

    void Galaxy::Tick()
    {
        for (uint32_t i = 0; i < kSystemsMax; ++i)
        {
            this->m_systems[i].Tick();
        }
    }
}
