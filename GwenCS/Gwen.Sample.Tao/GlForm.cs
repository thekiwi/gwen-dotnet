using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Gwen.Control;
using Tao.OpenGl;

namespace Gwen.Sample.Tao
{
    public partial class GlForm : Form
    {
        private Canvas canvas;
        private Gwen.Renderer.Tao renderer;
        private Gwen.Skin.Base skin;
        private Gwen.UnitTest.UnitTest test;

        const int fps_frames = 50;
        private readonly List<long> ftime;
        private readonly Stopwatch stopwatch;
        private long lastTime;

        public GlForm()
        {
            ftime = new List<long>();
            stopwatch = new Stopwatch();
            
            InitializeComponent();

            Width = 1024;
            Height = 768;
        }

        private void GlForm_Load(object sender, EventArgs e)
        {
            glControl.InitializeContexts();
            Gl.glClearColor(1f, 0f, 0f, 1f);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glOrtho(0, glControl.Width, glControl.Height, 0, -1, 1);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glViewport(0, 0, glControl.Width, glControl.Height);

            renderer = new Gwen.Renderer.Tao();
            skin = new Gwen.Skin.TexturedBase(renderer, "DefaultSkin.png");
            //skin = new Gwen.Skin.Simple(renderer);
            canvas = new Canvas(skin);
            canvas.SetSize(glControl.Width, glControl.Height);
            canvas.ShouldDrawBackground = true;
            canvas.BackgroundColor = Color.FromArgb(255, 150, 170, 170);
            //canvas.KeyboardInputEnabled = true;

            test = new UnitTest.UnitTest(canvas);

            stopwatch.Restart();
            lastTime = 0;
        }

        private void GlForm_Resize(object sender, EventArgs e)
        {
            if (canvas != null)
                canvas.SetSize(glControl.Width, glControl.Height);
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (ftime.Count == fps_frames)
                ftime.RemoveAt(0);

            ftime.Add(stopwatch.ElapsedMilliseconds - lastTime);
            lastTime = stopwatch.ElapsedMilliseconds;

            if (stopwatch.ElapsedMilliseconds > 1000)
            {
                test.Note = String.Format("String Cache size: {0}", renderer.TextCacheSize);
                test.Fps = 1000f * ftime.Count / ftime.Sum();
                stopwatch.Restart();

                if (renderer.TextCacheSize > 1000) // each cached string is an allocated texture, flush the cache once in a while in your real project
                    renderer.FlushTextCache();
            }

            Gl.glClear(Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_COLOR_BUFFER_BIT);
            canvas.RenderCanvas();
        }
    }
}
