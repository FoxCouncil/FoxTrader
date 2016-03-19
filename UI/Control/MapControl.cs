using System;
using System.Collections.Generic;
using System.Drawing;
using FoxTrader.Game;
using FoxTrader.UI.Skin;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader.UI.Control
{
    sealed class MapControl : GameControl
    {
        private Vector2 m_mapPosition;
        private Vector2 m_mapOffset;
        private bool m_isMouseWheelClicked;
        private bool m_zoomChanged;
        private int m_zoomLevel = 1;
        private string m_zoomLabelString = I18N.GetString("ZoomLabel");

        private readonly Label m_mousePositionLabel;
        private readonly Label m_coordinateLabel;
        private readonly Label m_zoomLabel;

        private readonly Dictionary<int, Label> m_labelDictionary;

        public MapZoomState ZoomState
        {
            get;
        } = MapZoomState.Universe;

        public MapControl(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            m_mapPosition = Vector2.Zero;

            m_zoomLabel = new Label(this) { AutoSizeToContents = true, Text = "Zoom:0", X = 10 };
            m_zoomLabel.MakeColorBright();

            m_coordinateLabel = new Label(this) { AutoSizeToContents = true, Text = "X:0, Y:0" };
            m_coordinateLabel.MakeColorBright();

            m_mousePositionLabel = new Label(this) { AutoSizeToContents = true, Text = "X:0, Y:0" };
            m_mousePositionLabel.MakeColorBright();

            m_labelDictionary = new Dictionary<int, Label>();

            foreach (var a_gameGalaxy in GameContext.Instance.Universe.Galaxies)
            {
                var a_galaxyLabel = new Label(this) { AutoSizeToContents = true, Text = a_gameGalaxy.Name, X = a_gameGalaxy.Position.X, Y = a_gameGalaxy.Position.Y, Font = FoxTraderWindow.Instance.Renderer.GetFont("Roboto Condensed", 8) };
                a_galaxyLabel.MakeColorBright();

                m_labelDictionary.Add(a_gameGalaxy.Index, a_galaxyLabel);
            }

            KeyboardInputEnabled = true;
            Focus();
        }

        protected override bool OnKeyPressed(Key c_keys, bool c_isButtonDown = true)
        {
            if (c_keys != Key.C)
            {
                return false;
            }

            CentreMap();

            return true;
        }

        protected override void OnMouseMoved(MouseState c_mouseState, int c_x, int c_y, int c_dx, int c_dy)
        {
            if (c_mouseState.LeftButton != ButtonState.Pressed)
            {
                return;
            }

            m_mapPosition.X += c_dx;
            m_mapPosition.Y += c_dy;

            CalculatePositions();
        }

        protected override bool OnMouseWheeled(int c_delta)
        {
            if (!m_isMouseWheelClicked)
            {
                m_isMouseWheelClicked = true;
                return true;
            }

            m_isMouseWheelClicked = false;

            var a_mapPosX = m_mapPosition.X - m_mapOffset.X;
            var a_mapPosY = m_mapPosition.Y - m_mapOffset.Y;

            if (c_delta > 0)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (m_zoomLevel)
                {
                    case 1:
                        m_zoomLevel = 2;
                        m_zoomChanged = true;
                        break;
                    case 2:
                        m_zoomLevel = 4;
                        m_zoomChanged = true;
                        break;
                }

                if (m_zoomChanged)
                {
                    m_mapPosition.X = a_mapPosX * 2 + m_mapOffset.X;
                    m_mapPosition.Y = a_mapPosY * 2 + m_mapOffset.Y;

                    m_zoomChanged = false;
                }
            }
            else
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (m_zoomLevel)
                {
                    case 4:
                        m_zoomLevel = 2;
                        m_zoomChanged = true;
                        break;
                    case 2:
                        m_zoomLevel = 1;
                        m_zoomChanged = true;
                        break;
                }

                if (m_zoomChanged)
                {
                    m_mapPosition.X = a_mapPosX / 2 + m_mapOffset.X;
                    m_mapPosition.Y = a_mapPosY / 2 + m_mapOffset.Y;

                    m_zoomChanged = false;
                }
            }

            CalculatePositions();

            return true;
        }

        private void CalculatePositions()
        {
            var a_mapPosX = m_mapPosition.X - m_mapOffset.X;
            var a_mapPosY = m_mapPosition.Y - m_mapOffset.Y;

            if (a_mapPosX > 0)
            {
                m_mapPosition.X = 0 + m_mapOffset.X;
            }

            var a_xOffset = (kGalaxySizeMax * m_zoomLevel - m_mapOffset.X) * -1;

            if (m_mapPosition.X < a_xOffset)
            {
                m_mapPosition.X = a_xOffset;
            }

            if (a_mapPosY > 0)
            {
                m_mapPosition.Y = 0 + m_mapOffset.Y;
            }

            var a_yOffset = (kGalaxySizeMax * m_zoomLevel - m_mapOffset.Y) * -1;

            if (m_mapPosition.Y < a_yOffset)
            {
                m_mapPosition.Y = a_yOffset;
            }
        }

        private void CentreMap()
        {
            m_mapPosition.X = (kGalaxySizeMax / 2) - Width / 2 * -1;
            m_mapPosition.Y = (kGalaxySizeMax / 2) - Height / 2 * -1;
        }

        protected override void Layout(SkinBase c_skin)
        {
            base.Layout(c_skin);

            if (!I18N.GetString("ZoomLabel").Equals(m_zoomLabelString))
            {
                m_zoomLabelString = I18N.GetString("ZoomLabel");
            }

            CalculatePositions();

            m_zoomLabel.Y = m_coordinateLabel.Y = m_mousePositionLabel.Y = Height - m_coordinateLabel.Height - 5;
            Align.CenterHorizontally(m_coordinateLabel);
            m_mousePositionLabel.X = Width - m_mousePositionLabel.Width - 5;

            m_mapOffset.X = Width / 2;
            m_mapOffset.Y = Height / 2;
        }

        protected override void Render(SkinBase c_skin)
        {
            base.Render(c_skin);

            c_skin.Renderer.DrawColor = Color.FromArgb(24, 128, 0, 0);

            c_skin.Renderer.DrawFilledRect(Rectangle.FromLTRB(m_mapPosition.X, m_mapPosition.Y, m_mapPosition.X + (kGalaxySizeMax * m_zoomLevel), m_mapPosition.Y + (kGalaxySizeMax * m_zoomLevel)));

            c_skin.Renderer.DrawColor = Color.FromArgb(24, 255, 255, 255);

            var a_mapPosX = m_mapPosition.X - m_mapOffset.X;
            var a_mapPosY = m_mapPosition.Y - m_mapOffset.Y;

            var a_sectionSize = 16 * m_zoomLevel;
            var a_widthSections = Width / a_sectionSize + 4;
            var a_heightSections = Height / a_sectionSize + 2;
            var a_xGridOffset = Math.Abs(m_mapPosition.X) % a_sectionSize + 1;
            var a_yGridOffset = Math.Abs(m_mapPosition.Y) % a_sectionSize + 1;

            for (var a_y = 0; a_y < a_widthSections; ++a_y)
            {
                int a_yPos;

                if (m_mapPosition.X > 0)
                {
                    a_yPos = a_y * a_sectionSize + a_xGridOffset;
                }
                else
                {
                    a_yPos = a_y * a_sectionSize - a_xGridOffset;
                }

                var a_rect = Rectangle.FromLTRB(a_yPos, 0, a_yPos + 1, Height);
                c_skin.Renderer.DrawFilledRect(a_rect);
            }

            for (var a_x = 0; a_x < a_heightSections; ++a_x)
            {
                int a_xPos;

                if (m_mapPosition.Y > 0)
                {
                    a_xPos = a_x * a_sectionSize + a_yGridOffset;
                }
                else
                {
                    a_xPos = a_x * a_sectionSize - a_yGridOffset;
                }

                var a_rect = Rectangle.FromLTRB(0, a_xPos, Width, a_xPos + 1);
                c_skin.Renderer.DrawFilledRect(a_rect);
            }

            c_skin.Renderer.DrawColor = Color.White;

            foreach (var a_gameGalaxy in GameContext.Instance.Universe.Galaxies)
            {
                var a_pos = a_gameGalaxy.Position.ToVec2();

                var a_offsetX = a_pos.X * m_zoomLevel + m_mapPosition.X;
                var a_offsetY = a_pos.Y * m_zoomLevel + m_mapPosition.Y;
                var a_dotSize = 2 * m_zoomLevel;

                c_skin.Renderer.DrawFilledRect(Rectangle.FromLTRB(a_offsetX, a_offsetY, a_offsetX + a_dotSize, a_offsetY + a_dotSize));

                m_labelDictionary[a_gameGalaxy.Index].SetPosition(a_offsetX, a_offsetY - m_labelDictionary[a_gameGalaxy.Index].Height - 1);
                m_labelDictionary[a_gameGalaxy.Index].Font = FoxTraderWindow.Instance.Renderer.GetFont("Roboto Condensed", 8 * m_zoomLevel);
            }

            m_coordinateLabel.Text = $"X:{Math.Abs(a_mapPosX)}, Y:{Math.Abs(a_mapPosY)}";
            m_mousePositionLabel.Text = $"X:{m_mapOffset.X}, Y:{m_mapOffset.Y}";
            m_zoomLabel.Text = $"{m_zoomLabelString}:{m_zoomLevel}x";
        }
    }
}