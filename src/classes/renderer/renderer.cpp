// renderer.cpp
// 2014-12-24T15:30:00-07:00

#include "renderer.h"

// GUI Headers
#include <panel.h>
#include <label.h>
#include <button.h>
#include <textbox.h>

#include <game.h>
#include <player.h>
#include <galaxy.h>
#include <system.h>

// TEMP
#include <fonts.h>

namespace FoxTrader
{
    // Font Map is static!
    std::map<std::string, TTF_Font*> Renderer::m_fontMap;

    // Let's focus
    Panel* Renderer::m_focusedPanel;

    // ctor
    Renderer::Renderer()
    {
        Renderer::InitFonts();

        Renderer::m_focusedPanel = NULL;

        this->m_context = NULL;
        this->m_showFPS = false;
        this->m_totalFrames = 0;
        this->m_needsLayout = true;
    }

    // Render Tick
    void Renderer::Tick()
    {
        if (this->m_context == NULL)
        {
            Game::TriggerError(Err_Error, std::string("Renderer::Tick() error: m_context was NULL!"));
        }

        // CSelect the color for drawing.
        SDL_SetRenderDrawColor(this->m_context, 0x14, 0x14, 0x87, 0xFF);

        // Clear the entire screen to our selected color.
        SDL_RenderClear(this->m_context);

        for (int i = this->m_panels.size(); i --> 0;)
        {
            this->m_panels[i]->Draw(this->m_context);
        }

        if (this->m_needsLayout)
        {
            switch (Game::GetCurrentState())
            {
                case GameState::MainMenu:
                {
                    this->r_mainMenu();
                }
                break;

                case GameState::GameScreen:
                {
                    Renderer::r_gameScreen();
                }
                break;

                default:
                {

                }
                break;
            }

            this->m_needsLayout = false;
        }


        /*if (Renderer::CurrentPos >= Game::m_universe->m_galaxies.size())
        {
            Renderer::CurrentPos = 0;
        }

        if (Renderer::CurrentPos < 0)
        {
            Renderer::CurrentPos = Game::m_universe->m_galaxies.size() - 1;
        }

        Galaxy a_galaxy = Game::m_universe->m_galaxies[Renderer::CurrentPos];

        Renderer::Text(std::string("Galaxy #") + SSTR((a_galaxy.GetIndex() + 1)) + std::string(" ") + a_galaxy.GetName(), 10, 100);

        std::string a_timeString = "";

        a_timeString.append(kDayOfWeekName[Tools::DayOfTheWeek(Game::m_universe->GetSpaceYear(), Game::m_universe->GetSpaceMonth(), Game::m_universe->GetSpaceDay())]);
        a_timeString.append(" ");
        a_timeString.append();
        a_timeString.append("/");
        a_timeString.append();
        a_timeString.append("/");
        a_timeString.append();
        a_timeString.append(" ");
        a_timeString.append(str( boost::format("%02d") % static_cast<int>(Game::m_universe->GetSpaceHour()) ));
        a_timeString.append(":");
        a_timeString.append(str( boost::format("%02d") % static_cast<int>(Game::m_universe->GetSpaceMinutes()) ));
        a_timeString.append(" UTC");

        Renderer::Text(a_timeString, 0, 380);

        // Show Credits!
        Renderer::TextCentered("Credits: " + Tools::Commafy(Player::GetCredits()), 0);*/

        // Do this last
        if (this->m_showFPS)
        {
            Renderer::r_fpsMeter();
        }

        SDL_RenderPresent(this->m_context);

        this->m_totalFrames++;
    }

    // Context Method
    bool Renderer::SetContext(SDL_Renderer *c_context)
    {
        if (c_context == NULL)
        {
            return false;
        }

        this->m_context = c_context;

        return true;
    }

    // Events
    void Renderer::HandleEvent(SDL_Event *c_event)
    {
        // Let the UI Handle Events First
        // First Right of Cancel
        for (int i = this->m_panels.size(); i --> 0;)
        {
            this->m_panels[i]->HandleEvent(c_event);
        }

        // Low level FPS control
        if (c_event->type == SDL_KEYUP && c_event->key.keysym.sym == SDLK_f)
        {
            Renderer::ToggleFPS();
        }
    }

    // Static Drawing Methods
    /*void Renderer::Text(std::string c_font, const std::string &c_message, text_align_t c_textAlignment, uint32_t c_x, uint32_t c_y)
    {
        SDL_Color a_whiteColor = { 0xFF, 0xFF, 0xFF, 0xFF };

        Renderer::Text(c_font, c_message, c_textAlignment, a_whiteColor, c_x, c_y);
    }

    void Renderer::Text(std::string c_font, const std::string &c_message, text_align_t c_textAlignment, SDL_Color c_color, uint32_t c_x, uint32_t c_y)
    {
        SDL_Texture *a_image = Renderer::TextToTexture(c_font, c_message, c_color);

        if (c_textAlignment == TextAlign::Center)
        {
            int32_t a_width;
            SDL_QueryTexture(a_image, NULL, NULL, &a_width, NULL);
            c_x = (SCREEN_WIDTH / 2) - (a_width / 2);
        }
        else if (c_textAlignment == TextAlign::Right)
        {
            int32_t a_width;
            SDL_QueryTexture(a_image, NULL, NULL, &a_width, NULL);
            c_x = (SCREEN_WIDTH - a_width) - c_x;
        }

        Renderer::RenderTexture(a_image, c_x, c_y);

        SDL_DestroyTexture(a_image);
    }

    SDL_Texture* Renderer::TextToTexture(SDL_Renderer* c_renderer, std::string c_font, const std::string &c_message, SDL_Color c_color)
    {
        if (c_renderer == NULL)
        {
            Game::TriggerError(Err_Error, std::string("TextToTexture error: c_renderer was NULL!"));
            return NULL;
        }

        if (Renderer::m_fontMap.count(c_font) == 0)
        {
            Game::TriggerError(Err_Error, std::string("TextToTexture error: Font name [\"" + c_font + "\"] does not exist!"));
            return NULL;
        }

        SDL_Surface *a_surface = TTF_RenderText_Blended(Renderer::m_fontMap[c_font], c_message.c_str(), c_color);

        if (a_surface == NULL)
        {
            Game::TriggerError(Err_Error, std::string("TextToTexture error: a_surface was NULL!"));

            return NULL;
        }

        SDL_Texture *a_texture = SDL_CreateTextureFromSurface(c_renderer, a_surface);

        if (a_texture == NULL)
        {
            Game::TriggerError(Err_Error, std::string("TextToTexture error: a_texture was NULL!"));

            return NULL;
        }

        SDL_FreeSurface(a_surface);

        return a_texture;
        return null;
    }

    void Renderer::RenderTexture(SDL_Texture *c_texture, uint32_t c_x, uint32_t c_y, uint32_t c_w, uint32_t c_h)
    {
        if (Renderer::m_context == NULL)
        {
            Game::TriggerError(FoxTrader::Err_Error, std::string("RenderTexture error: Renderer::m_context was NULL!"));
        }

        SDL_Rect a_rect;
        a_rect.x = c_x;
        a_rect.y = c_y;
        a_rect.w = c_w;
        a_rect.h = c_h;
        SDL_RenderCopy(Renderer::m_context, c_texture, NULL, &a_rect);
    }

    void Renderer::RenderTexture(SDL_Texture *c_texture, uint32_t c_x, uint32_t c_y)
    {
        if (Renderer::m_context == NULL)
        {
            Game::TriggerError(FoxTrader::Err_Error, std::string("RenderTexture error: Renderer::m_context was NULL!"));
        }

        uint32_t a_width, a_height;
        SDL_QueryTexture(c_texture, NULL, NULL, (int*)&a_width, (int*)&a_height);
        RenderTexture(c_texture, c_x, c_y, a_width, a_height);
    }
    */

    // Render Things
    void Renderer::r_fpsMeter()
    {
/*        float a_fpsReading = Renderer::m_totalFrames / ( SDL_GetTicks() / 1000.f );

        if( a_fpsReading > 2000000 )
        {
            a_fpsReading = 0;
        }

        SDL_Color a_color = { 255, 0, 0, 255 };

        Renderer::Text("bold_24", std::string(SSTR(round(a_fpsReading))), TextAlign::Left, a_color, 3, 0);*/
    }

    // Focus Control
    bool Renderer::RequestFocus(Panel* c_panel)
    {
        if (!c_panel->GetCanFocus())
        {
            return false;
        }

        if (Renderer::m_focusedPanel != NULL)
        {
            Renderer::m_focusedPanel->Blur();
            Renderer::m_focusedPanel = NULL;
        }

        Renderer::m_focusedPanel = c_panel;
        Renderer::m_focusedPanel->Focus();

        return true;
    }

    bool Renderer::MouseUpEvent(Panel *c_target)
    {
        Game::TriggerError(FoxTrader::Err_Information, "HOLY FUCK!");

        return true;
    }

    void Renderer::r_mainMenu()
    {
        Button *a_button = new Button("OK");
        a_button->SetFont("bold_64");
        a_button->SetX(512 - (a_button->GetWidth() / 2));
        a_button->SetY(288 - (a_button->GetHeight() / 2));
        a_button->AddMouseUpDelegate(boost::bind(&Renderer::MouseUpEvent, this, _1));

        this->m_panels.push_back(a_button);

        Label *a_label = new Label("This is a label, a default label...");
        a_label->SetX(100);
        a_label->SetY(100);

        this->m_panels.push_back(a_label);

        TextBox *a_textBox = new TextBox();
        a_textBox->SetX(200),
        a_textBox->SetY(150);

        this->m_panels.push_back(a_textBox);

        /*Renderer::m_renderObjects.clear();

        SDL_Color a_redColor  = { 0xAA, 0x00, 0x00, 0xFF };
        SDL_Rect a_rect = { 0, 0, 100, 100 };



        SDL_Color a_greyColor = { 0xAA, 0xAA, 0xAA, 0xFF };

        Renderer::Text("bold_small_caps_96", "Fox Trader", TextAlign::Center, a_redColor, 0, 28);
        Renderer::Text("regular_24", "A cunning game about foxes and space", TextAlign::Center, 0, 118);

        if ((Tools::GetMilliseconds() & 500) < 250)
        {
            Renderer::Text("regular_24", "Press SPACE BAR to start", TextAlign::Center, 0, 420);
        }

        Renderer::Text("light_16", "Copyright 2015-2017 Fox Council", TextAlign::Center, a_greyColor, 0, 550);*/
    }

    void Renderer::r_gameScreen()
    {
        /*// Header
        SDL_SetRenderDrawColor(Renderer::m_context, 100, 0, 0, 255);
        SDL_Rect a_topRectangle;

        a_topRectangle.x = 0;
        a_topRectangle.y = 0;
        a_topRectangle.w = SCREEN_WIDTH;
        a_topRectangle.h = 26;
        SDL_RenderFillRect(Renderer::m_context, &a_topRectangle);

        // Game::GetUniverse()->m_galaxies[Game::GetPlayer()->GetGalaxyIndex()].GetName()
        Renderer::Text("light_small_caps_24", Game::GetUniverse()->GetName(), TextAlign::Left, 10, 0);
        Renderer::Text("regular_16", Tools::Commafy(Game::GetPlayer()->GetCredits()) + '¢', TextAlign::Right, 10, 5);

        // Body
        SDL_Color a_lightGreenColor = { 0, 255, 0, 255 };
        SDL_Color a_darkGreenColor = { 0, 128, 0, 255 };
        SDL_Color a_lightBlueColor = { 0, 0, 255, 255 };
        Renderer::Text("regular_16", "Current Galaxies:", TextAlign::Left, a_lightGreenColor, 10, 30);

        SDL_PumpEvents();
        int32_t a_mouseX, a_mouseY;
        Uint8 a_mouseButtons = SDL_GetMouseState(&a_mouseX, &a_mouseY);

        uint16_t a_row = 0;
        uint16_t a_col = 0;

        for(std::vector<Galaxy>::size_type i = 0; i != Game::GetUniverse()->m_galaxies.size(); i++)
        {
            SDL_Color a_colorUsed = a_darkGreenColor;

            int32_t a_textX = (a_col * 125) + 10;
            int32_t a_textY = 55 + (a_row++ * 14);

            if (
                (a_mouseX > a_textX && a_mouseX < (a_textX + 125)) &&
                (a_mouseY > a_textY && a_mouseY < (a_textY +  14))
               )
            {
                if (a_mouseButtons & SDL_BUTTON(SDL_BUTTON_LEFT))
                {
                    a_colorUsed = a_lightBlueColor;
                }
                else
                {
                    a_colorUsed = a_lightGreenColor;
                }
            }

            Galaxy a_galaxy = Game::GetUniverse()->m_galaxies.at(i);
            Renderer::Text("regular_12", str( boost::format("%03d") % (i + 1) ) + ": " + a_galaxy.GetName(), TextAlign::Left, a_colorUsed, a_textX, a_textY);

            if (((i + 1) % 35) == 0)
            {
                a_col++;
                a_row = 0;
            }
        }

        // Footer
        SDL_SetRenderDrawColor(Renderer::m_context, 200, 200, 200, 255);
        SDL_Rect a_bottomRectangle;

        a_bottomRectangle.x = 0;
        a_bottomRectangle.y = SCREEN_HEIGHT - 26;
        a_bottomRectangle.w = SCREEN_WIDTH;
        a_bottomRectangle.h = 26;
        SDL_RenderFillRect(Renderer::m_context, &a_bottomRectangle);

        std::string a_timeString = "";

        a_timeString.append(kDayOfWeekNameUpper[Tools::DayOfTheWeek(Game::GetUniverse()->GetSpaceYear(), Game::GetUniverse()->GetSpaceMonth(), Game::GetUniverse()->GetSpaceDay())]);
        a_timeString.append(" ");
        a_timeString.append(SSTR(static_cast<int>(Game::GetUniverse()->GetSpaceDay())));
        a_timeString.append("/");
        a_timeString.append(SSTR(static_cast<int>(Game::GetUniverse()->GetSpaceMonth())));
        a_timeString.append("/");
        a_timeString.append(SSTR(Game::GetUniverse()->GetSpaceYear()));
        a_timeString.append(" ");
        a_timeString.append(str( boost::format("%02d") % static_cast<int>(Game::GetUniverse()->GetSpaceHour()) ));
        a_timeString.append((Tools::GetMilliseconds() & 1000) < 500 ? ":" : " ");
        a_timeString.append(str( boost::format("%02d") % static_cast<int>(Game::GetUniverse()->GetSpaceMinutes()) ));
        a_timeString.append(" UTC");

        SDL_Color a_blackColor = { 0, 0, 0, 255 };
        Renderer::Text("bold_24", "PILOT: " + Game::GetPlayer()->GetName(), TextAlign::Left, a_blackColor, 10, SCREEN_HEIGHT - 26);
        SDL_Color a_redColor = { 128, 0, 0, 255 };
        Renderer::Text("bold_24", a_timeString, TextAlign::Right, a_redColor, 10, SCREEN_HEIGHT - 26);
        */
    }

    // Initialize Fonts
    void Renderer::InitFonts()
    {
        if (Renderer::m_fontMap.size() == 0)
        {
            // Load Font from Memory
            Renderer::m_fontMap["regular_12"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_regular_ttf, Fonts::font_regular_ttf_len), 1, 12);
            Renderer::m_fontMap["regular_16"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_regular_ttf, Fonts::font_regular_ttf_len), 1, 16);
            Renderer::m_fontMap["regular_24"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_regular_ttf, Fonts::font_regular_ttf_len), 1, 24);
            Renderer::m_fontMap["regular_32"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_regular_ttf, Fonts::font_regular_ttf_len), 1, 32);
            Renderer::m_fontMap["regular_64"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_regular_ttf, Fonts::font_regular_ttf_len), 1, 64);
            Renderer::m_fontMap["regular_96"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_regular_ttf, Fonts::font_regular_ttf_len), 1, 96);

            Renderer::m_fontMap["light_12"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_ttf, Fonts::font_light_ttf_len), 1, 12);
            Renderer::m_fontMap["light_16"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_ttf, Fonts::font_light_ttf_len), 1, 16);
            Renderer::m_fontMap["light_24"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_ttf, Fonts::font_light_ttf_len), 1, 24);
            Renderer::m_fontMap["light_32"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_ttf, Fonts::font_light_ttf_len), 1, 32);
            Renderer::m_fontMap["light_64"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_ttf, Fonts::font_light_ttf_len), 1, 64);
            Renderer::m_fontMap["light_96"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_ttf, Fonts::font_light_ttf_len), 1, 96);

            Renderer::m_fontMap["bold_12"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_ttf, Fonts::font_bold_ttf_len), 1, 12);
            Renderer::m_fontMap["bold_16"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_ttf, Fonts::font_bold_ttf_len), 1, 16);
            Renderer::m_fontMap["bold_24"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_ttf, Fonts::font_bold_ttf_len), 1, 24);
            Renderer::m_fontMap["bold_32"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_ttf, Fonts::font_bold_ttf_len), 1, 32);
            Renderer::m_fontMap["bold_64"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_ttf, Fonts::font_bold_ttf_len), 1, 64);
            Renderer::m_fontMap["bold_96"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_ttf, Fonts::font_bold_ttf_len), 1, 96);

            Renderer::m_fontMap["italic_12"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_oblique_ttf, Fonts::font_oblique_ttf_len), 1, 12);
            Renderer::m_fontMap["italic_16"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_oblique_ttf, Fonts::font_oblique_ttf_len), 1, 16);
            Renderer::m_fontMap["italic_24"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_oblique_ttf, Fonts::font_oblique_ttf_len), 1, 24);
            Renderer::m_fontMap["italic_32"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_oblique_ttf, Fonts::font_oblique_ttf_len), 1, 32);
            Renderer::m_fontMap["italic_64"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_oblique_ttf, Fonts::font_oblique_ttf_len), 1, 64);
            Renderer::m_fontMap["italic_96"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_oblique_ttf, Fonts::font_oblique_ttf_len), 1, 96);

            Renderer::m_fontMap["light_italic_12"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_oblique_ttf, Fonts::font_light_oblique_ttf_len), 1, 12);
            Renderer::m_fontMap["light_italic_16"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_oblique_ttf, Fonts::font_light_oblique_ttf_len), 1, 16);
            Renderer::m_fontMap["light_italic_24"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_oblique_ttf, Fonts::font_light_oblique_ttf_len), 1, 24);
            Renderer::m_fontMap["light_italic_32"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_oblique_ttf, Fonts::font_light_oblique_ttf_len), 1, 32);
            Renderer::m_fontMap["light_italic_64"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_oblique_ttf, Fonts::font_light_oblique_ttf_len), 1, 64);
            Renderer::m_fontMap["light_italic_96"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_light_oblique_ttf, Fonts::font_light_oblique_ttf_len), 1, 96);

            Renderer::m_fontMap["bold_italic_12"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_oblique_ttf, Fonts::font_bold_oblique_ttf_len), 1, 12);
            Renderer::m_fontMap["bold_italic_16"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_oblique_ttf, Fonts::font_bold_oblique_ttf_len), 1, 16);
            Renderer::m_fontMap["bold_italic_24"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_oblique_ttf, Fonts::font_bold_oblique_ttf_len), 1, 24);
            Renderer::m_fontMap["bold_italic_32"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_oblique_ttf, Fonts::font_bold_oblique_ttf_len), 1, 32);
            Renderer::m_fontMap["bold_italic_64"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_oblique_ttf, Fonts::font_bold_oblique_ttf_len), 1, 64);
            Renderer::m_fontMap["bold_italic_96"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_bold_oblique_ttf, Fonts::font_bold_oblique_ttf_len), 1, 96);

            Renderer::m_fontMap["small_caps_12"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_ttf, Fonts::font_small_caps_ttf_len), 1, 12);
            Renderer::m_fontMap["small_caps_16"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_ttf, Fonts::font_small_caps_ttf_len), 1, 16);
            Renderer::m_fontMap["small_caps_24"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_ttf, Fonts::font_small_caps_ttf_len), 1, 24);
            Renderer::m_fontMap["small_caps_32"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_ttf, Fonts::font_small_caps_ttf_len), 1, 32);
            Renderer::m_fontMap["small_caps_64"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_ttf, Fonts::font_small_caps_ttf_len), 1, 64);
            Renderer::m_fontMap["small_caps_96"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_ttf, Fonts::font_small_caps_ttf_len), 1, 96);

            Renderer::m_fontMap["light_small_caps_12"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_light_ttf, Fonts::font_small_caps_light_ttf_len), 1, 12);
            Renderer::m_fontMap["light_small_caps_16"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_light_ttf, Fonts::font_small_caps_light_ttf_len), 1, 16);
            Renderer::m_fontMap["light_small_caps_24"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_light_ttf, Fonts::font_small_caps_light_ttf_len), 1, 24);
            Renderer::m_fontMap["light_small_caps_32"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_light_ttf, Fonts::font_small_caps_light_ttf_len), 1, 32);
            Renderer::m_fontMap["light_small_caps_64"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_light_ttf, Fonts::font_small_caps_light_ttf_len), 1, 64);
            Renderer::m_fontMap["light_small_caps_96"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_light_ttf, Fonts::font_small_caps_light_ttf_len), 1, 96);

            Renderer::m_fontMap["bold_small_caps_12"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_bold_ttf, Fonts::font_small_caps_bold_ttf_len), 1, 12);
            Renderer::m_fontMap["bold_small_caps_16"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_bold_ttf, Fonts::font_small_caps_bold_ttf_len), 1, 16);
            Renderer::m_fontMap["bold_small_caps_24"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_bold_ttf, Fonts::font_small_caps_bold_ttf_len), 1, 24);
            Renderer::m_fontMap["bold_small_caps_32"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_bold_ttf, Fonts::font_small_caps_bold_ttf_len), 1, 32);
            Renderer::m_fontMap["bold_small_caps_64"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_bold_ttf, Fonts::font_small_caps_bold_ttf_len), 1, 64);
            Renderer::m_fontMap["bold_small_caps_96"] = TTF_OpenFontRW(SDL_RWFromMem(Fonts::font_small_caps_bold_ttf, Fonts::font_small_caps_bold_ttf_len), 1, 96);
        }
    }

    TTF_Font* Renderer::GetFont(std::string c_fontName)
    {
        if (Renderer::m_fontMap.count(c_fontName) == 0)
        {
            Game::TriggerError(LoggingType::Err_Error, std::string("Renderer::GetFont error: Font name [\"" + c_fontName + "\"] does not exist!"));
            return NULL;
        }

        return Renderer::m_fontMap[c_fontName];
    }

    // Display Controls
    void Renderer::ToggleFPS()
    {
        this->m_showFPS = !this->m_showFPS;
    }
}

