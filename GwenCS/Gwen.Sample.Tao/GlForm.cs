using System;
using System.Drawing;
using System.Windows.Forms;
using Gwen.Controls;
using Tao.OpenGl;
using Button = Gwen.Controls.Button;
using ComboBox = Gwen.Controls.ComboBox;
using GroupBox = Gwen.Controls.GroupBox;
using Label = Gwen.Controls.Label;
using ListBox = Gwen.Controls.ListBox;
using MenuStrip = Gwen.Controls.MenuStrip;
using NumericUpDown = Gwen.Controls.NumericUpDown;
using TabControl = Gwen.Controls.TabControl;
using TextBox = Gwen.Controls.TextBox;

namespace Gwen.Sample.Tao
{
    public partial class GlForm : Form
    {
        private Canvas canvas;
        private Gwen.Renderer.Tao renderer;
        private Gwen.Skin.Base skin;

        private static Label fpsLabel;
        private static RadioButtonController rbc2;
        private static TabControl tab;

        private static Label _ColorText;

        public GlForm()
        {
            InitializeComponent();
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

            MenuStrip ms = new MenuStrip(canvas);
            ms.Dock = Pos.Top;
            //ms.SetPos(300, 20);
            var root = ms.AddItem("File");
            var item = root.Menu.AddItem("New", "test16.png");
            item.Menu.AddItem("Account");
            item.Menu.AddItem("Character", "test16.png");
            root.Menu.AddItem("Load (works)", "test16.png").OnMenuItemSelected += Sample_OnMenuItemSelectedLoad;
            root.Menu.AddItem("Save");
            root.Menu.AddDivider();
            root.Menu.AddItem("Quit (works)").OnMenuItemSelected += Sample_OnMenuItemSelectedQuit;
            //ms.ShouldCacheToTexture = true;
            // ms.AddDivider(); // no vertical dividers yet

            root = ms.AddItem("zażółć", "test16.png");
            root.Menu.AddItem("gęślą");
            root.Menu.AddItem("jaźń");
            item = root.Menu.AddItem("checkable");
            item.IsCheckable = true;
            item.Checked = true;

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
            cb.SetPos(200, 50);
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
            label2.SetPos(10, 80);
            label2.AutoSizeToContents = true;
            //label2.Font = font2;
            label2.MouseInputEnabled = true;
            label2.SetToolTipText("this is a tooltip");
            label2.Text = "Hover mouse here";
            label2.Cursor = Cursors.Cross;
            label2.TextColor = System.Drawing.Color.DeepPink;
            //label2.Dock = Pos.Center;

            Button button1 = new Button(canvas);
            button1.AutoSizeToContents = true;
            button1.Text = "DO STUFF";
            button1.SetPos(10, 110);
            button1.Width = 150;
            button1.Height = 30;
            button1.IsTabable = true;
            button1.KeyboardInputEnabled = true;
            button1.OnPress += button1_OnPress;

            LabeledCheckBox cb1 = new LabeledCheckBox(canvas);
            cb1.SetPos(10, 140);
            cb1.IsTabable = true;
            cb1.KeyboardInputEnabled = true;
            cb1.Text = "Sample checkbox 1";
            cb1.SetToolTipText("trololo 1");
            cb1.IsChecked = true;

            LabeledCheckBox cb2 = new LabeledCheckBox(canvas);
            cb2.SetPos(200, 140);
            cb2.IsTabable = true;
            cb2.KeyboardInputEnabled = true;
            cb2.Text = "Sample checkbox 2";
            cb2.SetToolTipText("trololo 2");

            TextBox tb1 = new TextBox(canvas);
            tb1.SetPos(10, 180);
            tb1.Text = "sample edit";
            tb1.CursorPos = 3;
            tb1.CursorEnd = 7; // todo: show even without focus

            TextBoxNumeric tb2 = new TextBoxNumeric(canvas);
            tb2.SetPos(10, 200);
            tb2.Text = "123.4asdasd"; // this fails
            tb2.Text = "123.4"; // ok
            tb2.SelectAllOnFocus = true;

            NumericUpDown n1 = new NumericUpDown(canvas);
            n1.SetPos(10, 220);
            n1.Min = -10;
            n1.Text = "-51"; // this fails
            n1.Text = "-5"; // ok

            RadioButtonController rb1 = new RadioButtonController(canvas);
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
            var row = lb1.AddItem("multiselect");
            row.SetCellText(1, "is");
            row.SetCellText(2, "on");
            lb1.AddItem("item 2");
            lb1.AddItem("item 3").SetCellText(2, "3rd column");
            row = lb1.AddItem("zażółć");
            row.SetCellText(1, "gęślą");
            row.SetCellText(2, "jaźń");
            lb1.SelectRow(1);
            lb1.SelectRow(3);

            ListBox lb2 = new ListBox(gb1);
            lb2.SetSize(70, 100);
            lb2.Dock = Pos.Left;
            lb2.AddItem("row 1");
            lb2.AddItem("row 2");
            lb2.AddItem("row 3");
            lb2.AddItem("row 4");
            lb2.SelectRow(0); // this will be unselected since it's not multiselect
            lb2.SelectRow(1);
            lb2.RemoveRow(2);

            HSVColorPicker cp1 = new HSVColorPicker(canvas);
            cp1.SetPos(400, 50);
            cp1.SetColor(System.Drawing.Color.Green);
            cp1.OnColorChanged += OnColorChanged;

            ColorPicker cp2 = new ColorPicker(canvas);
            cp2.SetPos(400, 250);
            cp2.OnColorChanged += OnColorChanged;

            _ColorText = new Label(canvas);
            _ColorText.SetPos(400, 200);
            _ColorText.AutoSizeToContents = true;

            CrossSplitter spl = new CrossSplitter(canvas);
            spl.SetBounds(150, 380, 150, 150);
            spl.SplittersVisible = true;

            Button btl = new Button(spl);
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

            rbc2 = new RadioButtonController(tabbtn1.Page);
            rbc2.SetBounds(10, 10, 100, 100);
            rbc2.AddOption("Top").Select();
            rbc2.AddOption("Bottom");
            rbc2.AddOption("Left");
            rbc2.AddOption("Right");
            rbc2.OnSelectionChange += rbc2_OnSelectionChange;

            tab.AddPage("Red");
            tab.AddPage("Green");
            tab.AddPage("Blue");

            tab.AllowReorder = true;
        }

        private void GlForm_Resize(object sender, EventArgs e)
        {
            canvas.SetSize(glControl.Width, glControl.Height);
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            Gl.glClear(Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_COLOR_BUFFER_BIT);
            canvas.RenderCanvas();
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
            MessageBox.Show("File opened: " + file);
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
            RadioButtonController rc = control as RadioButtonController;

            if (rc.SelectedLabel == "Top") tab.TabStripPosition = Pos.Top;
            if (rc.SelectedLabel == "Bottom") tab.TabStripPosition = Pos.Bottom;
            if (rc.SelectedLabel == "Left") tab.TabStripPosition = Pos.Left;
            if (rc.SelectedLabel == "Right") tab.TabStripPosition = Pos.Right;
        }

    }
}
