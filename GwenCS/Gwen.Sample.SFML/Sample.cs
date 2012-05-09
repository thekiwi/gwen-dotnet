using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gwen.Control;
using SFML.Graphics;
using SFML.Window;
using Tao.OpenGl;
using KeyEventArgs = SFML.Window.KeyEventArgs;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Gwen.Sample.SFML
{
    public class Program
    {
        private static Input.SFML m_Input;
        private static RenderWindow m_Window;

        private static Canvas m_Canvas;
        private static UnitTest.UnitTest m_UnitTest;

        [STAThread]
        static void Main()
        {
            //try
            {
                const int width = 1024;
                const int height = 768;

                // Create main window
                m_Window = new RenderWindow(new VideoMode(width, height), "GWEN.Net SFML test", Styles.Titlebar|Styles.Close|Styles.Resize, new ContextSettings(32, 0));

                // Setup event handlers
                m_Window.Closed += OnClosed;
                m_Window.KeyPressed += OnKeyPressed;
                m_Window.Resized += OnResized;
                m_Window.KeyReleased += window_KeyReleased;
                m_Window.MouseButtonPressed += window_MouseButtonPressed;
                m_Window.MouseButtonReleased += window_MouseButtonReleased;
                m_Window.MouseWheelMoved += window_MouseWheelMoved;
                m_Window.MouseMoved += window_MouseMoved;
                m_Window.TextEntered += window_TextEntered;

                //m_Window.SetFramerateLimit(60);

                const int fps_frames = 50;
                List<long> ftime = new List<long>(fps_frames);
                long lastTime = 0;

                // create GWEN renderer
                Renderer.SFML gwenRenderer = new Renderer.SFML(m_Window);

                // Create GWEN skin
                //Skin.Simple skin = new Skin.Simple(GwenRenderer);
                Skin.TexturedBase skin = new Skin.TexturedBase(gwenRenderer, "DefaultSkin.png");

                // set default font
                Font defaultFont = new Font(gwenRenderer) {Size = 10, FaceName = "Arial Unicode MS"};
                
                // try to load, fallback if failed
                if (gwenRenderer.LoadFont(defaultFont))
                {
                    gwenRenderer.FreeFont(defaultFont);
                }
                else // try another
                {
                    defaultFont.FaceName = "Arial";
                    if (gwenRenderer.LoadFont(defaultFont))
                    {
                        gwenRenderer.FreeFont(defaultFont);
                    }
                    else // try default
                    {
                        defaultFont.FaceName = "OpenSans.ttf";
                    }
                }

                skin.SetDefaultFont(defaultFont.FaceName);
                defaultFont.Dispose(); // skin has its own

                // Create a Canvas (it's root, on which all other GWEN controls are created)
                m_Canvas = new Canvas(skin);
                m_Canvas.SetSize(width, height);
                m_Canvas.ShouldDrawBackground = true;
                m_Canvas.BackgroundColor = System.Drawing.Color.FromArgb(255, 150, 170, 170);
                m_Canvas.KeyboardInputEnabled = true;

                // create the unit test control
                m_UnitTest = new UnitTest.UnitTest(m_Canvas);

                // Create GWEN input processor
                m_Input = new Input.SFML();
                m_Input.Initialize(m_Canvas, m_Window);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (m_Window.IsOpen())
                {
                    m_Window.SetActive();
                    m_Window.DispatchEvents();
                    m_Window.Clear();

                    // Clear depth buffer
                    Gl.glClear(Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_COLOR_BUFFER_BIT);

                    if (ftime.Count == fps_frames)
                        ftime.RemoveAt(0);

                    ftime.Add(stopwatch.ElapsedMilliseconds - lastTime);
                    lastTime = stopwatch.ElapsedMilliseconds;

                    if (stopwatch.ElapsedMilliseconds > 1000)
                    {
                        m_UnitTest.Fps = 1000f * ftime.Count / ftime.Sum();
                        stopwatch.Restart();
                    }

                    // render GWEN canvas
                    m_Canvas.RenderCanvas();

                    m_Window.Display();
                }

                // we only need to dispose the canvas, it will take care of disposing all its children
                m_Canvas.Dispose();
                
                // also dispose of these
                skin.Dispose();
                gwenRenderer.Dispose();
            }
            //catch (Exception e)
            //{
                //String msg = String.Format("Exception: {0}\n{1}", e.Message, e.StackTrace);
                //MessageBox.Show(msg);
            //}

            m_Window.Dispose();
        }

        static void window_TextEntered(object sender, TextEventArgs e)
        {
            m_Input.ProcessMessage(e);
        }

        static void window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            m_Input.ProcessMessage(e);
        }

        static void window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            m_Input.ProcessMessage(e);
        }

        static void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            m_Input.ProcessMessage(new Input.SFMLMouseButtonEventArgs(e, true));
        }

        static void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            m_Input.ProcessMessage(new Input.SFMLMouseButtonEventArgs(e, false));
        }

        static void window_KeyReleased(object sender, KeyEventArgs e)
        {
            m_Input.ProcessMessage(new Input.SFMLKeyEventArgs(e, false));
        }

        /// <summary>
        /// Function called when the window is closed
        /// </summary>
        static void OnClosed(object sender, EventArgs e)
        {
            m_Window.Close();
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
                m_Window.Close();

            if (e.Code == Keyboard.Key.F12)
            {
                Image img = m_Window.Capture();
                if (img.Pixels == null)
                {
                    MessageBox.Show("Failed to capture window");
                }
                String path = String.Format("screenshot-{0:D2}{1:D2}{2:D2}.png", DateTime.Now.Hour, DateTime.Now.Minute,
                                            DateTime.Now.Second);
                if (!img.SaveToFile(path))
                    MessageBox.Show(path, "Failed to save screenshot");
                img.Dispose();
            }
            else
                m_Input.ProcessMessage(new Input.SFMLKeyEventArgs(e, true));
        }

        /// <summary>
        /// Function called when the window is resized
        /// </summary>
        static void OnResized(object sender, SizeEventArgs e)
        {
            m_Window.SetView(new View(new FloatRect(0f, 0f, e.Width, e.Height)));
            m_Canvas.SetSize((int)e.Width, (int)e.Height);
        }
    }
}
