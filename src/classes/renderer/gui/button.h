// button.h
// 2015-02-19T22:09:00-07:00

#ifndef BUTTON_H
#define BUTTON_H

#include <Label.h>

namespace FoxTrader
{
    class Button : public Label
    {
        public:
            /** Default constructor */
            Button();
            Button(std::string c_buttonName);
            /** Default destructor */
            virtual ~Button();

            // Draw Method
            virtual void Draw(SDL_Renderer *c_context);

            // Virtual Mouse Events
            virtual void OnMouseOver(SDL_Event *c_event);
            virtual void OnMouseOut(SDL_Event *c_event);



        protected:

        private:
            // Core Init Funtion
            void Init();
    };
}

#endif // BUTTON_H
