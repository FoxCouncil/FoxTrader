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
        private int m_galaxySelection = -1;
        private int m_systemSelection = -1;
        private string m_zoomLabelString = I18N.GetString("ZoomLabel");

        private readonly Label m_zoomLabel;
        private readonly Label m_breadcrumLabel;
        private readonly Label m_coordinateLabel;
        private readonly Button m_backButton;

        private Dictionary<int, MapControlButton> m_buttonDictionary;

        public MapZoomState ZoomState
        {
            get;
            private set;
        } = MapZoomState.Universe;

        public MapControl(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            m_mapPosition = Vector2.Zero;

            m_zoomLabel = new Label(this) { AutoSizeToContents = true, Text = $"{m_zoomLabelString}:0", X = 10, UserData = "MapUI" };
            m_zoomLabel.TextColor = Color.White;

            m_breadcrumLabel = new Label(this) { AutoSizeToContents = true, Text = "Universe", UserData = "MapUI" };
            m_breadcrumLabel.TextColor = Color.White;

            m_coordinateLabel = new Label(this) { AutoSizeToContents = true, Text = "X:0, Y:0", UserData = "MapUI" };
            m_coordinateLabel.TextColor = Color.White;

            m_backButton = new Button(this) { AutoSizeToContents = true, Text = "Back", X = 10, Y = 10, UserData = "MapUI" };
            m_backButton.Clicked += (c_control, c_arg) =>
            {
                if (m_systemSelection != -1)
                {
                    ZoomState = MapZoomState.Galaxy;
                    m_systemSelection = -1;
                    SetupMap();
                }
                else if (m_galaxySelection != -1)
                {
                    ZoomState = MapZoomState.Universe;
                    m_galaxySelection = -1;
                    SetupMap();
                }
            };
            m_backButton.Hide();

            SetupMap();

            KeyboardInputEnabled = true;
            OnFocus();
        }

        private void SetupMap()
        {
            m_mapPosition = Vector2.Zero;

            switch (ZoomState)
            {
                case MapZoomState.Universe:
                m_backButton.Hide();
                SetupUniverseMap();
                break;
                case MapZoomState.Galaxy:
                m_backButton.Show();
                SetupGalaxyMap();
                break;
                case MapZoomState.System:
                m_backButton.Show();
                SetupSystemMap();
                break;
                default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private void RefreshDictionaries()
        {
            if (m_buttonDictionary != null)
            {
                foreach (var a_childControl in m_buttonDictionary.Values)
                {
                    RemoveChild(a_childControl, true);
                }
            }

            m_buttonDictionary?.Clear();

            m_buttonDictionary = new Dictionary<int, MapControlButton>();
        }

        private void SetupUniverseMap()
        {
            RefreshDictionaries();

            foreach (var a_gameGalaxy in GameContext.Instance.Universe.Galaxies)
            {
                var a_galaxyButton = new MapControlButton(this, a_gameGalaxy);
                /*a_galaxyButton.Clicked += c_sender =>
                {
                    m_galaxySelection = ((MapControlButton)c_sender).MapObject.Index;
                    ZoomState = MapZoomState.Galaxy;
                    SetupMap();
                };*/

                m_buttonDictionary.Add(a_gameGalaxy.Index, a_galaxyButton);
            }
        }

        private void SetupGalaxyMap()
        {
            RefreshDictionaries();

            var a_gameGalaxy = GameContext.Instance.Universe.Galaxies[m_galaxySelection];

            foreach (var a_gameSystem in a_gameGalaxy.Systems)
            {
                var a_systemButton = new MapControlButton(this, a_gameSystem);
                /*a_systemButton.Clicked += c_sender =>
                {
                    m_systemSelection = ((MapControlButton)c_sender).MapObject.Index;
                    ZoomState = MapZoomState.System;
                    SetupMap();
                };*/

                m_buttonDictionary.Add(a_gameSystem.Index, a_systemButton);
            }
        }

        private void SetupSystemMap()
        {
            RefreshDictionaries();

            var a_gameSystem = GameContext.Instance.Universe.Galaxies[m_galaxySelection].Systems[m_systemSelection];

            foreach (var a_gamePlanetoid in a_gameSystem.Planetoids)
            {
                var a_systemButton = new MapControlButton(this, a_gamePlanetoid);
                /*a_systemButton.Clicked += c_sender =>
                {
                    // TODO: ??
                };*/

                m_buttonDictionary.Add(a_gamePlanetoid.Index, a_systemButton);
            }
        }

        public override void OnMouseMoved(MouseMoveEventArgs c_mouseEventArgs)
        {
            if (!c_mouseEventArgs.Mouse.IsButtonDown(MouseButton.Left))
            {
                return;
            }

            m_mapPosition.X += c_mouseEventArgs.XDelta;
            m_mapPosition.Y += c_mouseEventArgs.YDelta;

            CalculatePositions();
        }

        public override void OnMouseWheel(MouseWheelEventArgs c_mouseWheelEventArgs)
        {
            if (!m_isMouseWheelClicked)
            {
                m_isMouseWheelClicked = true;
                return;
            }

            m_isMouseWheelClicked = false;

            var a_mapPosX = m_mapPosition.X - m_mapOffset.X;
            var a_mapPosY = m_mapPosition.Y - m_mapOffset.Y;

            if (c_mouseWheelEventArgs.Delta > 0)
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
        }

        private void CalculatePositions()
        {
            var a_mapMaxSize = GetMapMaxSize();

            var a_mapPosX = m_mapPosition.X - m_mapOffset.X;
            var a_mapPosY = m_mapPosition.Y - m_mapOffset.Y;

            if (a_mapPosX > 0)
            {
                m_mapPosition.X = 0 + m_mapOffset.X;
            }

            var a_xOffset = (a_mapMaxSize * m_zoomLevel - m_mapOffset.X) * -1;

            if (m_mapPosition.X < a_xOffset)
            {
                m_mapPosition.X = a_xOffset;
            }

            if (a_mapPosY > 0)
            {
                m_mapPosition.Y = 0 + m_mapOffset.Y;
            }

            var a_yOffset = (a_mapMaxSize * m_zoomLevel - m_mapOffset.Y) * -1;

            if (m_mapPosition.Y < a_yOffset)
            {
                m_mapPosition.Y = a_yOffset;
            }
        }

        private int GetMapMaxSize()
        {
            int a_mapMaxSize;

            switch (ZoomState)
            {
                case MapZoomState.Universe:
                a_mapMaxSize = kUniverseSizeMax;
                break;
                case MapZoomState.Galaxy:
                a_mapMaxSize = kGalaxySizeMax;
                break;
                case MapZoomState.System:
                a_mapMaxSize = kSystemSizeMax;
                break;
                default:
                throw new ArgumentOutOfRangeException();
            }

            return a_mapMaxSize;
        }

        private void CentreMap()
        {
            var a_mapMaxSize = GetMapMaxSize();

            m_mapPosition.X = (a_mapMaxSize / 2) - Width / 2 * -1;
            m_mapPosition.Y = (a_mapMaxSize / 2) - Height / 2 * -1;
        }

        protected override void OnLayout(SkinBase c_skin)
        {
            base.OnLayout(c_skin);

            if (!I18N.GetString("ZoomLabel").Equals(m_zoomLabelString))
            {
                m_zoomLabelString = I18N.GetString("ZoomLabel");
            }

            CalculatePositions();

            m_zoomLabel.Y = m_breadcrumLabel.Y = m_coordinateLabel.Y = Height - m_coordinateLabel.Height - 5;
            Align.CenterHorizontally(m_breadcrumLabel);
            m_coordinateLabel.X = Width - m_coordinateLabel.Width - 5;

            m_mapOffset.X = Width / 2;
            m_mapOffset.Y = Height / 2;
        }

        protected override void Render(SkinBase c_skin)
        {
            base.Render(c_skin);

            RenderMapBackground(c_skin);

            RenderGrid(c_skin);

            switch (ZoomState)
            {
                case MapZoomState.Universe:
                RenderUniverse();
                break;
                case MapZoomState.Galaxy:
                RenderGalaxy();
                break;
                case MapZoomState.System:
                RenderSystem();
                break;
                default:
                throw new ArgumentOutOfRangeException();
            }

            RenderLabelText();
        }

        private void RenderUniverse()
        {
            foreach (var a_gameGalaxy in GameContext.Instance.Universe.Galaxies)
            {
                var a_pos = a_gameGalaxy.Position;

                var a_offsetX = a_pos.X * m_zoomLevel + m_mapPosition.X;
                var a_offsetY = a_pos.Y * m_zoomLevel + m_mapPosition.Y;

                m_buttonDictionary[a_gameGalaxy.Index].SetPosition(a_offsetX, a_offsetY - m_buttonDictionary[a_gameGalaxy.Index].Height - 1);
            }
        }

        private void RenderGalaxy()
        {
            var a_galaxy = GameContext.Instance.Universe.Galaxies[m_galaxySelection];

            foreach (var a_gameSystem in a_galaxy.Systems)
            {
                var a_pos = a_gameSystem.Position;

                var a_offsetX = a_pos.X * m_zoomLevel + m_mapPosition.X;
                var a_offsetY = a_pos.Y * m_zoomLevel + m_mapPosition.Y;

                m_buttonDictionary[a_gameSystem.Index].SetPosition(a_offsetX, a_offsetY - m_buttonDictionary[a_gameSystem.Index].Height - 1);
            }
        }

        private void RenderSystem()
        {
            var a_system = GameContext.Instance.Universe.Galaxies[m_galaxySelection].Systems[m_systemSelection];

            foreach (var a_planetoid in a_system.Planetoids)
            {
                var a_pos = a_planetoid.Position;

                var a_offsetX = a_pos.X * m_zoomLevel + m_mapPosition.X;
                var a_offsetY = a_pos.Y * m_zoomLevel + m_mapPosition.Y;

                m_buttonDictionary[a_planetoid.Index].SetPosition(a_offsetX, a_offsetY - m_buttonDictionary[a_planetoid.Index].Height - 1);
            }
        }

        private void RenderGrid(SkinBase c_skin)
        {
            c_skin.Renderer.DrawColor = Color.FromArgb(24, 255, 255, 255);

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
        }

        private void RenderMapBackground(SkinBase c_skin)
        {
            const int intensityValue = 128;

            int a_mapSize;

            switch (ZoomState)
            {
                case MapZoomState.Universe:
                a_mapSize = kUniverseSizeMax;
                break;
                case MapZoomState.Galaxy:
                a_mapSize = kGalaxySizeMax;
                break;
                case MapZoomState.System:
                a_mapSize = kSystemSizeMax;
                break;
                default:
                throw new ArgumentOutOfRangeException();
            }

            c_skin.Renderer.DrawColor = Color.FromArgb(24, ZoomState == MapZoomState.Universe ? intensityValue : 0, ZoomState == MapZoomState.System ? intensityValue : 0, ZoomState == MapZoomState.Galaxy ? intensityValue : 0);

            c_skin.Renderer.DrawFilledRect(Rectangle.FromLTRB(m_mapPosition.X, m_mapPosition.Y, m_mapPosition.X + (a_mapSize * m_zoomLevel), m_mapPosition.Y + (a_mapSize * m_zoomLevel)));
        }

        private void RenderLabelText()
        {
            m_coordinateLabel.Text = $"X:{Math.Abs(m_mapPosition.X - m_mapOffset.X)}, Y:{Math.Abs(m_mapPosition.Y - m_mapOffset.Y)}";

            m_breadcrumLabel.Text = $"{GameContext.Instance.Player.Name}'s Universe";

            if (m_galaxySelection != -1)
            {
                m_breadcrumLabel.Text += $" > {GameContext.Instance.Universe.Galaxies[m_galaxySelection].Name}";
            }

            if (m_systemSelection != -1)
            {
                m_breadcrumLabel.Text += $" > {GameContext.Instance.Universe.Galaxies[m_galaxySelection].Systems[m_systemSelection].Name}";
            }

            m_zoomLabel.Text = $"{m_zoomLabelString}:{m_zoomLevel}x";
        }
    }
}