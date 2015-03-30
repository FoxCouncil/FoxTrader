#------------------------------------------------------------------------------#
# This makefile was generated by 'cbp2make' tool rev.147                       #
#------------------------------------------------------------------------------#


WORKDIR = %cd%

CC = gcc.exe
CXX = g++.exe
AR = ar.exe
LD = g++.exe
WINDRES = windres.exe

INC = -Ilibs\\boost
CFLAGS = -Wall -Winvalid-pch
RESINC = 
LIBDIR = 
LIB = 
LDFLAGS = -static-libstdc++ -static-libgcc

INC_DEBUG_WIN = $(INC) -Ilibs\sdl\include -Ilibs\sdl\include\SDL2 -Isrc\classes -Isrc\classes\ -Isrc\classes\renderer -Isrc\classes\renderer\gui\ -Isrc\gfx
CFLAGS_DEBUG_WIN = $(CFLAGS) -g -include "$(PROJECT_DIR)src\pch.h"
RESINC_DEBUG_WIN = $(RESINC)
RCFLAGS_DEBUG_WIN = $(RCFLAGS)
LIBDIR_DEBUG_WIN = $(LIBDIR) -Llibs\sdl\lib -Llibs\sqlite3\win32-mingw64
LIB_DEBUG_WIN = $(LIB)
LDFLAGS_DEBUG_WIN = $(LDFLAGS) -lmingw32 -lSDL2main -lSDL2 -lSDL2_image -lSDL2_ttf -lSDL2_mixer -lvorbisfile -lvorbis -logg -lfreetype -ljpeg -lpng12 -lz -mwindows -lsqlite3 -Wl,--no-undefined -lm -ldinput8 -ldxguid -ldxerr8 -luser32 -lgdi32 -lwinmm -limm32 -lole32 -loleaut32 -lshell32 -lversion -luuid
OBJDIR_DEBUG_WIN = obj\\win32\\debug
DEP_DEBUG_WIN = 
OUT_DEBUG_WIN = bin\\win32\\debug\\FoxTrader.exe

INC_RELEASE_WIN = $(INC) -Ilibs\sdl\include -Ilibs\sdl\include\SDL2 -Isrc\classes -Isrc\classes\ -Isrc\classes\renderer -Isrc\classes\renderer\gui\ -Isrc\gfx
CFLAGS_RELEASE_WIN = $(CFLAGS) -O2 -include "$(PROJECT_DIR)src\pch.h"
RESINC_RELEASE_WIN = $(RESINC)
RCFLAGS_RELEASE_WIN = $(RCFLAGS)
LIBDIR_RELEASE_WIN = $(LIBDIR) -Llibs\sdl\lib -Llibs\sqlite3\win32-mingw64
LIB_RELEASE_WIN = $(LIB)
LDFLAGS_RELEASE_WIN = $(LDFLAGS) -s -lmingw32 -lSDL2main -lSDL2 -lSDL2_image -lSDL2_ttf -lSDL2_mixer -lvorbisfile -lvorbis -logg -lfreetype -ljpeg -lpng12 -lz -mwindows -lsqlite3 -Wl,--no-undefined -lm -ldinput8 -ldxguid -ldxerr8 -luser32 -lgdi32 -lwinmm -limm32 -lole32 -loleaut32 -lshell32 -lversion -luuid
OBJDIR_RELEASE_WIN = obj\\win32\\release
DEP_RELEASE_WIN = 
OUT_RELEASE_WIN = bin\\win32\\release\\FoxTrader.exe

OBJ_DEBUG_WIN = $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\renderer.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\system.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\universe.o $(OBJDIR_DEBUG_WIN)\\src\\main.o $(OBJDIR_DEBUG_WIN)\\src\\resources.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\agent.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\galaxy.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\game.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\planetoid.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\player.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\button.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\label.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\panel.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\textbox.o $(OBJDIR_DEBUG_WIN)\\src\\classes\\tools.o

OBJ_RELEASE_WIN = $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\renderer.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\system.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\universe.o $(OBJDIR_RELEASE_WIN)\\src\\main.o $(OBJDIR_RELEASE_WIN)\\src\\resources.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\agent.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\galaxy.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\game.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\planetoid.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\player.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\button.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\label.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\panel.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\textbox.o $(OBJDIR_RELEASE_WIN)\\src\\classes\\tools.o

all: Debug_Win Release_Win

clean: clean_Debug_Win clean_Release_Win

before_Debug_Win: 
	cbp2make -in FoxTrader.cbp -out Makefile --target-case keep
	cmd /c if not exist bin\\win32\\debug md bin\\win32\\debug
	cmd /c if not exist $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer md $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer
	cmd /c if not exist $(OBJDIR_DEBUG_WIN)\\src\\classes md $(OBJDIR_DEBUG_WIN)\\src\\classes
	cmd /c if not exist $(OBJDIR_DEBUG_WIN)\\src md $(OBJDIR_DEBUG_WIN)\\src
	cmd /c if not exist $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui md $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui

after_Debug_Win: 

Debug_Win: before_Debug_Win out_Debug_Win after_Debug_Win

out_Debug_Win: before_Debug_Win $(OBJ_DEBUG_WIN) $(DEP_DEBUG_WIN)
	$(LD) $(LIBDIR_DEBUG_WIN) -o $(OUT_DEBUG_WIN) $(OBJ_DEBUG_WIN)  $(LDFLAGS_DEBUG_WIN) -mwindows $(LIB_DEBUG_WIN)

$(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\renderer.o: src\\classes\\renderer\\renderer.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\renderer\\renderer.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\renderer.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\system.o: src\\classes\\system.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\system.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\system.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\universe.o: src\\classes\\universe.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\universe.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\universe.o

$(OBJDIR_DEBUG_WIN)\\src\\main.o: src\\main.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\main.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\main.o

$(OBJDIR_DEBUG_WIN)\\src\\resources.o: src\\resources.rc
	$(WINDRES) -i src\\resources.rc -J rc -o $(OBJDIR_DEBUG_WIN)\\src\\resources.o -O coff $(INC_DEBUG_WIN)

$(OBJDIR_DEBUG_WIN)\\src\\classes\\agent.o: src\\classes\\agent.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\agent.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\agent.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\galaxy.o: src\\classes\\galaxy.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\galaxy.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\galaxy.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\game.o: src\\classes\\game.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\game.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\game.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\planetoid.o: src\\classes\\planetoid.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\planetoid.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\planetoid.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\player.o: src\\classes\\player.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\player.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\player.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\button.o: src\\classes\\renderer\\gui\\button.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\renderer\\gui\\button.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\button.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\label.o: src\\classes\\renderer\\gui\\label.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\renderer\\gui\\label.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\label.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\panel.o: src\\classes\\renderer\\gui\\panel.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\renderer\\gui\\panel.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\panel.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\textbox.o: src\\classes\\renderer\\gui\\textbox.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\renderer\\gui\\textbox.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui\\textbox.o

$(OBJDIR_DEBUG_WIN)\\src\\classes\\tools.o: src\\classes\\tools.cpp
	$(CXX) $(CFLAGS_DEBUG_WIN) $(INC_DEBUG_WIN) -c src\\classes\\tools.cpp -o $(OBJDIR_DEBUG_WIN)\\src\\classes\\tools.o

clean_Debug_Win: 
	cmd /c del /f $(OBJ_DEBUG_WIN) $(OUT_DEBUG_WIN)
	cmd /c rd bin\\win32\\debug
	cmd /c rd $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer
	cmd /c rd $(OBJDIR_DEBUG_WIN)\\src\\classes
	cmd /c rd $(OBJDIR_DEBUG_WIN)\\src
	cmd /c rd $(OBJDIR_DEBUG_WIN)\\src\\classes\\renderer\\gui

before_Release_Win: 
	cbp2make -in FoxTrader.cbp -out Makefile --target-case keep
	cmd /c if not exist bin\\win32\\release md bin\\win32\\release
	cmd /c if not exist $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer md $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer
	cmd /c if not exist $(OBJDIR_RELEASE_WIN)\\src\\classes md $(OBJDIR_RELEASE_WIN)\\src\\classes
	cmd /c if not exist $(OBJDIR_RELEASE_WIN)\\src md $(OBJDIR_RELEASE_WIN)\\src
	cmd /c if not exist $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui md $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui

after_Release_Win: 

Release_Win: before_Release_Win out_Release_Win after_Release_Win

out_Release_Win: before_Release_Win $(OBJ_RELEASE_WIN) $(DEP_RELEASE_WIN)
	$(LD) $(LIBDIR_RELEASE_WIN) -o $(OUT_RELEASE_WIN) $(OBJ_RELEASE_WIN)  $(LDFLAGS_RELEASE_WIN) -mwindows $(LIB_RELEASE_WIN)

$(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\renderer.o: src\\classes\\renderer\\renderer.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\renderer\\renderer.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\renderer.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\system.o: src\\classes\\system.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\system.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\system.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\universe.o: src\\classes\\universe.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\universe.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\universe.o

$(OBJDIR_RELEASE_WIN)\\src\\main.o: src\\main.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\main.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\main.o

$(OBJDIR_RELEASE_WIN)\\src\\resources.o: src\\resources.rc
	$(WINDRES) -i src\\resources.rc -J rc -o $(OBJDIR_RELEASE_WIN)\\src\\resources.o -O coff $(INC_RELEASE_WIN)

$(OBJDIR_RELEASE_WIN)\\src\\classes\\agent.o: src\\classes\\agent.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\agent.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\agent.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\galaxy.o: src\\classes\\galaxy.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\galaxy.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\galaxy.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\game.o: src\\classes\\game.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\game.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\game.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\planetoid.o: src\\classes\\planetoid.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\planetoid.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\planetoid.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\player.o: src\\classes\\player.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\player.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\player.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\button.o: src\\classes\\renderer\\gui\\button.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\renderer\\gui\\button.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\button.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\label.o: src\\classes\\renderer\\gui\\label.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\renderer\\gui\\label.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\label.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\panel.o: src\\classes\\renderer\\gui\\panel.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\renderer\\gui\\panel.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\panel.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\textbox.o: src\\classes\\renderer\\gui\\textbox.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\renderer\\gui\\textbox.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui\\textbox.o

$(OBJDIR_RELEASE_WIN)\\src\\classes\\tools.o: src\\classes\\tools.cpp
	$(CXX) $(CFLAGS_RELEASE_WIN) $(INC_RELEASE_WIN) -c src\\classes\\tools.cpp -o $(OBJDIR_RELEASE_WIN)\\src\\classes\\tools.o

clean_Release_Win: 
	cmd /c del /f $(OBJ_RELEASE_WIN) $(OUT_RELEASE_WIN)
	cmd /c rd bin\\win32\\release
	cmd /c rd $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer
	cmd /c rd $(OBJDIR_RELEASE_WIN)\\src\\classes
	cmd /c rd $(OBJDIR_RELEASE_WIN)\\src
	cmd /c rd $(OBJDIR_RELEASE_WIN)\\src\\classes\\renderer\\gui

.PHONY: before_Debug_Win after_Debug_Win clean_Debug_Win before_Release_Win after_Release_Win clean_Release_Win

