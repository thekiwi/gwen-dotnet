using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gwen.Controls;
using SFML.Graphics;
using SFML.Window;
using Tao.OpenGl;
using Button = Gwen.Controls.Button;
using CheckBox = Gwen.Controls.CheckBox;
using Color = SFML.Graphics.Color;
using Image = SFML.Graphics.Image;
using KeyEventArgs = SFML.Window.KeyEventArgs;
using Label = Gwen.Controls.Label;
using Menu = Gwen.Controls.Menu;
using TextBox = Gwen.Controls.TextBox;

namespace Gwen.Sample
{
    public class Sample
    {
        private static Input.SFML GwenInput;
        private static Canvas canvas;
        private static RenderWindow window;

        static void Main()
        {
            int width = 800;
            int height = 600;
            // Create main window
            window = new RenderWindow(new VideoMode((uint)width, (uint)height), "GWEN.NET test",
                Styles.Default, new ContextSettings(32, 0));

            // Setup event handlers
            window.Closed += OnClosed;
            window.KeyPressed += OnKeyPressed;
            window.Resized += OnResized;
            window.KeyReleased += window_KeyReleased;
            window.MouseButtonPressed += window_MouseButton;
            window.MouseButtonReleased += window_MouseButton;
            window.MouseWheelMoved += window_MouseWheelMoved;
            window.MouseMoved += window_MouseMoved;
            window.TextEntered += new EventHandler<TextEventArgs>(window_TextEntered);

            int fps_frames = 50;
            List<int> ftime = new List<int>(fps_frames);
            float time = 0.0F;
            long frame = 0;
            Text t = new Text(String.Format("FPS: {0}", frame / (time / 1000.0)));
            t.Position = new Vector2(10, 10);
            t.Color = Color.Red;

            Text btnText = new Text("Button pressed!");
            btnText.Position = new Vector2(200, 100);
            btnText.Color = Color.White;

            Renderer.SFML GwenRenderer = new Renderer.SFML(window);
            
            // Create a GWEN skin
            //Skin.Simple skin = new Skin.Simple(GwenRenderer);
            Skin.TexturedBase skin = new Skin.TexturedBase(GwenRenderer, "DefaultSkin.png");

            // The fonts work differently in SFML - it can't use
            // system fonts. So force the skin to use a local one.
            skin.SetDefaultFont("OpenSans.ttf", 10);
            Font font2 = skin.DefaultFont;
            font2.Size = 15;

            // Create a Canvas (it's root, on which all other GWEN panels are created)
            canvas = new Canvas(skin);
            canvas.SetSize(width, height);
            canvas.DrawBackground = true;
            canvas.BackgroundColor = System.Drawing.Color.FromArgb(255, 150, 170, 170);
            canvas.KeyboardInputEnabled = true;

            LabelClickable label1 = new LabelClickable(canvas);
            label1.SetPos(10, 50);
            label1.AutoSizeToContents = true;
            //label1.Font = skin.DefaultFont; // this does not corrupt text
            label1.SetText("Welcome to GWEN in SFML.NET!");
            label1.TextColor = System.Drawing.Color.Blue;
            //label1.Dock = Pos.Right;
            
            Label label2 = new Label(canvas);
            label2.SetPos(10, 80);
            label2.AutoSizeToContents = true;
            label2.Font = font2;
            label2.MouseInputEnabled = true;
            label2.SetToolTipText("this is a tooltip");
            label2.SetText("Hover mouse here");
            label2.Cursor = Cursors.Cross;
            label2.TextColor = System.Drawing.Color.DeepPink;
            //label2.Dock = Pos.Center;

            Button button1 = new Button(canvas);
            button1.AutoSizeToContents = true;
            button1.SetText("DO STUFF");
            button1.SetPos(10, 110);
            button1.Width = 150;
            button1.Height = 30;
            button1.IsTabable = true;
            button1.KeyboardInputEnabled = true;
            button1.OnPress += button1_OnPress;

            CheckBoxWithLabel cb1 = new CheckBoxWithLabel(canvas);
            cb1.SetPos(10, 140);
            cb1.IsTabable = true;
            cb1.KeyboardInputEnabled = true;
            cb1.Label.SetText("Sample checkbox 1");
            cb1.Label.SetToolTipText("trololo 1");
            cb1.CheckBox.IsChecked = true;

            CheckBoxWithLabel cb2 = new CheckBoxWithLabel(canvas);
            cb2.SetPos(300, 140);
            cb2.IsTabable = true;
            cb2.KeyboardInputEnabled = true;
            cb2.Label.SetText("Sample checkbox 2");
            cb2.Label.SetToolTipText("trololo 2");

            TextBox tb1 = new TextBox(canvas);
            tb1.SetPos(10, 200);
            tb1.SetText("sample edit");
            tb1.CursorPos = 3;
            tb1.CursorEnd = 7; // todo: show even without focus
            
            ScrollControl sc1 = new ScrollControl(canvas);
            sc1.SetBounds(10, 250, 100, 100);
            Button b = new Button(sc1);
            b.SetBounds(0, 0, 200, 200);
            b.SetText("twice as big");
            //sc1.SetScrollPos(0.5f, 0.5f);

            Menu m1 = new Menu(canvas);
            m1.AddItem("item 1");
            var i1 = m1.AddItem("item 2");
            i1.IsCheckable = true;
            i1.Checked = true;
            m1.AddDivider();
            m1.AddItem("item 3");
            m1.SetPos(250, 250);
            
            // Create an input processor
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
                Gl.glClear(Gl.GL_DEPTH_BUFFER_BIT);
                window.SaveGLStates();

                canvas.RenderCanvas();

                ulong frametime = window.GetFrameTime();
                time += frametime;
                frame++;

                if (ftime.Count == fps_frames)
                    ftime.RemoveAt(0);

                ftime.Add((int)frametime);

                if (button1.IsDepressed)
                    window.Draw(btnText);

                t.DisplayedString = String.Format("FPS: {0:F2}", 1000f * ftime.Count / ftime.Sum());
                //t.DisplayedString = String.Format("FPS: {0:F2}", 1000f * frame / w.ElapsedMilliseconds);
                window.Draw(t);

                window.RestoreGLStates();
                window.Display();
            }
        }

        static void window_TextEntered(object sender, TextEventArgs e)
        {
            GwenInput.ProcessMessage(e);
        }

        static void button1_OnPress(Base control)
        {
            
        }

        static void window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            GwenInput.ProcessMessage(e);
        }

        static void window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            GwenInput.ProcessMessage(e);
        }

        static void window_MouseButton(object sender, MouseButtonEventArgs e)
        {
            GwenInput.ProcessMessage(e);
        }

        static void window_KeyReleased(object sender, KeyEventArgs e)
        {
            GwenInput.ProcessMessage(e);
        }

        /// <summary>
        /// Function called when the window is closed
        /// </summary>
        static void OnClosed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            if (e.Code == KeyCode.Escape)
                window.Close();

            if (e.Code == KeyCode.F12)
            {
                Image i = new Image(1, 1);
                i.CopyScreen(window);
                i.SaveToFile(string.Format("screenshot-{0:D2}{1:D2}{2:D2}.png", DateTime.Now.Hour, DateTime.Now.Minute,
                                           DateTime.Now.Second));
            }
            GwenInput.ProcessMessage(e);
        }

        /// <summary>
        /// Function called when the window is resized
        /// </summary>
        static void OnResized(object sender, SizeEventArgs e)
        {
            Gl.glViewport(0, 0, (int)e.Width, (int)e.Height);
            // todo: gwen doesn't handle resizing well
            //canvas.SetSize((int)e.Width, (int)e.Height);
            // window.ConvertCoords()
        }
    }
}
