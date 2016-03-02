﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using FoxTrader.Game;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using FoxTrader.UI.DragDrop;
using FoxTrader.UI.Skin;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using static FoxTrader.Constants;

namespace FoxTrader
{
    internal class FoxTraderWindow : GameWindow
    {
        private UI.Renderer.OpenTK m_renderer;
        private SkinBase m_skin;
        private Point m_mousePosition;
        private static readonly UI.Input.KeyData m_keyData = new UI.Input.KeyData();
        private static readonly float[] m_lastClickedTime = new float[kMaxMouseButtons];
        private static Point m_lastClickedPosition;
        private static FoxTraderWindow m_gameWindowInstance;

        private StatusBar m_statusBar;

        const int kFpsFrames = 50;
        private readonly List<long> m_frameTime;
        private readonly Stopwatch m_stopwatch;
        private long m_lastFrameTime;
        private bool m_altDown = false;
        private double m_fps;

        private const string kViewsFormatString = "FoxTrader.Views.{0}";
        private Canvas m_canvas;

        private GameView m_currentView;

        private static readonly object m_windowContextLock = new object();
        private PrivateFontCollection m_privateFontCollection;
        private Dictionary<string, GameFont> m_fontStore;

        internal static FoxTraderWindow Instance
        {
            get
            {
                lock (m_windowContextLock)
                {
                    return m_gameWindowInstance;
                }
            }
        }

        public FoxTraderWindow() : base(kDefaultWinWidth, kDefaultWinHeight)
        {
            Location = new Point(42, 42);

            HookInputEvents();

            m_frameTime = new List<long>(kFpsFrames);
            m_stopwatch = new Stopwatch();

            m_gameWindowInstance = this;
        }

        public SkinBase Skin => m_skin;

        public GameControl MouseFocus
        {
            get;
            internal set;
        }
        public GameControl HoveredControl
        {
            get;
            internal set;
        }
        public GameControl KeyboardFocus
        {
            get;
            internal set;
        }
        public bool IsControlDown
        {
            get;
            internal set;
        }
        public bool IsShiftDown
        {
            get;
            internal set;
        }
        public Point MousePosition
        {
            get
            {
                return m_mousePosition;
            }

            internal set
            {
                m_mousePosition = value;
            }
        }

        protected override void OnLoad(EventArgs c_e)
        {
            Time.Initialize();

            GL.ClearColor(Color.Black);

            m_renderer = new UI.Renderer.OpenTK();

            InitFonts();

            m_skin = new TexturedSkin(m_renderer, "png_FoxTraderSkin");
            m_skin.DefaultFont = m_fontStore[kDefaultGameFontName];

            m_canvas = new Canvas();

            m_canvas.SetSize(Width, Height);
            m_canvas.ShouldDrawBackground = true;
            m_canvas.BackgroundColor = Color.FromArgb(255, 0, 0, 12);

            m_stopwatch.Restart();
            m_lastFrameTime = 0;

            FoxTraderGame.GameContextInstance.StateChanged += GameContextInstance_StateChanged;
            GameContextInstance_StateChanged(ContextState.Bumpers);
        }

        private void GameContextInstance_StateChanged(ContextState c_newState)
        {
            switch (c_newState)
            {
                case ContextState.Bumpers:
                {
                    ShowView("BumpersView");
                }
                break;

                case ContextState.MainMenu:
                {
                    ShowView("MainMenuView");
                }
                break;

                case ContextState.Options:
                {
                    ShowView("OptionsView");
                }
                break;
            }
        }

        protected override void OnResize(EventArgs c_e)
        {
            // Clamp
            if (Width < kDefaultWinWidth)
            {
                Width = kDefaultWinWidth;
            }

            if (Height < kDefaultWinHeight)
            {
                Height = kDefaultWinHeight;
            }

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Width, Height, 0, -1, 1);

            m_canvas.SetSize(Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs c_e)
        {
            if (m_frameTime.Count == kFpsFrames)
            {
                m_frameTime.RemoveAt(0);
            }

            m_frameTime.Add(m_stopwatch.ElapsedMilliseconds - m_lastFrameTime);
            m_lastFrameTime = m_stopwatch.ElapsedMilliseconds;

            if (m_stopwatch.ElapsedMilliseconds > 1000)
            {
                m_fps = 1000f * m_frameTime.Count / m_frameTime.Sum();

                m_stopwatch.Restart();

                FlushTextCache();
            }

            Time.Tick();

            FoxTraderGame.GameContextInstance.Tick();

            m_canvas.UpdateCanvas();
        }

        protected override void OnRenderFrame(FrameEventArgs c_e)
        {
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            m_canvas.RenderCanvas();

            SwapBuffers();

            if (m_statusBar != null)
            {
                m_statusBar.Text = $"{m_fps:F0}fps - {m_renderer.DrawCallCount}dc - {m_renderer.VertexCount}vc";
            }
        }

        private void InitFonts()
        {
            if (m_renderer == null)
            {
                throw new InvalidOperationException("Tried to initialize fonts before the renderer was even created!");
            }

            // Load Internal Fonts
            m_privateFontCollection = new PrivateFontCollection();

            var a_chicagoFont = Properties.Resources.ttf_chicago;

            var a_rawFontData = Marshal.AllocCoTaskMem(a_chicagoFont.Length);

            Marshal.Copy(a_chicagoFont, 0, a_rawFontData, a_chicagoFont.Length);

            m_privateFontCollection.AddMemoryFont(a_rawFontData, a_chicagoFont.Length);

            Marshal.FreeCoTaskMem(a_rawFontData);

            m_fontStore = new Dictionary<string, GameFont>();

            foreach (var a_font in kDefaultGameFonts)
            {
                var a_fontData = a_font.Split(',');
                m_fontStore.Add(a_font, new GameFont(m_renderer, a_fontData[0], int.Parse(a_fontData[1])));
            }
        }

        public GameFont GetFont(string c_fontName, int c_fontSize)
        {
            if (string.IsNullOrWhiteSpace(c_fontName) || c_fontSize <= 0)
            {
                return m_fontStore[kDefaultGameFontName];
            }

            var a_fontKey = $"{c_fontName},{c_fontSize}";

            if (m_fontStore.ContainsKey(a_fontKey))
            {
                return m_fontStore[a_fontKey];
            }

            return m_fontStore[kDefaultGameFontName];
        }

        public bool ShowView(string c_viewString)
        {
            var a_viewType = Type.GetType(string.Format(kViewsFormatString, c_viewString), false);

            if (a_viewType != null)
            {
                if (m_currentView != null)
                {
                    m_canvas.RemoveChild(m_currentView, true);
                }

                m_currentView = (GameView)Activator.CreateInstance(a_viewType, m_canvas);

                return true;
            }

            return false;
        }

        public bool ClearView()
        {
            if (m_currentView != null)
            {
                m_canvas.RemoveChild(m_currentView, true);
                m_currentView = null;

                return true;
            }

            return false;
        }

        private void FlushTextCache()
        {
            if (m_renderer.TextCacheSize > 1000)
            {
                m_renderer.FlushTextCache();
            }
        }

        private void HookInputEvents()
        {
            // Keyboard Events
            Keyboard.KeyDown += ProcessKeyDownInput;
            KeyPress += ProcessKeyPressInput;
            Keyboard.KeyUp += ProcessKeyUpInput;

            // Mouse Events
            Mouse.ButtonDown += ProcessMouseInput;
            Mouse.ButtonUp += ProcessMouseInput;
            Mouse.Move += ProcessMouseInput;
            Mouse.WheelChanged += ProcessMouseInput;
        }

        internal void OnCanvasThink()
        {
            if (MouseFocus != null && !MouseFocus.IsVisible)
            {
                MouseFocus = null;
            }

            if (KeyboardFocus != null && (!KeyboardFocus.IsVisible || !KeyboardFocus.KeyboardInputEnabled))
            {
                KeyboardFocus = null;
            }

            if (KeyboardFocus == null)
            {
                return;
            }

            var a_time = UI.Platform.Neutral.GetTimeInSeconds();

            for (var a_idx = 0; a_idx < (int)UI.Key.Count; a_idx++)
            {
                if (m_keyData.m_keyState[a_idx] && m_keyData.m_target != KeyboardFocus)
                {
                    m_keyData.m_keyState[a_idx] = false;
                    continue;
                }

                if (m_keyData.m_keyState[a_idx] && a_time > m_keyData.m_nextRepeat[a_idx])
                {
                    m_keyData.m_nextRepeat[a_idx] = UI.Platform.Neutral.GetTimeInSeconds() + kKeyRepeatDelay;

                    if (KeyboardFocus != null)
                    {
                        KeyboardFocus.InputKeyPressed((UI.Key)a_idx);
                    }
                }
            }
        }

        private UI.Key TranslateKeyCode(OpenTK.Input.Key c_key)
        {
            switch (c_key)
            {
                case OpenTK.Input.Key.BackSpace:
                return UI.Key.Backspace;
                case OpenTK.Input.Key.Enter:
                return UI.Key.Return;
                case OpenTK.Input.Key.Escape:
                return UI.Key.Escape;
                case OpenTK.Input.Key.Tab:
                return UI.Key.Tab;
                case OpenTK.Input.Key.Space:
                return UI.Key.Space;
                case OpenTK.Input.Key.Up:
                return UI.Key.Up;
                case OpenTK.Input.Key.Down:
                return UI.Key.Down;
                case OpenTK.Input.Key.Left:
                return UI.Key.Left;
                case OpenTK.Input.Key.Right:
                return UI.Key.Right;
                case OpenTK.Input.Key.Home:
                return UI.Key.Home;
                case OpenTK.Input.Key.End:
                return UI.Key.End;
                case OpenTK.Input.Key.Delete:
                return UI.Key.Delete;
                case OpenTK.Input.Key.LControl:
                m_altDown = true;
                return UI.Key.Control;
                case OpenTK.Input.Key.LAlt:
                return UI.Key.Alt;
                case OpenTK.Input.Key.LShift:
                return UI.Key.Shift;
                case OpenTK.Input.Key.RControl:
                return UI.Key.Control;
                case OpenTK.Input.Key.RAlt:
                if (m_altDown)
                {
                    m_canvas.Input_Key(UI.Key.Control, false);
                }
                return UI.Key.Alt;
                case OpenTK.Input.Key.RShift:
                return UI.Key.Shift;
            }

            return UI.Key.Invalid;
        }

        private char TranslateChar(OpenTK.Input.Key c_key)
        {
            if (c_key >= OpenTK.Input.Key.A && c_key <= OpenTK.Input.Key.Z)
            {
                return (char)('a' + ((int)c_key - (int)OpenTK.Input.Key.A));
            }

            return ' ';
        }

        private bool DoSpecialKeys(char c_char)
        {
            if (null == KeyboardFocus)
            {
                return false;
            }

            if (KeyboardFocus.GetCanvas() != m_canvas)
            {
                return false;
            }

            if (!KeyboardFocus.IsVisible)
            {
                return false;
            }

            if (!IsControlDown)
            {
                return false;
            }

            if (c_char == 'C' || c_char == 'c')
            {
                KeyboardFocus.InputCopy(null);

                return true;
            }

            if (c_char == 'V' || c_char == 'v')
            {
                KeyboardFocus.InputPaste(null);
                return true;
            }

            if (c_char == 'X' || c_char == 'x')
            {
                KeyboardFocus.InputCut(null);
                return true;
            }

            if (c_char == 'A' || c_char == 'a')
            {
                KeyboardFocus.InputSelectAll(null);
                return true;
            }

            return false;
        }

        private void UpdateHoveredControl()
        {
            var a_hoveredControl = m_canvas.GetControlAt(m_mousePosition.X, m_mousePosition.Y);

            if (HoveredControl != a_hoveredControl)
            {
                if (HoveredControl != null)
                {
                    var a_oldHoveredControl = HoveredControl;
                    HoveredControl = null;
                    a_oldHoveredControl.InputMouseLeft();
                }

                HoveredControl = a_hoveredControl;
            }

            if (MouseFocus != null && MouseFocus.GetCanvas() == m_canvas)
            {
                if (HoveredControl != null)
                {
                    var a_oldHoveredControl = HoveredControl;
                    HoveredControl = null;
                    a_oldHoveredControl.Redraw();
                }

                HoveredControl = MouseFocus;
            }
        }

        private void FindKeyboardFocusedControl(GameControl c_hoveredControl)
        {
            if (c_hoveredControl == null)
            {
                return;
            }

            if (c_hoveredControl.KeyboardInputEnabled)
            {
                if (c_hoveredControl.Children.Any(c_childControl => c_childControl == KeyboardFocus))
                {
                    return;
                }

                c_hoveredControl.Focus();

                return;
            }

            FindKeyboardFocusedControl(c_hoveredControl.Parent);
        }

        public void ProcessMouseInput(object c_sender, EventArgs c_mouseArgs)
        {
            if (m_canvas == null)
            {
                return;
            }
            else if (c_mouseArgs is MouseMoveEventArgs)
            {
                var a_mouseMoveEvent = c_mouseArgs as MouseMoveEventArgs;
                var a_dx = a_mouseMoveEvent.X - m_mousePosition.X;
                var a_dy = a_mouseMoveEvent.Y - m_mousePosition.Y;

                m_mousePosition.X = a_mouseMoveEvent.X;
                m_mousePosition.Y = a_mouseMoveEvent.Y;

                m_canvas.Input_MouseMoved(m_mousePosition.X, m_mousePosition.Y, a_dx, a_dy);
            }
            else if (c_mouseArgs is MouseButtonEventArgs)
            {
                var a_mouseButtonEvent = c_mouseArgs as MouseButtonEventArgs;
                m_canvas.Input_MouseButton((int)a_mouseButtonEvent.Button, a_mouseButtonEvent.IsPressed);
            }
            else if (c_mouseArgs is MouseWheelEventArgs)
            {
                var a_mouseWheelEvent = c_mouseArgs as MouseWheelEventArgs;
                m_canvas.Input_MouseWheel(a_mouseWheelEvent.Delta * 60);
            }
        }

        private void ProcessKeyDownInput(object c_sender, KeyboardKeyEventArgs c_e)
        {
            if (c_e.Key == OpenTK.Input.Key.Escape)
            {
                Exit();
            }
            else if (c_e.Key == OpenTK.Input.Key.F)
            {
                if (m_statusBar == null)
                {
                    m_statusBar = new StatusBar(m_canvas);
                    m_statusBar.Dock = Pos.Bottom;
                    m_statusBar.Hide();
                }

                if (m_statusBar.IsVisible)
                {
                    m_statusBar.Hide();
                }
                else
                {
                    m_statusBar.Show();
                }
            }
            else if (c_e.Key == OpenTK.Input.Key.O)
            {
                if (m_currentView == null)
                {
                    ShowView("TestView");
                }
            }

            var a_char = TranslateChar(c_e.Key);

            if (DoSpecialKeys(a_char))
            {
                return;
            }

            var a_key = TranslateKeyCode(c_e.Key);

            m_canvas.Input_Key(a_key, true);
        }

        private void ProcessKeyPressInput(object c_sender, KeyPressEventArgs c_e)
        {
            m_canvas.Input_Character(c_e.KeyChar);
        }

        private void ProcessKeyUpInput(object c_sender, KeyboardKeyEventArgs c_e)
        {
            var a_key = TranslateKeyCode(c_e.Key);

            m_canvas.Input_Key(a_key, false);
        }

        internal bool OnKeyEvent(UI.Key c_key, bool c_isButtonDown)
        {

            if (MouseFocus is Button && c_key == UI.Key.Space && c_isButtonDown)
            {
                var a_button = HoveredControl as Button;

                a_button.Press();

                return true;
            }

            if (KeyboardFocus == null)
            {
                return false;
            }

            if (KeyboardFocus.GetCanvas() != m_canvas)
            {
                return false;
            }

            if (!KeyboardFocus.IsVisible)
            {
                return false;
            }

            var a_key = (int)c_key;

            if (c_isButtonDown)
            {
                if (!m_keyData.m_keyState[a_key])
                {
                    m_keyData.m_keyState[a_key] = true;
                    m_keyData.m_nextRepeat[a_key] = UI.Platform.Neutral.GetTimeInSeconds() + kKeyRepeatDelay;
                    m_keyData.m_target = KeyboardFocus;

                    return KeyboardFocus.InputKeyPressed(c_key);
                }
            }
            else
            {
                if (m_keyData.m_keyState[a_key])
                {
                    m_keyData.m_keyState[a_key] = false;

                    return KeyboardFocus.InputKeyPressed(c_key, false);
                }
            }

            return false;
        }

        internal bool HandleAccelerator(char c_char)
        {
            var a_acceleratorSearchRaw = new StringBuilder();

            if (IsControlDown)
            {
                a_acceleratorSearchRaw.Append("CTRL+");
            }

            if (IsShiftDown)
            {
                a_acceleratorSearchRaw.Append("SHIFT+");
            }

            a_acceleratorSearchRaw.Append(c_char);

            var a_acceleratorSearchString = a_acceleratorSearchRaw.ToString();

            if (KeyboardFocus != null && KeyboardFocus.HandleAccelerator(a_acceleratorSearchString))
            {
                return true;
            }

            if (MouseFocus != null && MouseFocus.HandleAccelerator(a_acceleratorSearchString))
            {
                return true;
            }

            if (m_canvas.HandleAccelerator(a_acceleratorSearchString))
            {
                return true;
            }

            return false;
        }

        internal void OnMouseMoved(int c_x, int c_y, int c_dx, int c_dy)
        {
            m_mousePosition.X = c_x;
            m_mousePosition.Y = c_y;

            UpdateHoveredControl();
        }

        internal bool OnMouseClicked(int c_mouseButton, bool c_isButtonDown)
        {
            if (c_isButtonDown && (HoveredControl == null || !HoveredControl.IsMenuComponent))
            {
                m_canvas.CloseMenus();
            }

            if (HoveredControl == null || HoveredControl.GetCanvas() != m_canvas || !HoveredControl.IsVisible || HoveredControl == m_canvas)
            {
                return false;
            }

            if (c_mouseButton > kMaxMouseButtons)
            {
                return false;
            }

            if (c_mouseButton == 0)
            {
                m_keyData.m_leftMouseDown = c_isButtonDown;
            }
            else if (c_mouseButton == 1)
            {
                m_keyData.m_rightMouseDown = c_isButtonDown;
            }

            var a_isDoubleClick = false;

            if (c_isButtonDown && m_lastClickedPosition.X == m_mousePosition.X && m_lastClickedPosition.Y == m_lastClickedPosition.Y && (UI.Platform.Neutral.GetTimeInSeconds() - m_lastClickedTime[c_mouseButton] < kMouseDoubleClickSpeed))
            {
                a_isDoubleClick = true;
            }

            if (c_isButtonDown && a_isDoubleClick)
            {
                m_lastClickedTime[c_mouseButton] = UI.Platform.Neutral.GetTimeInSeconds();
                m_lastClickedPosition = MousePosition;
            }

            if (c_isButtonDown)
            {
                FindKeyboardFocusedControl(HoveredControl);
            }

            HoveredControl.UpdateCursor();

            if (c_isButtonDown)
            {
                HoveredControl.Touch();
            }

            switch (c_mouseButton)
            {
                case 0:
                {
                    if (DragAndDrop.OnMouseButton(HoveredControl, MousePosition.X, MousePosition.Y, c_isButtonDown))
                    {
                        return true;
                    }

                    if (a_isDoubleClick)
                    {
                        HoveredControl.InputMouseDoubleClickedLeft(MousePosition.X, MousePosition.Y);
                    }
                    else
                    {
                        HoveredControl.InputMouseClickedLeft(MousePosition.X, MousePosition.Y, c_isButtonDown);
                    }

                    return true;
                }

                case 1:
                {
                    if (a_isDoubleClick)
                    {
                        HoveredControl.InputMouseDoubleClickedRight(MousePosition.X, MousePosition.Y);
                    }
                    else
                    {
                        HoveredControl.InputMouseClickedRight(MousePosition.X, MousePosition.Y, c_isButtonDown);
                    }

                    return true;

                }
            }

            return false;
        }

        public override void Dispose()
        {
            ClearView();

            m_canvas.Dispose();
            m_skin.Dispose();
            m_renderer.Dispose();
            base.Dispose();
        }
    }
}