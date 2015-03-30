// agent.cpp
// 2014-10-12T20:10:29-07:00

namespace FoxTrader
{
    class Agent
    {
        public:
            Agent();
            int GetAge();
            void SetAge(int age);
            char* GetName();
        private:
            char tName[21];
            int tAge;
    };

    // constructor of Cat,
    Agent::Agent()
    {
        tAge = 15;
    }

    // GetAge, Public accessor function
    // returns value of tAge member
    int Agent::GetAge()
    {
        return tAge;
    }

    // Definition of SetAge, public
    // accessor function
    void Agent::SetAge(int age)
    {
        // set member variable its age to
        // value passed in by parameter age
        tAge = age;
    }

    char* Agent::GetName()
    {
        return tName;
    }
}
