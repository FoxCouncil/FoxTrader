// textbox.h
// 2015-03-20T23:43:00-07:00

#ifndef TEXTBOX_H
#define TEXTBOX_H

#include <panel.h>

namespace FoxTrader
{
    class TextBox : public Panel
    {
        public:
            TextBox();

            virtual ~TextBox();

            // Draw Method
            virtual void Draw(SDL_Renderer* c_context);

        protected:
            SDL_Color m_textColor;
            std::string m_text;
            std::string m_font;
            SDL_Rect m_textDrawRect;

            uint8_t m_caretPosition;

        private:
            SDL_Texture* DrawText(SDL_Renderer *c_renderer, std::string c_font, const std::string &c_message, SDL_Color c_color);
            void Init();

            SDL_Rect m_textRect;

            SDL_Texture *m_textTexture;

            // Internal Events
            void OnText(SDL_Event* c_event);
            void OnMouseDown(SDL_Event* c_event);
            void OnKeyDown(SDL_Event* c_event);
    };
}

#endif // TEXTBOX_H
