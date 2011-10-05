using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Gwen.Control;
namespace Gwen.Sample.OpenTKSample
{
    /// <summary>
    /// Demonstrates the GameWindow class.
    /// </summary>
    public class SimpleWindow : GameWindow
    {

        Gwen.Input.OpenTK input = null;
        Gwen.Renderer.Base renderer = null;
        Gwen.Skin.Base skin = null;
        Gwen.Control.Canvas canvas = null;

        private static Label fpsLabel;
        private static RadioButtonGroup rbc2;
        private static TabControl tab;

        private static Label _ColorText;


        public SimpleWindow()
            : base(800, 600)
        {


            Keyboard.KeyDown += Keyboard_KeyDown;
            Keyboard.KeyUp += Keyboard_KeyUp;

            Mouse.ButtonDown += Mouse_ButtonDown;
            Mouse.ButtonUp += Mouse_ButtonUp;
            Mouse.Move += Mouse_Move;
            Mouse.WheelChanged += Mouse_Wheel;

        }


        #region Keyboard_KeyDown

        /// <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        /// <param name="sender">The KeyboardDevice which generated this event.</param>
        /// <param name="e">The key that was pressed.</param>
        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == global::OpenTK.Input.Key.Escape)
                this.Exit();

            if ((e.Key == global::OpenTK.Input.Key.AltLeft 
                || e.Key == global::OpenTK.Input.Key.AltRight)
                && (e.Key == global::OpenTK.Input.Key.Enter 
                || e.Key == global::OpenTK.Input.Key.KeypadEnter))
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;


            input.ProcessKeyDown(e);


        }

        void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
 
            input.ProcessKeyUp(e);


        }

        void Mouse_ButtonDown(object sender, MouseButtonEventArgs args)
        {

            input.ProcessMouseMessage(args);
        }


        void Mouse_ButtonUp(object sender, MouseButtonEventArgs args)
        {

            input.ProcessMouseMessage(args);
        }

        void Mouse_Move(object sender, MouseMoveEventArgs args)
        {

            input.ProcessMouseMessage(args);
        }

        void Mouse_Wheel(object sender, MouseWheelEventArgs args)
        {

            input.ProcessMouseMessage(args);
        }

        #endregion

        #region OnLoad

        /// <summary>
        /// Setup OpenGL and load resources here.
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.MidnightBlue);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, this.Width, this.Height, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Viewport(0, 0, this.Width, this.Height);

            
            renderer = new Gwen.Renderer.OpenTK();
            skin = new Gwen.Skin.TexturedBase(renderer, "DefaultSkin.png");
            //skin = new Gwen.Skin.Simple(renderer);
            canvas = new Canvas(skin);

            this.input = new Input.OpenTK();
            this.input.Initialize(canvas);

            canvas.SetSize(this.Width, this.Height);
            canvas.ShouldDrawBackground = true;
            canvas.BackgroundColor = Color.FromArgb(255, 150, 170, 170);
            //canvas.KeyboardInputEnabled = true;

            MenuStrip ms = new MenuStrip(canvas);
            ms.Dock = Pos.Top;
            //ms.SetPosition(300, 20);
            var root = ms.AddItem("File");
            var item = root.Menu.AddItem("New", "test16.png");
            item.Menu.AddItem("Account");
            item.Menu.AddItem("Character", "test16.png");
            root.Menu.AddItem("Load (works)", "test16.png").Selected += Sample_OnMenuItemSelectedLoad;
            root.Menu.AddItem("Save");
            root.Menu.AddDivider();
            root.Menu.AddItem("Quit (works)").Selected += Sample_OnMenuItemSelectedQuit;
            //ms.ShouldCacheToTexture = true;
            // ms.AddDivider(); // no vertical dividers yet

            root = ms.AddItem("zażółć", "test16.png");
            root.Menu.AddItem("gęślą");
            root.Menu.AddItem("jaźń");
            item = root.Menu.AddItem("checkable");
            item.IsCheckable = true;
            item.IsChecked = true;

            /////////////////////////////////////////////////////////
            // bug: if this is moved to the end, tooltips are kind of screwed
            ScrollControl sc1 = new ScrollControl(canvas);
            sc1.SetBounds(10, 250, 100, 100);
            Button b = new Button(sc1);
            b.SetBounds(0, 0, 200, 200);
            b.Text = "twice as big";
            //b.ShouldCacheToTexture = true;
            //sc1.SetScrollPos(0.5f, 0.5f);
            //sc1.ShouldCacheToTexture = true;

            ComboBox cb = new ComboBox(canvas);
            cb.SetPosition(200, 50);
            cb.KeyboardInputEnabled = true;
            cb.AddItem("item 1", "a");
            cb.AddItem("item 2", "b");
            cb.AddItem("item 3", "c");
            cb.AddItem("item 4", "d");
            cb.AddItem("item 5", "e");
            cb.AddItem("item 6", "f");
            //cb.ShouldCacheToTexture = true;

            LabelClickable label1 = new LabelClickable(canvas);
            label1.SetBounds(10, 50, 100, 10);
            label1.AutoSizeToContents = true;
            label1.Text = "Welcome to GWEN in SFML.NET!";
            label1.TextColor = System.Drawing.Color.Blue;
            //label1.Dock = Pos.Right;

            Label label2 = new Label(canvas);
            label2.SetPosition(10, 80);
            label2.AutoSizeToContents = true;
            //label2.Font = font2;
            label2.MouseInputEnabled = true;
            label2.SetToolTipText("this is a tooltip");
            label2.Text = "Hover mouse here";
            label2.Cursor = System.Windows.Forms.Cursors.Cross;
            label2.TextColor = System.Drawing.Color.DeepPink;
            //label2.Dock = Pos.Center;

            Button button1 = new Button(canvas);
            button1.AutoSizeToContents = true;
            button1.Text = "DO STUFF";
            button1.SetPosition(10, 110);
            button1.Width = 150;
            button1.Height = 30;
            button1.IsTabable = true;
            button1.KeyboardInputEnabled = true;
            button1.Clicked += button1_OnPress;

            LabeledCheckBox cb1 = new LabeledCheckBox(canvas);
            cb1.SetPosition(10, 140);
            cb1.IsTabable = true;
            cb1.KeyboardInputEnabled = true;
            cb1.Text = "Sample checkbox 1";
            cb1.SetToolTipText("trololo 1");
            cb1.IsChecked = true;

            LabeledCheckBox cb2 = new LabeledCheckBox(canvas);
            cb2.SetPosition(200, 140);
            cb2.IsTabable = true;
            cb2.KeyboardInputEnabled = true;
            cb2.Text = "Sample checkbox 2";
            cb2.SetToolTipText("trololo 2");

            TextBox tb1 = new TextBox(canvas);
            tb1.SetPosition(10, 180);
            tb1.Text = "sample edit";
            tb1.CursorPos = 3;
            tb1.CursorEnd = 7; // todo: show even without focus

            TextBoxNumeric tb2 = new TextBoxNumeric(canvas);
            tb2.SetPosition(10, 200);
            tb2.Text = "123.4asdasd"; // this fails
            tb2.Text = "123.4"; // ok
            tb2.SelectAllOnFocus = true;

            NumericUpDown n1 = new NumericUpDown(canvas);
            n1.SetPosition(10, 220);
            n1.Min = -10;
            n1.Text = "-51"; // this fails
            n1.Text = "-5"; // ok

            RadioButtonGroup rb1 = new RadioButtonGroup(canvas);
            rb1.AddOption("Option 1");
            rb1.AddOption("Option 2");
            rb1.AddOption("Option 3");
            rb1.AddOption("zażółć gęślą jaźń");
            rb1.SetBounds(10, 350, 150, 200);
            rb1.SetSelection(1);
            rb1.SetSelection(2); // overrides above

            GroupBox gb1 = new GroupBox(canvas);
            gb1.SetBounds(130, 250, 250, 120);
            gb1.Text = "Listbox test";

            ListBox lb1 = new ListBox(gb1);
            lb1.ColumnCount = 3;
            lb1.SetSize(150, 100);
            lb1.Dock = Pos.Left;
            lb1.AllowMultiSelect = true;
            var row = lb1.AddRow("multiselect");
            row.SetCellText(1, "is");
            row.SetCellText(2, "on");
            lb1.AddRow("item 2");
            lb1.AddRow("item 3").SetCellText(2, "3rd column");
            row = lb1.AddRow("zażółć");
            row.SetCellText(1, "gęślą");
            row.SetCellText(2, "jaźń");
            lb1.SelectRow(1);
            lb1.SelectRow(3);

            ListBox lb2 = new ListBox(gb1);
            lb2.SetSize(70, 100);
            lb2.Dock = Pos.Left;
            lb2.AddRow("row 1");
            lb2.AddRow("row 2");
            lb2.AddRow("row 3");
            lb2.AddRow("row 4");
            lb2.SelectRow(0); // this will be unselected since it's not multiselect
            lb2.SelectRow(1);
            lb2.RemoveRow(2);

            HSVColorPicker cp1 = new HSVColorPicker(canvas);
            cp1.SetPosition(400, 50);
            cp1.SetColor(System.Drawing.Color.Green);
            cp1.ColorChanged += OnColorChanged;

            ColorPicker cp2 = new ColorPicker(canvas);
            cp2.SetPosition(400, 250);
            cp2.ColorChanged += OnColorChanged;

            _ColorText = new Label(canvas);
            _ColorText.SetPosition(400, 200);
            _ColorText.AutoSizeToContents = true;

            CrossSplitter spl = new CrossSplitter(canvas);
            spl.SetBounds(150, 380, 150, 150);
            spl.SplittersVisible = true;

            Gwen.Control.Button btl = new Button(spl);
            btl.Text = "Top Left";
            spl.SetPanel(0, btl);
            Button btr = new Button(spl);
            btr.Text = "Top Right";
            spl.SetPanel(1, btr);
            Button bbl = new Button(spl);
            bbl.Text = "Bottom Left";
            spl.SetPanel(2, bbl);
            Button bbr = new Button(spl);
            bbr.Text = "Bottom Right";
            spl.SetPanel(3, bbr);

            tab = new TabControl(canvas);
            tab.SetBounds(420, 400, 200, 150);

            var tabbtn1 = tab.AddPage("Controls");

            rbc2 = new RadioButtonGroup(tabbtn1.Page);
            rbc2.SetBounds(10, 10, 100, 100);
            rbc2.AddOption("Top").Select();
            rbc2.AddOption("Bottom");
            rbc2.AddOption("Left");
            rbc2.AddOption("Right");
            rbc2.SelectionChanged += rbc2_OnSelectionChange;

            tab.AddPage("Red");
            tab.AddPage("Green");
            tab.AddPage("Blue");

            tab.AllowReorder = true;

        }

        #endregion

        #region OnResize

        /// <summary>
        /// Respond to resize events here.
        /// </summary>
        /// <param name="e">Contains information on the new GameWindow size.</param>
        /// <remarks>There is no need to call the base implementation.</remarks>
        protected override void OnResize(EventArgs e)
        {


            GL.Viewport(0, 0, Width, Height);

            canvas.SetSize(Width, Height);

        }

        #endregion

        #region OnUpdateFrame

        /// <summary>
        /// Add your game logic here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        /// <remarks>There is no need to call the base implementation.</remarks>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Nothing to do!
        }

        #endregion

        #region OnRenderFrame

        /// <summary>
        /// Add your game rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        /// <remarks>There is no need to call the base implementation.</remarks>
        protected override void OnRenderFrame(FrameEventArgs e)
        {

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            canvas.RenderCanvas();

            /*
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(BeginMode.Triangles);

            GL.Color3(Color.MidnightBlue);
            GL.Vertex2(-1.0f, 1.0f);
            GL.Color3(Color.SpringGreen);
            GL.Vertex2(0.0f, -1.0f);
            GL.Color3(Color.Ivory);
            GL.Vertex2(1.0f, 1.0f);

            GL.End();
            */
            this.SwapBuffers();
        }

        #endregion

        #region public static void Main()

        /// <summary>
        /// Entry point of this example.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (SimpleWindow example = new SimpleWindow())
            {
                example.Title = "Gwen-DotNet OpenTK test";
                example.Run(30.0, 0.0);
            }
        }


        void Sample_OnMenuItemSelectedQuit(Base control)
        {
            this.Close();
        }

        void Sample_OnMenuItemSelectedLoad(Base control)
        {
            Gwen.Platform.Neutral.FileOpen("Open file test", @"c:\", "All files(*.*)|*.*", OnFileOpen);
        }

        static void OnFileOpen(String file)
        {
            System.Windows.Forms.MessageBox.Show("File opened: " + file);
        }

        void button1_OnPress(Base control)
        {
        }

        void OnColorChanged(Base control)
        {
            var picker = control as IColorPicker;
            var c = picker.SelectedColor;
            var hsv = c.ToHSV();
            _ColorText.Text = String.Format("RGB: {0:X2}{1:X2}{2:X2} HSV: {3:F1} {4:F2} {5:F2}",
                                            c.R, c.G, c.B, hsv.h, hsv.s, hsv.v);
        }

        void rbc2_OnSelectionChange(Base control)
        {
            RadioButtonGroup rc = control as RadioButtonGroup;

            if (rc.SelectedLabel == "Top") tab.TabStripPosition = Pos.Top;
            if (rc.SelectedLabel == "Bottom") tab.TabStripPosition = Pos.Bottom;
            if (rc.SelectedLabel == "Left") tab.TabStripPosition = Pos.Left;
            if (rc.SelectedLabel == "Right") tab.TabStripPosition = Pos.Right;
        }

        

        #endregion

    }
}
