// button.cpp
// 2015-02-19T22:09:00-07:00

#include "button.h"

namespace FoxTrader
{
    Button::Button()
    : Label("Button")
    {
        this->Init();
    }

    Button::Button(std::string c_buttonName)
    : Label(c_buttonName)
    {
        this->Init();
    }

    Button::~Button()
    {
        //dtor
    }

    void Button::Draw(SDL_Renderer *c_context)
    {
        Label::Draw(c_context);

        if (this->m_mouseButtonHeld && this->m_mouseOver)
        {
            this->m_borderColor = Tools::Colors::DarkGrey;
            this->Padding = padding_t(6, 24, 4, 26);
            this->SetNeedsLayout();

            SDL_SetRenderDrawColor(c_context, Tools::Colors::Grey.r, Tools::Colors::Grey.g, Tools::Colors::Grey.b, Tools::Colors::Grey.a);
            SDL_RenderDrawLine(c_context, this->m_rect.x + this->m_rect.w - 1, this->m_rect.y, this->m_rect.x + this->m_rect.w - 1, this->m_rect.y + this->m_rect.h - 1);
            SDL_RenderDrawLine(c_context, this->m_rect.x, this->m_rect.y + this->m_rect.h - 1, this->m_rect.x + this->m_rect.w - 2, this->m_rect.y + this->m_rect.h - 1);

            SDL_SetRenderDrawColor(c_context, Tools::Colors::DarkGrey.r, Tools::Colors::DarkGrey.g, Tools::Colors::DarkGrey.b, Tools::Colors::DarkGrey.a);
            SDL_RenderDrawLine(c_context, this->m_rect.x + 1, this->m_rect.y + 1, this->m_rect.x + this->m_rect.w - 3, this->m_rect.y + 1);
            SDL_RenderDrawLine(c_context, this->m_rect.x + 1, this->m_rect.y + 2, this->m_rect.x + 1, this->m_rect.y + this->m_rect.h - 3);

            SDL_SetRenderDrawColor(c_context, Tools::Colors::LightGrey.r, Tools::Colors::LightGrey.g, Tools::Colors::LightGrey.b, Tools::Colors::LightGrey.a);
            SDL_RenderDrawLine(c_context, this->m_rect.x + this->m_rect.w - 2, this->m_rect.y + 1, this->m_rect.x + this->m_rect.w - 2, this->m_rect.y + this->m_rect.h - 2);
            SDL_RenderDrawLine(c_context, this->m_rect.x + 1, this->m_rect.y + this->m_rect.h - 2, this->m_rect.x + this->m_rect.w - 2, this->m_rect.y + this->m_rect.h - 2);
        }
        else
        {
            this->m_borderColor = Tools::Colors::BlackWolf;
            this->Padding = padding_t(5, 25);
            this->SetNeedsLayout();

            SDL_SetRenderDrawColor(c_context, Tools::Colors::LightGrey.r, Tools::Colors::LightGrey.g, Tools::Colors::LightGrey.b, Tools::Colors::LightGrey.a);
            SDL_RenderDrawLine(c_context, this->m_rect.x + 1, this->m_rect.y + 1, this->m_rect.x + this->m_rect.w - 3, this->m_rect.y + 1);
            SDL_RenderDrawLine(c_context, this->m_rect.x + 1, this->m_rect.y + 2, this->m_rect.x + 1, this->m_rect.y + this->m_rect.h - 3);

            SDL_SetRenderDrawColor(c_context, Tools::Colors::Grey.r, Tools::Colors::Grey.g, Tools::Colors::Grey.b, Tools::Colors::Grey.a);
            SDL_RenderDrawLine(c_context, this->m_rect.x + 2, this->m_rect.y + 2, this->m_rect.x + this->m_rect.w - 4, this->m_rect.y + 2);
            SDL_RenderDrawLine(c_context, this->m_rect.x + 2, this->m_rect.y + 2, this->m_rect.x + 2, this->m_rect.y + this->m_rect.h - 4);

            SDL_SetRenderDrawColor(c_context, Tools::Colors::DarkGrey.r, Tools::Colors::DarkGrey.g, Tools::Colors::DarkGrey.b, Tools::Colors::DarkGrey.a);
            SDL_RenderDrawLine(c_context, this->m_rect.x + this->m_rect.w - 3, this->m_rect.y + 2, this->m_rect.x + this->m_rect.w - 3, this->m_rect.y + this->m_rect.h - 3);
            SDL_RenderDrawLine(c_context, this->m_rect.x + 2, this->m_rect.y + this->m_rect.h - 3, this->m_rect.x + this->m_rect.w - 3, this->m_rect.y + this->m_rect.h - 3);

            SDL_SetRenderDrawColor(c_context, this->m_borderColor.r, this->m_borderColor.g, this->m_borderColor.b, this->m_borderColor.a);
            SDL_RenderDrawLine(c_context, this->m_rect.x + this->m_rect.w - 2, this->m_rect.y + 1, this->m_rect.x + this->m_rect.w - 2, this->m_rect.y + this->m_rect.h - 2);
            SDL_RenderDrawLine(c_context, this->m_rect.x + 1, this->m_rect.y + this->m_rect.h - 2, this->m_rect.x + this->m_rect.w - 2, this->m_rect.y + this->m_rect.h - 2);
        }
    }

    void Button::Init()
    {
        this->Padding = padding_t(5, 25);

        this->m_canFocus = true;

        this->m_backgroundColor = Tools::Colors::Control;
        this->m_borderColor = Tools::Colors::BlackWolf;

        this->SetTextColor(Tools::Colors::Black);
    }

    // Panel Overrides
    void Button::OnMouseOver(SDL_Event *c_event)
    {
        Panel::OnMouseOver(c_event);
    }

    void Button::OnMouseOut(SDL_Event *c_event)
    {
        Panel::OnMouseOut(c_event);
    }
}
