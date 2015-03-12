// label.cpp
// 2015-02-19T22:09:00-07:00

#include "label.h"
#include "renderer.h"
#include "game.h"

namespace FoxTrader
{
    Label::Label()
    : Panel()
    {
        this->Init();
    }

    Label::Label(std::string c_text)
    : Panel()
    {
        this->Init();
        this->SetText(c_text);
    }

    Label::~Label()
    {
        //dtor
        if (this->m_textTexture != NULL)
        {
            SDL_DestroyTexture(this->m_textTexture);
        }
    }

    void Label::Draw(SDL_Renderer* c_context)
    {
        if (this->m_visible)
        {
            if (this->Blink && (Tools::GetMilliseconds() & 500) > 250)
            {
                return;
            }

            Panel::Draw(c_context);

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

                if (this->AutoSize)
                {
                    this->m_rect.w = this->m_textRect.w;
                    this->m_rect.h = this->m_textRect.h;
                }

                if (this->Padding.top != 0 || this->Padding.bottom != 0)
                {
                    this->m_rect.h += this->Padding.top;
                    this->m_rect.h += this->Padding.bottom;
                }

                if (this->Padding.right != 0 || this->Padding.left != 0)
                {
                    this->m_rect.w += this->Padding.right;
                    this->m_rect.w += this->Padding.left;
                }

                this->m_textDrawRect = this->m_textRect;

                // Horizontal Alignment!
                if (this->HorizontalAlign == HorizontalAlign::Center)
                {
                    this->m_textDrawRect.x = this->m_rect.x + ((this->m_rect.w / 2) - (this->m_textRect.w /2));
                }
                else if (this->HorizontalAlign == HorizontalAlign::Right)
                {
                    this->m_textDrawRect.x = this->m_rect.x + (this->m_rect.w - this->m_textRect.w);
                }
                else
                {
                    this->m_textDrawRect.x = this->m_rect.x + this->m_textRect.x;
                }

                // Vertical Alignment!
                if (this->VerticalAlign == VerticalAlign::Middle)
                {
                    this->m_textDrawRect.y = this->m_rect.y + ((this->m_rect.h / 2) - (this->m_textRect.h /2));
                }
                else if (this->VerticalAlign == VerticalAlign::Bottom)
                {
                    this->m_textDrawRect.y = this->m_rect.y + (this->m_rect.h - this->m_textRect.h);
                }
                else
                {
                    this->m_textDrawRect.y = this->m_rect.y + this->m_textRect.y;
                }

                if (this->Padding.left != 0)
                {
                    this->m_textDrawRect.x += this->Padding.left;
                }

                if (this->Padding.top != 0)
                {
                    this->m_textDrawRect.y += this->Padding.top;
                }

                this->m_needsLayout = false;
            }

            SDL_RenderCopy(c_context, this->m_textTexture, NULL, &m_textDrawRect);
        }
    }

    void Label::SetText(std::string c_text)
    {
        this->m_text = c_text;
        this->SetNeedsLayout();
    }

    void Label::SetFont(std::string c_font)
    {
        if (Renderer::GetFont(c_font) != NULL)
        {
            this->m_font = c_font;
            this->SetNeedsLayout();
        }
    }

    // Private Methods
    void Label::Init()
    {
        // Set Defaults
        this->AutoSize = true;
        this->Blink = false;
        this->VerticalAlign = VerticalAlign::Top;
        this->HorizontalAlign = HorizontalAlign::Left;
        this->Padding = padding_t(5, 10);

        this->m_textRect = Tools::SDL_RectMake(0, 0, 0, 0);
        this->m_textTexture = NULL;
        this->m_textColor = Tools::Colors::Black;
        this->m_font = "regular_12";

        this->SetText("Label");
    }

    SDL_Texture* Label::DrawText(SDL_Renderer *c_renderer, std::string c_font, const std::string &c_message, SDL_Color c_color)
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
}
