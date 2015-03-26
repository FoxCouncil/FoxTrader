// universe.h
// 2014-10-12T23:32:50-07:00

#ifndef UNIVERSE_H_INCLUDED
#define UNIVERSE_H_INCLUDED

namespace FoxTrader
{
    class Galaxy;
	class Universe
	{
		// State Vars
		uint16_t m_startYear;
        time_t   m_startTime;
		std::string m_name;

		// Timey Whimey Stuff
		time_t   m_spaceTime;
		uint16_t m_spaceYear;
		uint8_t  m_spaceMonth;
		uint8_t  m_spaceDay;
		uint8_t  m_spaceHour;

		public:
		    // Matter Storage
            std::vector<Galaxy> m_galaxies;

			// The Default Constructor (The Big Bang)
			Universe();
			~Universe();
			void Tick();

			// Get Methods
			std::string GetName();

			// Time Frabric Reader Methods
			uint32_t GetSpaceTime();
			uint16_t GetSpaceYear();
			uint8_t GetSpaceMonth();
			uint8_t GetSpaceDay();
			uint8_t GetSpaceHour();
			uint8_t GetSpaceMinutes();
	};
}

#endif // UNIVERSE_H_INCLUDED
