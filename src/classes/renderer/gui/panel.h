// panel.h
// 2015-02-19T22:09:00-07:00

#ifndef PANEL_H
#define PANEL_H

#include <delegates.h>

namespace FoxTrader
{
    // The base type of a drawable on screen object
    class Panel
    {
        public:
            // Constructor and virtual deconstructor
            Panel();
            Panel(SDL_Rect a_rect);
            Panel(SDL_Rect a_rect, SDL_Color a_borderColor);
            Panel(SDL_Rect a_rect, SDL_Color a_borderColor, SDL_Color a_backgroundColor);
            virtual ~Panel();

            // Drawing Methods
            virtual void Draw(SDL_Renderer *c_context);

            // Events
            virtual bool HandleEvent(SDL_Event *c_event);

            // Virtual Mouse Events
            virtual void OnMouseOver(SDL_Event *c_event);
            virtual void OnMouseOut(SDL_Event *c_event);
            virtual void OnMouseUp(SDL_Event *c_event);
            virtual void OnMouseDown(SDL_Event *c_event);
            virtual void OnMouseMove(SDL_Event *c_event);
            virtual void OnMouseWheel(SDL_Event *c_event);

            // Virtual Keyboard Events
            virtual void OnKeyDown(SDL_Event *c_event);
            virtual void OnKeyUp(SDL_Event *c_event);

            // External Event Handlers
            // Mouse Over
            void AddMouseOverDelegate(MouseOverDelegate c_mouseOverDelegate);
            // void RemoveMouseOverDelegate(MouseOverDelegate c_mouseOverDelegate);
            void ClearAllMouseOverDelegates();

            // Mouse Out
            void AddMouseOutDelegate(MouseOutDelegate c_mouseOutDelegate);
            // void RemoveMouseOutDelegate(MouseOutDelegate c_mouseOutDelegate);
            void ClearAllMouseOutDelegates();

            // Mouse Up
            void AddMouseUpDelegate(MouseUpDelegate c_mouseUpDelegate);
            // void RemoveMouseUpDelegate(MouseUpDelegate c_mouseUpDelegate);
            void ClearAllMouseUpDelegates();

            // Mouse Down
            void AddMouseDownDelegate(MouseDownDelegate c_mouseDownDelegate);
            // void RemoveMouseDownDelegate(MouseDownDelegate c_mouseDownDelegate);
            void ClearAllMouseDownDelegates();

            // Mouse Move
            void AddMouseMoveDelegate(MouseMoveDelegate c_mouseMoveDelegate);
            // void RemoveMouseMoveDelegate(MouseMoveDelegate c_mouseMoveDelegate);
            void ClearAllMouseMoveDelegates();

            // Mouse Wheel
            void AddMouseWheelDelegate(MouseWheelDelegate c_mouseWheelDelegate);
            // void RemoveMouseWheelDelegate(MouseWheelDelegate c_mouseWheelDelegate);
            void ClearAllMouseWheelDelegates();

            // Key Up
            void AddKeyUpDelegate(KeyUpDelegate c_keyUpDelegate);
            // void RemoveKeyUpDelegate(KeyUpDelegate c_keyUpDelegate);
            void ClearAllKeyUpDelegates();

            // Key Down
            void AddKeyDownDelegate(KeyDownDelegate c_keyDownDelegate);
            // void RemoveKeyDownDelegate(KeyDownDelegate c_keyDownDelegate);
            void ClearAllKeyDownDelegates();

            // Public Getters
            SDL_Color GetBorderColor() { return this->m_borderColor; }
            SDL_Color GetBackgroundColor() { return this->m_backgroundColor; }
            int GetWidth() { return this->m_rect.w; }
            int GetHeight() { return this->m_rect.h; }
            int GetX() { return this->m_rect.x; }
            int GetY() { return this->m_rect.y; }
            SDL_Rect GetRect() { return this->m_rect; }
            bool GetCanFocus() { return this->m_canFocus; }

            // Public Setters
            void SetBorderColor(SDL_Color c_color) { this->m_borderColor = c_color; this->SetNeedsLayout(); }
            void SetBackgroundColor(SDL_Color c_color) { this->m_backgroundColor = c_color; this->SetNeedsLayout(); }
            void SetWidth(int c_width) { this->m_rect.w = c_width; this->SetNeedsLayout(); }
            void SetHeight(int c_height) { this->m_rect.h = c_height; this->SetNeedsLayout(); }
            void SetX(int c_x) { this->m_rect.x = c_x; this->SetNeedsLayout(); }
            void SetY(int c_y) { this->m_rect.y = c_y; this->SetNeedsLayout(); }
            void SetRect(SDL_Rect c_rect) { this->m_rect = c_rect; this->SetNeedsLayout(); }
            void SetNeedsLayout() { this->m_needsLayout = true; }
            void SetParent(Panel *c_parentPanel) { this->m_parentPanel = c_parentPanel; }
            void ClearParent() { this->m_parentPanel = NULL; }
            void Focus() { this->m_hasFocus = true; this->SetNeedsLayout(); }
            void Blur() { this->m_hasFocus = false; this->SetNeedsLayout(); }

            // Children Control
            void AddPanel(Panel *c_panel);
            void RemovePanel(Panel *c_panel);
            void CleanPanels();

        protected:
            bool m_visible;
            bool m_needsLayout;
            bool m_mouseOver;
            bool m_mouseButtonHeld;
            bool m_canFocus;
            bool m_hasFocus;

            Panel *m_parentPanel;

            SDL_Rect m_rect;
            SDL_Color m_borderColor;
            SDL_Color m_backgroundColor;

        private:
            // Event Handlers
            std::vector<MouseOverDelegate> m_mouseOverDelegates;
            std::vector<MouseOutDelegate> m_mouseOutDelegates;
            std::vector<MouseUpDelegate> m_mouseUpDelegates;
            std::vector<MouseDownDelegate> m_mouseDownDelegates;
            std::vector<MouseMoveDelegate> m_mouseMoveDelegates;
            std::vector<MouseWheelDelegate> m_mouseWheelDelegates;
            std::vector<KeyUpDelegate> m_keyUpDelegates;
            std::vector<KeyDownDelegate> m_keyDownDelegates;

            // Child Panels
            std::vector<Panel *> m_childPanels;

            // Core Init Funtion
            void Init();
    };
}

#endif // PANEL_H
