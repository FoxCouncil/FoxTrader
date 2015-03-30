// tools.h
// 2014-10-14T22:37:10-07:00

#ifndef TOOLS_H
#define TOOLS_H

namespace FoxTrader
{
    class Tools
    {
        private:
            static boost::random::mt19937 randomNumberGenerator;

        public:
            // Randoms
            static uint32_t RandomNumber(uint32_t c_min, uint32_t c_max);
            static uint32_t RandomNumber(double_vec_t probabilities);

            // Randomness Randoms
            static std::string GenerateName();
            static std::string GenerateFullName();
            static std::string GenerateCatalogName();

            // String Utilities
            static std::string Commafy(uint16_t c_rawNumber);
            static std::string Commafy(uint32_t c_rawNumber);
            static std::string Commafy(uint64_t c_rawNumber);

            // Date and Time Stuffs
            static uint8_t DayOfTheWeek(uint16_t c_year, uint8_t c_month, uint8_t c_date);
            static uint32_t GetMilliseconds();

            // SDL Helpers
            static SDL_Color SDL_ColorMake(uint8_t c_r, uint8_t c_g, uint8_t c_b) { SDL_Color a_color = { c_r, c_g, c_b, SDL_ALPHA_OPAQUE }; return a_color; }
            static SDL_Color SDL_ColorMake(uint8_t c_r, uint8_t c_g, uint8_t c_b, uint8_t c_a) { SDL_Color a_color = { c_r, c_g, c_b, c_a }; return a_color; }
            static SDL_Rect SDL_RectMake(int c_x, int c_y) { SDL_Rect a_rect = { c_x, c_y, 1, 1 }; return a_rect; }
            static SDL_Rect SDL_RectMake(int c_x, int c_y, int c_w, int c_h) { SDL_Rect a_rect = { c_x, c_y, c_w, c_h }; return a_rect; }

            class Colors
            {
                public:
                    // Transparency with Black
                    static const SDL_Color Transparent;

                    // Boolean Color
                    static const SDL_Color Black;
                    static const SDL_Color White;

                    // Control Color
                    static const SDL_Color Control;

                    // The Variant Non-Colors
                    static const SDL_Color WinterWolf;
                    static const SDL_Color LightGrey;
                    static const SDL_Color Grey;
                    static const SDL_Color DarkGrey;
                    static const SDL_Color BlackWolf;

                    // The rainbow
                    static const SDL_Color Red;
                    static const SDL_Color Green;
                    static const SDL_Color Blue;
            };
    };
}

#endif // TOOLS_H
