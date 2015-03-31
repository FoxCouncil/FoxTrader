// panel.cpp
// 2015-02-19T22:09:00-07:00

#include "panel.h"
#include "renderer.h"

namespace FoxTrader
{
    Panel::Panel()
    {
        //ctor
        this->Init();
    }

    Panel::Panel(SDL_Rect c_rect)
    {
        this->Init();
        this->m_rect = c_rect;
    }

    Panel::Panel(SDL_Rect c_rect, SDL_Color a_borderColor)
    {
        this->Init();
        this->m_rect = c_rect;
        this->m_borderColor = a_borderColor;
    }

    Panel::Panel(SDL_Rect c_rect, SDL_Color a_borderColor, SDL_Color a_backgroundColor)
    {
        this->Init();
        this->m_rect = c_rect;
        this->m_borderColor = a_borderColor;
        this->m_backgroundColor = a_backgroundColor;
    }

    Panel::~Panel()
    {
        //dtor
    }

    void Panel::Draw(SDL_Renderer *c_context)
    {
        if (this->m_visible)
        {
            // Render Background
            if (this->m_backgroundColor.a != Tools::Colors::Transparent.a)
            {
                SDL_SetRenderDrawColor(c_context, this->m_backgroundColor.r, this->m_backgroundColor.g, this->m_backgroundColor.b, this->m_backgroundColor.a);
                SDL_RenderFillRect(c_context, &this->m_rect);
            }

            // Render Border
            if (this->m_borderColor.a != Tools::Colors::Transparent.a)
            {
                SDL_SetRenderDrawColor(c_context, this->m_borderColor.r, this->m_borderColor.g, this->m_borderColor.b, this->m_borderColor.a);
                SDL_RenderDrawRect(c_context, &this->m_rect);
            }
        }

        for (int i = this->m_childPanels.size(); i --> 0;)
        {
            this->m_childPanels[i]->Draw(c_context);
        }
    }

    // Events
    bool Panel::HandleEvent(SDL_Event *c_event)
    {
        for (int i = this->m_childPanels.size(); i --> 0;)
        {
            this->m_childPanels[i]->HandleEvent(c_event);
        }

        SDL_Point a_mousePosition;

        a_mousePosition.x = c_event->motion.x;
        a_mousePosition.y = c_event->motion.y;

        switch (c_event->type)
        {
            case SDL_KEYDOWN:
            {
                this->OnKeyDown(c_event);
            }
            break;

            case SDL_KEYUP:
            {
                this->OnKeyUp(c_event);
            }
            break;

            case SDL_MOUSEMOTION:
            {
                if (SDL_PointInRect(&a_mousePosition, &this->m_rect))
                {
                    if (!this->m_mouseOver)
                    {
                        this->m_mouseOver = true;
                        this->OnMouseOver(c_event);
                    }
                }
                else
                {
                    if (this->m_mouseOver)
                    {
                        this->m_mouseOver = false;
                        this->OnMouseOut(c_event);
                    }
                }

                this->OnMouseMove(c_event);
            }
            break;

            case SDL_MOUSEBUTTONUP:
            {
                if (this->m_mouseButtonHeld)
                {
                    this->m_mouseButtonHeld = false;
                }

                if (SDL_PointInRect(&a_mousePosition, &this->m_rect))
                {
                    this->OnMouseUp(c_event);
                }
            }
            break;

            case SDL_MOUSEBUTTONDOWN:
            {
                if (!this->m_mouseButtonHeld)
                {
                    this->m_mouseButtonHeld = true;
                }

                if (SDL_PointInRect(&a_mousePosition, &this->m_rect))
                {
                    this->OnMouseDown(c_event);
                }
            }
            break;
        }

        return false;
    }

    // Virtual Mouse Events
    void Panel::OnMouseOver(SDL_Event *c_event)
    {
        for (int i = this->m_mouseOverDelegates.size(); i --> 0;)
        {
            this->m_mouseOverDelegates[i](this, c_event);
        }
    }

    void Panel::OnMouseOut(SDL_Event *c_event)
    {
        for (int i = this->m_mouseOutDelegates.size(); i --> 0;)
        {
            this->m_mouseOutDelegates[i](this, c_event);
        }
    }

    void Panel::OnMouseUp(SDL_Event *c_event)
    {
        for (int i = this->m_mouseUpDelegates.size(); i --> 0;)
        {
            this->m_mouseUpDelegates[i](this, c_event);
        }
    }

    void Panel::OnMouseDown(SDL_Event *c_event)
    {
        // Panel Event Handlers
        Renderer::RequestFocus(this);

        for (int i = this->m_mouseDownDelegates.size(); i --> 0;)
        {
            this->m_mouseDownDelegates[i](this, c_event);
        }
    }

    void Panel::OnMouseMove(SDL_Event *c_event)
    {
        for (int i = this->m_mouseMoveDelegates.size(); i --> 0;)
        {
            this->m_mouseMoveDelegates[i](this, c_event);
        }
    }

    void Panel::OnMouseWheel(SDL_Event *c_event)
    {
        for (int i = this->m_mouseWheelDelegates.size(); i --> 0;)
        {
            this->m_mouseWheelDelegates[i](this, c_event);
        }
    }

    // Virtual Keyboard Events
    void Panel::OnKeyUp(SDL_Event *c_event)
    {
        for (int i = this->m_keyUpDelegates.size(); i --> 0;)
        {
            this->m_keyUpDelegates[i](this, c_event);
        }
    }

    void Panel::OnKeyDown(SDL_Event *c_event)
    {
        for (int i = this->m_keyDownDelegates.size(); i --> 0;)
        {
            this->m_keyDownDelegates[i](this, c_event);
        }
    }

    // Children Control
    void Panel::AddPanel(Panel *c_panel)
    {
        c_panel->SetParent(this);
        this->m_childPanels.push_back(c_panel);
    }

    void Panel::RemovePanel(Panel *c_panel)
    {
        this->m_childPanels.erase(std::remove(this->m_childPanels.begin(), this->m_childPanels.end(), c_panel), this->m_childPanels.end());
        c_panel->ClearParent();
    }

    void Panel::CleanPanels()
    {
        this->m_childPanels.clear();
    }

    // External Event Handlers
    // Mouse Over
    void Panel::AddMouseOverDelegate(MouseOverDelegate c_mouseOverDelegate)
    {
        this->m_mouseOverDelegates.push_back(c_mouseOverDelegate);
    }

    /*void Panel::RemoveMouseOverDelegate(MouseOverDelegate c_mouseOverDelegate)
    {
        this->m_mouseOverDelegates.erase(std::remove(this->m_mouseOverDelegates.begin(), this->m_mouseOverDelegates.end(), c_mouseOverDelegate), this->m_mouseOverDelegates.end());
    }*/

    void Panel::ClearAllMouseOverDelegates()
    {
        this->m_mouseOverDelegates.clear();
    }

    // Mouse Out
    void Panel::AddMouseOutDelegate(MouseOutDelegate c_mouseOutDelegate)
    {
        this->m_mouseOutDelegates.push_back(c_mouseOutDelegate);
    }

    /*void Panel::RemoveMouseOutDelegate(MouseOutDelegate c_mouseOutDelegate)
    {
        this->m_mouseOutDelegates.erase(std::remove(this->m_mouseOutDelegates.begin(), this->m_mouseOutDelegates.end(), c_mouseOutDelegate), this->m_mouseOutDelegates.end());
    }*/

    void Panel::ClearAllMouseOutDelegates()
    {
        this->m_mouseOutDelegates.clear();
    }

    // Mouse Up
    void Panel::AddMouseUpDelegate(MouseUpDelegate c_mouseUpDelegate)
    {
        this->m_mouseUpDelegates.push_back(c_mouseUpDelegate);
    }

    /*void Panel::RemoveMouseUpDelegate(MouseUpDelegate c_mouseUpDelegate)
    {
        this->m_mouseUpDelegates.erase(std::remove(this->m_mouseUpDelegates.begin(), this->m_mouseUpDelegates.end(), c_mouseUpDelegate), this->m_mouseUpDelegates.end());
    }*/

    void Panel::ClearAllMouseUpDelegates()
    {
        this->m_mouseUpDelegates.clear();
    }

    // Mouse Down
    void Panel::AddMouseDownDelegate(MouseUpDelegate c_mouseDownDelegate)
    {
        this->m_mouseDownDelegates.push_back(c_mouseDownDelegate);
    }

    /*void Panel::RemoveMouseDownDelegate(MouseUpDelegate c_mouseDownDelegate)
    {
        this->m_mouseDownDelegates.erase(std::remove(this->m_mouseDownDelegates.begin(), this->m_mouseDownDelegates.end(), c_mouseDownDelegate), this->m_mouseDownDelegates.end());
    }*/

    void Panel::ClearAllMouseDownDelegates()
    {
        this->m_mouseDownDelegates.clear();
    }

    // Mouse Move
    void Panel::AddMouseMoveDelegate(MouseMoveDelegate c_mouseMoveDelegate)
    {
        this->m_mouseMoveDelegates.push_back(c_mouseMoveDelegate);
    }

    /*void Panel::RemoveMouseMoveDelegate(MouseMoveDelegate c_mouseMoveDelegate)
    {
        this->m_mouseMoveDelegates.erase(std::remove(this->m_mouseMoveDelegates.begin(), this->m_mouseMoveDelegates.end(), c_mouseMoveDelegate), this->m_mouseMoveDelegates.end());
    }*/

    void Panel::ClearAllMouseMoveDelegates()
    {
        this->m_mouseMoveDelegates.clear();
    }

    // Mouse Wheel
    void Panel::AddMouseWheelDelegate(MouseWheelDelegate c_mouseWheelDelegate)
    {
        this->m_mouseWheelDelegates.push_back(c_mouseWheelDelegate);
    }

    /*void Panel::RemoveMouseWheelDelegate(MouseWheelDelegate c_mouseWheelDelegate)
    {
        this->m_mouseWheelDelegates.erase(std::remove(this->m_mouseWheelDelegates.begin(), this->m_mouseWheelDelegates.end(), c_mouseWheelDelegate), this->m_mouseWheelDelegates.end());
    }*/

    void Panel::ClearAllMouseWheelDelegates()
    {
        this->m_mouseWheelDelegates.clear();
    }

    // Key Up
    void Panel::AddKeyUpDelegate(MouseUpDelegate c_keyUpDelegate)
    {
        this->m_keyUpDelegates.push_back(c_keyUpDelegate);
    }

    /*void Panel::RemoveKeyUpDelegate(MouseUpDelegate c_keyUpDelegate)
    {
        this->m_keyUpDelegates.erase(std::remove(this->m_keyUpDelegates.begin(), this->m_keyUpDelegates.end(), c_keyUpDelegate), this->m_keyUpDelegates.end());
    }*/

    void Panel::ClearAllKeyUpDelegates()
    {
        this->m_keyUpDelegates.clear();
    }

    // Key Down
    void Panel::AddKeyDownDelegate(KeyDownDelegate c_keyDownDelegate)
    {
        this->m_keyDownDelegates.push_back(c_keyDownDelegate);
    }

    /*void Panel::RemoveKeyDownDelegate(KeyDownDelegate c_keyDownDelegate)
    {
        this->m_keyDownDelegates.erase(std::remove(this->m_keyDownDelegates.begin(), this->m_keyDownDelegates.end(), c_keyDownDelegate), this->m_keyDownDelegates.end());
    }*/

    void Panel::ClearAllKeyDownDelegates()
    {
        this->m_keyDownDelegates.clear();
    }

    // Private Methods
    void Panel::Init()
    {
        this->m_canFocus = false;
        this->m_hasFocus = false;
        this->m_visible = true;
        this->m_rect = Tools::SDL_RectMake(1, 1, 0, 0);
        this->m_borderColor = Tools::Colors::Transparent;
        this->m_backgroundColor = Tools::Colors::Control;
        this->m_mouseOver = false;
        this->m_mouseButtonHeld = false;
    }
}
