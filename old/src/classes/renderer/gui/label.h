// label.h
// 2015-02-19T22:09:00-07:00

#ifndef LABEL_H
#define LABEL_H

#include <panel.h>

namespace FoxTrader
{
    using namespace HorizontalAlign;
    using namespace VerticalAlign;
    class Label : public Panel
    {
        public:
            Label();
            Label(std::string c_text);
            virtual ~Label();

            // Draw Method
            virtual void Draw(SDL_Renderer *c_context);

            // Getters
            SDL_Color GetTextColor() { return this->m_textColor; }
            std::string GetText() { return this->m_text; }
            std::string GetFont() { return this->m_font; }

            // Setters
            void SetTextColor(SDL_Color c_color) { this->m_textColor = c_color; this->SetNeedsLayout(); }
            void SetText(std::string c_text);
            void SetFont(std::string c_font);

            // Properties
            bool AutoSize;
            bool Blink;
            horizontal_align_t HorizontalAlign;
            vertical_align_t VerticalAlign;
            padding_t Padding;

        protected:
            SDL_Color m_textColor;
            std::string m_text;
            std::string m_font;
            SDL_Rect m_textDrawRect;

        private:
            void Init();

            SDL_Rect m_textRect;

            SDL_Texture *m_textTexture;
            SDL_Texture* DrawText(SDL_Renderer *c_renderer, std::string c_font, const std::string &c_message, SDL_Color c_color);
    };
}

#endif // LABEL_H
