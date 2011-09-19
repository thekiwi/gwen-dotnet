using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Gwen.Control;
using SFML.Graphics;
using SFML.Window;
using Tao.OpenGl;
using KeyEventArgs = SFML.Window.KeyEventArgs;

namespace Gwen.Sample.SFML
{
    public class Program
    {
        private static Input.SFML GwenInput;
        private static RenderWindow window;

        private static Canvas canvas;
        private static UnitTest.UnitTest m_UnitTest;

        [STAThread]
        static void Main()
        {
            //try
            {
                const int width = 1004;
                const int height = 650;

                // Create main window
                window = new RenderWindow(new VideoMode((uint)width, (uint)height), "GWEN.Net SFML test",
                                          Styles.Default, new ContextSettings(32, 0));

                // Setup event handlers
                window.Closed += OnClosed;
                window.KeyPressed += OnKeyPressed;
                window.Resized += OnResized;
                window.KeyReleased += window_KeyReleased;
                window.MouseButtonPressed += window_MouseButtonPressed;
                window.MouseButtonReleased += window_MouseButtonReleased;
                window.MouseWheelMoved += window_MouseWheelMoved;
                window.MouseMoved += window_MouseMoved;
                window.TextEntered += window_TextEntered;

                const int fps_frames = 50;
                List<int> ftime = new List<int>(fps_frames);

                // create GWEN renderer
                Renderer.SFML GwenRenderer = new Renderer.SFML(window);

                // Create GWEN skin
                //Skin.Simple skin = new Skin.Simple(GwenRenderer);
                Skin.TexturedBase skin = new Skin.TexturedBase(GwenRenderer, "DefaultSkin.png");

                // set default font
                skin.SetDefaultFont("Arial Unicode MS", 10);
                //skin.SetDefaultFont("OpenSans.ttf", 10);

                // Create a Canvas (it's root, on which all other GWEN controls are created)
                canvas = new Canvas(skin);
                canvas.SetSize(width, height);
                canvas.ShouldDrawBackground = true;
                canvas.BackgroundColor = System.Drawing.Color.FromArgb(255, 150, 170, 170);
                canvas.KeyboardInputEnabled = true;

                // create the unit test control
                m_UnitTest = new UnitTest.UnitTest(canvas);

                // Create GWEN input processor
                GwenInput = new Input.SFML();
                GwenInput.Initialize(canvas);

                Stopwatch w = new Stopwatch();
                w.Start();
                while (window.IsOpened())
                {
                    window.SetActive();
                    window.DispatchEvents();
                    window.Clear();

                    // Clear depth buffer
                    Gl.glClear(Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_COLOR_BUFFER_BIT);

                    window.SaveGLStates();

                    ulong frametime = window.GetFrameTime();

                    if (ftime.Count == fps_frames)
                        ftime.RemoveAt(0);

                    ftime.Add((int)frametime);


                    if (w.ElapsedMilliseconds > 1000)
                    {
                        m_UnitTest.Fps = 1000f * ftime.Count / ftime.Sum();
                        w.Restart();
                    }

                    // render GWEN canvas
                    canvas.RenderCanvas();

                    window.RestoreGLStates();

                    window.Display();
                }
            }
            //catch (Exception e)
            //{
                //String msg = String.Format("Exception: {0}\n{1}", e.Message, e.StackTrace);
                //MessageBox.Show(msg);
            //}
        }

        static void window_TextEntered(object sender, TextEventArgs e)
        {
            GwenInput.ProcessMessage(e);
        }

        static void window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            GwenInput.ProcessMessage(e);
        }

        static void window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            GwenInput.ProcessMessage(e);
        }

        static void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            GwenInput.ProcessMessage(new Input.SFMLMouseButtonEventArgs(e, true));
        }

        static void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            GwenInput.ProcessMessage(new Input.SFMLMouseButtonEventArgs(e, false));
        }

        static void window_KeyReleased(object sender, KeyEventArgs e)
        {
            GwenInput.ProcessMessage(new Input.SFMLKeyEventArgs(e, false));
        }

        /// <summary>
        /// Function called when the window is closed
        /// </summary>
        static void OnClosed(object sender, EventArgs e)
        {
            window.Close();
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
                window.Close();

            if (e.Code == Keyboard.Key.F12)
            {
                Image img = window.Capture();
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
                GwenInput.ProcessMessage(new Input.SFMLKeyEventArgs(e, true));
        }

        /// <summary>
        /// Function called when the window is resized
        /// </summary>
        static void OnResized(object sender, SizeEventArgs e)
        {
            Gl.glViewport(0, 0, (int)e.Width, (int)e.Height);
            // todo: gwen/sfml doesn't handle resizing well
            canvas.SetSize((int)e.Width, (int)e.Height);
            // window.ConvertCoords()
        }
    }
}
