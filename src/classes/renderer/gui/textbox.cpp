// textbox.cpp
// 2015-03-20T23:43:00-07:00

#include "textbox.h"

namespace FoxTrader
{
    TextBox::TextBox()
    : Panel()
    {
        this->Init();
    }

    TextBox::~TextBox()
    {
    }

    void TextBox::Draw(SDL_Renderer* c_context)
    {
        Panel::Draw(c_context);
    }

    bool TextBox::HandleEvent(SDL_Event *c_event)
    {
        return false;
    }

    // Private Methods
    void TextBox::Init()
    {
        this->SetWidth(100);
        this->SetHeight(24);

        this->SetBackgroundColor(Tools::Colors::White);
        this->SetBorderColor(Tools::Colors::Black);
    }
}
