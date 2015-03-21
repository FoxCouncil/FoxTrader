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
            virtual void Draw(SDL_Renderer *c_context);

            // Events
            virtual bool HandleEvent(SDL_Event *c_event);

        protected:
            std::string m_text;
            std::string m_font;

        private:
            void Init();

            SDL_Rect m_textRect;

            SDL_Texture *m_textTexture;
    };
}

#endif // TEXTBOX_H
