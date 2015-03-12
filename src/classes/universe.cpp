// universe.cpp
// 2014-10-12T23:32:50-07:00

#include "universe.h"
#include "galaxy.h"

namespace FoxTrader
{
    Universe::Universe()
    {
        this->m_name = "Fox Universe";

        /* Time Functions */
        this->m_spaceTime = this->m_startTime = time(NULL); 	    // Initialize Time
        this->m_spaceTime--;										// We need atleast a second to count as day one!
        long startTime = static_cast<long int>(this->m_startTime);
        tm* timePtr = localtime(&startTime); 						// Grab A Time Pointer
        this->m_startYear = (timePtr->tm_year + 1900) + 500; 		// Save the current year, add 500 years in the future Marty!
        delete timePtr; 											// Clean up after yourself! You're a guest here!

        /* Fission */
        this->m_galaxies.reserve(kGalaxiesMax);
        for (uint32_t i = 0; i < kGalaxiesMax; ++i)
        {
            this->m_galaxies.push_back(Galaxy(this, i));
        }

        /* Put Batteries In The Clock */
        this->Tick();   											// Boot Up The Universe
    }

    Universe::~Universe()
    {
        this->m_galaxies.clear();
    }

    void Universe::Tick()
    {
        this->m_spaceTime = (uint32_t)time(NULL) - this->m_startTime;		// Remove from an ever growing number of seconds.

        uint32_t aTotalDays = (this->m_spaceTime / 24) + 1;
        uint16_t aTotalYears = 0;

        while (aTotalDays > 365)
        {
            // Parse the input to be less than or equal to 365
            aTotalDays -= 365;
            aTotalYears++;
        }

        this->m_spaceYear = this->m_startYear + aTotalYears;

        uint8_t aMonth = 0;

        while (kMonthLengths[aMonth] < aTotalDays)
        {
            // Figure out the correct month.
            aMonth++;
        }

        this->m_spaceMonth	= aMonth;
        this->m_spaceDay 	= aTotalDays - kMonthLengths[aMonth - 1];
        this->m_spaceHour	= Universe::m_spaceTime % 24;

        for (uint32_t i = 0; i < kGalaxiesMax; ++i)
        {
            this->m_galaxies[i].Tick();
        }
    }

    // Get Methods
    std::string Universe::GetName()
    {
        return this->m_name;
    }

    // Time Frabric Reader Methods
    uint32_t Universe::GetSpaceTime()
    {
        return this->m_spaceTime;
    }

    uint16_t Universe::GetSpaceYear()
    {
        return this->m_spaceYear;
    }

    uint8_t Universe::GetSpaceMonth()
    {
        return this->m_spaceMonth;
    }

    uint8_t Universe::GetSpaceDay()
    {
        return this->m_spaceDay;
    }

    uint8_t Universe::GetSpaceHour()
    {
        return this->m_spaceHour;
    }

    uint8_t Universe::GetSpaceMinutes()
    {
        uint8_t a_minutes = Tools::GetMilliseconds() / 16.5;

        if (a_minutes >= 60)
        {
            a_minutes = 59;
        }

        return a_minutes;
    }
}
