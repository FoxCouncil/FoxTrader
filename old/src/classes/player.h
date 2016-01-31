
#ifndef PLAYER_H
#define PLAYER_H

namespace FoxTrader
{
    class Player
    {
        private:
            // Player Data Members
            std::string m_name;
            vec3_t m_position;
            uint32_t m_galaxyIndex;
            uint32_t m_systemIndex;
            uint32_t m_planetIndex;

            // Economy Members
            uint64_t m_credits;

        public:
            // Constructor
            Player();

            // Data GET Methods
            std::string GetName();
            vec3_t GetPosition();
            uint32_t GetGalaxyIndex();

            // Data SET Methods
            void SetName(std::string c_name);

            // Economy Methods
            uint64_t GetCredits();
            bool DebitCredits(uint64_t c_totalDebit);
            bool CreditCredits(uint64_t c_totalCredit);
    };
}

#endif // PLAYER_H
