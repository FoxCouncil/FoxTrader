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

            // Render Tick
            void Tick();

            // Context Method
            bool SetContext(SDL_Renderer *c_context);
            SDL_Renderer* GetContext();

            // Events
            void HandleEvent(SDL_Event *c_event);

            // FPS Display Control
		    void ToggleFPS();

            // Drawing Methods
            //static void Text(std::string c_font, const std::string &c_message, text_align_t c_textAlignment, uint32_t c_x, uint32_t c_y);
            //static void Text(std::string c_font, const std::string &c_message, text_align_t c_textAlignment, SDL_Color c_color, uint32_t c_x, uint32_t c_y);
            //static SDL_Texture* TextToTexture(SDL_Renderer *c_renderer, std::string c_font, const std::string &c_message, SDL_Color c_color);
		    //static void RenderTexture(SDL_Texture *c_texture, uint32_t c_x, uint32_t c_y, uint32_t c_w, uint32_t c_h);
		    //static void RenderTexture(SDL_Texture *c_texture, uint32_t c_x, uint32_t c_y);

		    // Fonts
            static void InitFonts();
            static TTF_Font* GetFont(std::string c_fontName);

            // Test Shit
            bool MouseUpEvent(Panel *c_target);

        private:
            // Member Variables
            SDL_Renderer *m_context;
            std::vector<Panel *> m_panels;
            bool m_showFPS;
            uint64_t m_totalFrames;
            bool m_needsLayout;

            // Render State Methods
            void r_mainMenu();
            static void r_gameScreen();
            static void r_fpsMeter();

            // Fonts
            static std::map<std::string, TTF_Font*> m_fontMap;
    };
}

#endif // RENDERER_H
