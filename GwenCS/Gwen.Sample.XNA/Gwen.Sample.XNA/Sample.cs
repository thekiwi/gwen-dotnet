using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Gwen.Control;

namespace Gwen.Sample.XNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Sample : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        private SpriteBatch batch;

        private Input.XNA gwenInput;
        private Renderer.XNA gwenRenderer;
        private Skin.Base m_Skin;

        private Canvas canvas;
        private UnitTest.UnitTest m_UnitTest;

        private Quad quad;
        private BasicEffect qeffect;

        private RenderTarget2D m_RT2D; // Not quite R2D2

        private Matrix mtxEye, mtxProjection, mtxMVQuad;

        public Sample()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferMultiSampling = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;

            quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, 1.5f, 1.5f);

            mtxProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height, 0.5f, 10f);
            mtxEye = Matrix.CreateLookAt(new Vector3(0f, 0f, 3f), Vector3.Zero, Vector3.Up);
            mtxMVQuad = Matrix.Identity;
            mtxMVQuad = Matrix.Multiply(Matrix.CreateTranslation(new Vector3(-1f, 0f, 0f)), mtxMVQuad);
            mtxMVQuad = Matrix.Multiply(Matrix.CreateRotationY(MathHelper.PiOver4), mtxMVQuad);
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //m_RT2D = new RenderTarget2D(this.GraphicsDevice, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            m_RT2D = new RenderTarget2D(this.GraphicsDevice, 512, 512);

            gwenRenderer = new Renderer.XNA(m_RT2D, this.GraphicsDevice);
            gwenRenderer.Content = Content;

            m_Skin = new Skin.TexturedBase(gwenRenderer, "DefaultSkin");
            m_Skin.SetDefaultFont("Arial");

            canvas = new Canvas(m_Skin);
            canvas.SetSize(m_RT2D.Width, m_RT2D.Height);
            canvas.ShouldDrawBackground = false;
            canvas.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            canvas.KeyboardInputEnabled = true;

            m_UnitTest = new UnitTest.UnitTest(canvas);

            gwenInput = new Input.XNA();
            gwenInput.Initialize(canvas);

            gwenRenderer.LoadTextures(this.Content);
            gwenRenderer.LoadFonts(this.Content);

            batch = new SpriteBatch(this.GraphicsDevice);

            qeffect = new BasicEffect(this.GraphicsDevice);
            qeffect.World = mtxMVQuad;
            qeffect.View = mtxEye;
            qeffect.Projection = mtxProjection;
            qeffect.TextureEnabled = true;
            qeffect.Texture = m_RT2D;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            gwenRenderer.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
                return;
            }

            KeyboardState kbState = Keyboard.GetState();
            MouseState mState = Mouse.GetState();

            //gwenInput.ProcessKeyPress(kbState);
            //gwenInput.ProcessMouseClick(mState);
            //gwenInput.ProcessMouseMove(mState);

            m_UnitTest.Fps = 1d / gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            canvas.RenderCanvas();

            // 3D quad
            qeffect.CurrentTechnique.Passes[0].Apply();
            quad.Render(this.GraphicsDevice);

            // Basic fullscreen 2D
            /*batch.Begin();
            batch.Draw(m_RT2D, Vector2.Zero, Color.White);
            batch.End();*/

            base.Draw(gameTime);
        }
    }
}
