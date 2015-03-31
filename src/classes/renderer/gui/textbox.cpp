// textbox.cpp
// 2015-03-20T23:43:00-07:00

#include "textbox.h"
#include "renderer.h"
#include "game.h"

namespace FoxTrader
{
    TextBox::TextBox()
    : Panel()
    {
        this->Init();
    }

    TextBox::~TextBox()
    {
        SDL_DestroyTexture(this->m_textTexture);
    }

    void TextBox::Draw(SDL_Renderer* c_context)
    {
        if (this->m_visible)
        {
            Panel::Draw(c_context);

            // Draw Caret
            if (this->m_hasFocus && (Tools::GetMilliseconds() & 500) > 250)
            {
                SDL_SetRenderDrawColor(c_context, Tools::Colors::Black.r, Tools::Colors::Black.g, Tools::Colors::Black.b, Tools::Colors::Black.a);
                SDL_RenderDrawLine(c_context, this->m_rect.x + 3, this->m_rect.y + 4, this->m_rect.x + 3, this->m_rect.y + (this->m_rect.h - 4));
            }

            if (this->m_needsLayout)
            {
                if (this->m_textTexture != NULL)
                {
                    SDL_DestroyTexture(this->m_textTexture);
                }

                if (this->m_text == "")
                {
                    return;
                }

                this->m_textTexture = this->DrawText(c_context, this->m_font, this->m_text, this->m_textColor);

                SDL_QueryTexture(this->m_textTexture, NULL, NULL, &this->m_textRect.w, &this->m_textRect.h);

                this->m_textDrawRect = this->m_textRect;

                this->m_textDrawRect.x = this->m_rect.x + 4;
                this->m_textDrawRect.y = this->m_rect.y + ((this->m_rect.h / 2) - (this->m_textRect.h / 2));

                this->m_needsLayout = false;
            }

            SDL_RenderCopy(c_context, this->m_textTexture, NULL, &this->m_textDrawRect);
        }
    }

    bool TextBox::HandleEvent(SDL_Event *c_event)
    {
        return false;
    }

    // Private Methods
    SDL_Texture* TextBox::DrawText(SDL_Renderer *c_renderer, std::string c_font, const std::string &c_message, SDL_Color c_color)
    {
        if (c_renderer == NULL)
        {
            Game::TriggerError(LoggingType::Err_Error, std::string("DrawText error: c_renderer was NULL!"));
            return NULL;
        }

        TTF_Font *a_font = Renderer::GetFont(c_font);

        if (a_font == NULL)
        {
            Game::TriggerError(LoggingType::Err_Error, std::string("DrawText error: Font name [\"" + c_font + "\"] does not exist!"));
            return NULL;
        }

        SDL_Surface *a_surface = NULL;

        TTF_SetFontKerning(a_font, 0);

        a_surface = TTF_RenderText_Solid(a_font, c_message.c_str(), c_color);

        if (a_surface == NULL)
        {
            Game::TriggerError(LoggingType::Err_Error, std::string("DrawText error: a_surface was NULL! - " + std::string(SDL_GetError())));

            return NULL;
        }

        SDL_Texture *a_texture = SDL_CreateTextureFromSurface(c_renderer, a_surface);

        if (a_texture == NULL)
        {
            Game::TriggerError(LoggingType::Err_Error, std::string("DrawText error: a_texture was NULL!"));

            return NULL;
        }

        SDL_FreeSurface(a_surface);

        return a_texture;
    }

    void TextBox::Init()
    {
        this->m_canFocus = true;

        this->m_caretPosition = 0;

        this->SetWidth(100);
        this->SetHeight(24);

        this->m_textRect = Tools::SDL_RectMake(0, 0, 0, 0);
        this->m_textTexture = NULL;

        this->m_font = "regular_12";
        this->m_textColor = Tools::Colors::Black;
        this->m_text = "Foxes!";

        this->SetBackgroundColor(Tools::Colors::White);
        this->SetBorderColor(Tools::Colors::Black);

        this->m_needsLayout = true;
    }
}
