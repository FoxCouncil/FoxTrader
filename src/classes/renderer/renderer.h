// renderer.h
// 2014-12-24T15:30:00-07:00

#ifndef RENDERER_H
#define RENDERER_H

#include <panel.h>

namespace FoxTrader
{
    class Renderer
    {
        public:
            // ctor
            Renderer();
            ~Renderer();

            // Render Tick
            void Tick();

            // Context Method
            SDL_Renderer* GetContext();

            // Events
            void HandleEvent(SDL_Event* c_event);

            // FPS Display Control
            void ToggleFPS();

            // Drawing Methods
            //static void Text(std::string c_font, const std::string &c_message, text_align_t c_textAlignment, uint32_t c_x, uint32_t c_y);
            //static void Text(std::string c_font, const std::string &c_message, text_align_t c_textAlignment, SDL_Color c_color, uint32_t c_x, uint32_t c_y);
            //static SDL_Texture* TextToTexture(SDL_Renderer *c_renderer, std::string c_font, const std::string &c_message, SDL_Color c_color);
            //static void RenderTexture(SDL_Texture *c_texture, uint32_t c_x, uint32_t c_y, uint32_t c_w, uint32_t c_h);
            //static void RenderTexture(SDL_Texture *c_texture, uint32_t c_x, uint32_t c_y);

            // Fonts
            static TTF_Font* GetFont(std::string c_fontName);

            // Graphics

            // SDL Accessors
            SDL_Window* GetWindowHandle();

            // Focus Control
            static bool RequestFocus(Panel* c_panel);

            // Test Shit
            bool MouseUpEvent(Panel* c_target);

        private:
            // Member Variables
            std::vector<Panel *> m_panels;
            bool m_showFPS;
            uint64_t m_totalFrames;
            bool m_needsLayout;

            // Render State Methods
            void r_mainMenu();
            static void r_gameScreen();
            static void r_fpsMeter();

            // Render Target Support
            void InitSDL();

            // Fonts
            static void InitFonts();
            static std::map<std::string, TTF_Font*> m_fontMap;

            // Focus Control
            static Panel* m_focusedPanel;

            // SDL Objects
            SDL_Window* m_window;
            SDL_Renderer* m_SDLRenderer;
    };
}

#endif // RENDERER_H
