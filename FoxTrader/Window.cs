using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using FoxTrader.Game;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using FoxTrader.UI.Input;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using static FoxTrader.Constants;

namespace FoxTrader
{
    internal class FoxTraderWindow : GameWindow
    {
        const int kFpsFrames = 50;

        private static FoxTraderWindow m_gameWindowInstance;

        private Skin m_skin;
        private StatusBar m_statusBar;

        private readonly List<long> m_frameTime;
        private readonly Stopwatch m_stopwatch;
        private long m_lastFrameTime;
        private double m_fps;

        private const string kViewsFormatString = "FoxTrader.Views.{0}";

        private Canvas m_canvas;
        private BaseGameView m_currentView;

        private static readonly object m_windowContextLock = new object();

        public Renderer Renderer
        {
            get;
            private set;
        }

        internal static FoxTraderWindow Instance
        {
            get
            {
                lock (m_windowContextLock)
                {
                    if (m_gameWindowInstance != null)
                    {
                        return m_gameWindowInstance;
                    }

                    throw new AccessViolationException("Too early baby...");
                }
            }
        }

        public FoxTraderWindow() : base(kDefaultWinWidth, kDefaultWinHeight)
        {
            lock (m_windowContextLock)
            {
                Location = new Point(42, 42);

                m_frameTime = new List<long>(kFpsFrames);
                m_stopwatch = new Stopwatch();

                m_gameWindowInstance = this;

                Log.Info("New Instance Created", "WINDOW");
            }
        }

        public Skin Skin => m_skin;

        public GameControlDevices GetGameControlDevices()
        {
            return new GameControlDevices { Keyboard = Keyboard, Mouse = Mouse };
        }

        protected override void OnLoad(EventArgs c_e)
        {
            Time.Initialize();

            GL.ClearColor(Color.Black);

            Renderer = new Renderer();

            m_skin = new Skin(Renderer, "png_FoxTraderSkin") { DefaultFont = Renderer.GetFont(kDefaultGameFontName) };

            m_canvas = new Canvas();

            m_canvas.SetSize(Width, Height);
            m_canvas.ShouldDrawBackground = true;
            m_canvas.BackgroundColor = Color.FromArgb(255, 0, 0, 12);

            m_stopwatch.Restart();
            m_lastFrameTime = 0;

            GameContext.Instance.StateChanged += GameContextInstance_StateChanged;
            GameContextInstance_StateChanged(ContextState.Bumpers);

            Log.Info("Window is ready", "WINDOW");
        }

        private void GameContextInstance_StateChanged(ContextState c_newState)
        {
            Log.Info($"StateChanged: {c_newState}", "WINDOW");

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

                case ContextState.NewGame:
                {
                    ShowView("NewGameView");
                }
                break;

                case ContextState.Game:
                {
                    ShowView("GameView");
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

            Log.Info($"OnResize: {Width}, {Height}", "WINDOW");
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

            GameContext.Instance.Tick();

            m_canvas.UpdateCanvas();
        }

        protected override void OnRenderFrame(FrameEventArgs c_e)
        {
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            m_canvas.RenderCanvas();

            SwapBuffers();

            if (m_statusBar != null)
            {
                m_statusBar.Text = $"{m_fps:F0}fps - {Renderer.DrawCallCount}dc - {Renderer.VertexTotalCount}vc";
            }
        }

        public void ShowView(string c_viewString)
        {
            Log.Info($"Showing view: {c_viewString}", "WINDOW");

            var a_viewType = Type.GetType(string.Format(kViewsFormatString, c_viewString), false);

            if (a_viewType != null)
            {
                if (m_currentView != null)
                {
                    m_canvas.RemoveChild(m_currentView, true);
                }

                m_currentView = (BaseGameView)Activator.CreateInstance(a_viewType, m_canvas);
            }
            else
            {
                new MessageBox(m_canvas, "Cannot find view " + c_viewString, "View Error").MakeModal();
            }
        }

        public bool ClearView()
        {
            if (m_currentView != null)
            {
                Log.Info($"Clearing view: {m_currentView}", "WINDOW");

                m_canvas.RemoveChild(m_currentView, true);
                m_currentView = null;

                return true;
            }

            return false;
        }

        private void FlushTextCache()
        {
            if (Renderer.TextCacheSize > 1000)
            {
                Log.Info($"Flushing text cache: {Renderer.TextCacheSize}", "WINDOW");
                Renderer.FlushTextCache();
            }
        }

        public override void Dispose()
        {
            ClearView();

            m_canvas.Dispose();
            m_skin.Dispose();
            Renderer.Dispose();
            base.Dispose();
        }
    }
}