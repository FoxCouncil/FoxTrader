// planetoid.cpp
// 2014-10-12T23:33:08-07:00

#include "planetoid.h"
#include "system.h"

namespace FoxTrader
{
    using namespace boost::assign;

    Planetoid::Planetoid(System *a_system, uint32_t a_index)
    {
        // Store
        this->m_system = a_system;
        this->m_index = a_index;

        // Building blocks
        /*uint8_t a_radiusMax;
        a_radiusMax = 5;*/

        double_vec_t probabilities;

        if (this->m_index == 0)
        {
            probabilities += 1.0, 0.0001, 0.001, 0.01, 0.0001;
        }
        else
        {
            probabilities += 0.00001, 0.0, 0.0001, 0.0, 0.0, 0.2, 1.0, 0.2;
        }

        p_type = (planetoid_t)Tools::RandomNumber(probabilities);

        // Create It's Name
        this->m_name.append(this->m_system->GetName());
        this->m_name.append("-");
        this->m_name.append(SSTR(a_index + 1));

        this->m_position = vec3_t::Random(kPlanetoidSizeMax);
    }

    Planetoid::~Planetoid()
    {
        this->m_system = NULL;
    }

    void Planetoid::Tick()
    {
        // Ping?
    }
}
