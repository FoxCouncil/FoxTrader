// pch.h
// 2014-12-23T00:38:00-07:00

#ifndef PCH_INCLUDED
#define PCH_INCLUDED

#include <iostream>
#include <vector>
#include <sstream>
#include <map>

#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <string.h>

#ifdef _WIN32
	#include <Windows.h>
#else
	#include <unistd.h>
#endif

// Boost Libs
#include <boost/format.hpp>
#include <boost/assign/std/vector.hpp>
#include <boost/random.hpp>
// #include <boost/signal.hpp>
#include <boost/function.hpp>
#include <boost/bind.hpp>

// GLEW Libs
#define GL_GLEXT_PROTOTYPES 1
#ifdef __WIN32__
#  define GLEW_STATIC
#endif
#include <GL/glew.h>
#include <SDL2/SDL_opengl_glext.h>

// SDL Libs
#include <SDL.h>
#include <SDL_image.h>
#include <SDL_ttf.h>
#include <SDL_mixer.h>

// Slow SDL Functionality
/**
 *  \brief Returns true if point resides inside a rectangle.
 */
SDL_FORCE_INLINE SDL_bool SDL_PointInRect(const SDL_Point *p, const SDL_Rect *r)
{
    return ( (p->x >= r->x) && (p->x < (r->x + r->w)) &&
             (p->y >= r->y) && (p->y < (r->y + r->h)) ) ? SDL_TRUE : SDL_FALSE;
}

// SQLITE3
#include "sqlite3.h"

#include "typedef.h"
#include "defines.h"
#include "constants.h"

#include "classes/tools.h"

#include "structs.h"

#endif // PCH_INCLUDED
