using System;
using System.Drawing;
using FoxTrader.Game;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    // ReSharper disable once UnusedMember.Global
    class GameView : BaseGameView
    {
        private Vector2 m_mapPosition;
        private Vector2 m_mapOffset;
        private bool m_isDragging;
        private bool m_isMouseWheelClicked = false;
        private int m_zoomLevel = 1;

        private readonly Label m_coordinateLabel;
        private readonly Label m_zoomLabel;

        public GameView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            m_mapPosition = Vector2.Zero;

            m_zoomLabel = new Label(this) { AutoSizeToContents = true, Text = "Zoom:0", X = 10 };
            m_zoomLabel.MakeColorBright();

            m_coordinateLabel = new Label(this) { AutoSizeToContents = true, Text = "X:0, Y:0" };
            m_coordinateLabel.MakeColorBright();

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

        protected override void OnMouseClickedLeft(int c_x, int c_y, bool c_down)
        {
            m_isDragging = c_down;
        }

        protected override void OnMouseMoved(int c_x, int c_y, int c_dx, int c_dy)
        {
            if (m_isDragging)
            {
                m_mapPosition.X += c_dx;
                m_mapPosition.Y += c_dy;

                CalculatePositions();
            }
        }

        protected override bool OnMouseWheeled(int c_delta)
        {
            if (!m_isMouseWheelClicked)
            {
                m_isMouseWheelClicked = true;
                return true;
            }
            else
            {
                m_isMouseWheelClicked = false;
            }

            if (c_delta > 0)
            {
                m_zoomLevel++;
            }
            else
            {
                m_zoomLevel--;
            }

            if (m_zoomLevel > 3)
            {
                m_zoomLevel = 3;
            }

            if (m_zoomLevel < 1)
            {
                m_zoomLevel = 1;
            }

            CalculatePositions();

            return true;
        }

        private void CalculatePositions()
        {
            return;

            if (m_mapPosition.X > 0)
            {
                m_mapPosition.X = 0;
            }

            var a_xOffset = (kGalaxySizeMax * m_zoomLevel) * -1 + (Width - 100);

            if (m_mapPosition.X < a_xOffset)
            {
                m_mapPosition.X = a_xOffset;
            }

            if (m_mapPosition.Y > 0)
            {
                m_mapPosition.Y = 0;
            }

            var a_yOffset = (kGalaxySizeMax * m_zoomLevel) * -1 + (Height - 100);

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

            CalculatePositions();

            m_zoomLabel.Y = m_coordinateLabel.Y = Height - m_coordinateLabel.Height - 5;
            Align.CenterHorizontally(m_coordinateLabel);

            m_mapOffset.X = Width / 2;
            m_mapOffset.Y = Height / 2;
        }

        protected override void Render(SkinBase c_skin)
        {
            base.Render(c_skin);

            c_skin.Renderer.DrawColor = Color.FromArgb(24, 255, 255, 255);

            var a_sectionSize = 16 * m_zoomLevel;
            var a_widthSections = Width / a_sectionSize + 4;
            var a_heightSections = Height / a_sectionSize + 2;
            var a_yGridOffset = Math.Abs(m_mapPosition.Y % a_sectionSize) + 1;
            var a_xGridOffset = Math.Abs(m_mapPosition.X % a_sectionSize) + 1;

            for (var a_y = 0; a_y < a_widthSections; ++a_y)
            {
                var a_yPos = a_y * a_sectionSize - a_xGridOffset;
                var a_rect = Rectangle.FromLTRB(a_yPos, 0, a_yPos + 1, Height);
                c_skin.Renderer.DrawFilledRect(a_rect);
            }

            for (var a_x = 0; a_x < a_heightSections; ++a_x)
            {
                var a_xPos = a_x * a_sectionSize - a_yGridOffset;
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
            }

            m_coordinateLabel.Text = $"X:{m_mapPosition.X}, Y:{m_mapPosition.Y}";
            m_zoomLabel.Text = $"Zoom:{m_zoomLevel}";
        }
    }
}
