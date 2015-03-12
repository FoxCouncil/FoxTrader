// delegates.h
// 2015-02-22T13:45:00-07:00

#ifndef DELEGATES_H
#define DELEGATES_H

// #include <panel.h>
namespace FoxTrader
{
    class Panel;
    typedef boost::function<bool (Panel*, SDL_Event*)> MouseOverDelegate;
    typedef boost::function<bool (Panel*, SDL_Event*)> MouseOutDelegate;
    typedef boost::function<bool (Panel*, SDL_Event*)> MouseUpDelegate;
    typedef boost::function<bool (Panel*, SDL_Event*)> MouseDownDelegate;
    typedef boost::function<bool (Panel*, SDL_Event*)> MouseMoveDelegate;
    typedef boost::function<bool (Panel*, SDL_Event*)> MouseWheelDelegate;

    typedef boost::function<bool (Panel*, SDL_Event*)> KeyUpDelegate;
    typedef boost::function<bool (Panel*, SDL_Event*)> KeyDownDelegate;
}

#endif // DELEGATES_H
