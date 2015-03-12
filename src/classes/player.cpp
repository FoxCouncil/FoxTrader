#include "player.h"

namespace FoxTrader
{
    // Constructor
    Player::Player()
    {
        this->m_name = "Fox";

        this->m_credits = 2500;

        this->m_galaxyIndex = 0;
        this->m_systemIndex = 0;
        this->m_planetIndex = 0;
    }

    // Data GET Methods
    std::string Player::GetName()
    {
        return this->m_name;
    }

    vec3_t Player::GetPosition()
    {
        return this->m_position;
    }

    uint32_t Player::GetGalaxyIndex()
    {
        return this->m_galaxyIndex;
    }

    // Data SET Methods
    void Player::SetName(std::string c_name)
    {
        this->m_name = c_name;
    }

    // Economy Methods
    uint64_t Player::GetCredits()
    {
        return this->m_credits;
    }

    bool Player::DebitCredits(uint64_t c_totalDebit)
    {
        if (this->m_credits < c_totalDebit)
        {
            return false;
        }

        this->m_credits -= c_totalDebit;

        return true;
    }

    bool Player::CreditCredits(uint64_t c_totalCredit)
    {
        if((this->m_credits + c_totalCredit) > std::numeric_limits<uint64_t>::max())
        {
            return false;
        }

        this->m_credits += c_totalCredit;

        return true;
    }
}
