using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace StrategyGame
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static readonly int TileSize = 16;
        public static readonly int GameScale = 2;
        public static readonly int TileSizeScaled = TileSize * GameScale;
        public static readonly int WindowWidth = 1280;
        public static readonly int WindowHeight = 720;
        public static readonly Point WindowPosition = new Point(200);
        public static readonly Rectangle FadeRectangle = new Rectangle(0, 0, WindowWidth, WindowHeight);

        public static bool Quit = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Screen Screen;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Window.Position = WindowPosition;
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Art.LoadContent(Content);

            ButtonBase.Initialize();
            TextBase.Initialize();
            ScreenBase.Initialize();
            UnitBase.Initialize();
            BuildingBase.Initialize();
            ResourceNodeBase.Initialize();
            Recipe.Initialize();

            Screen = new Screen(ScreenBase.Dictionary["Main Menu"]);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //Input
            MouseExtension.Update();
            KeyboardExtension.Update();
            Input.Update();

            //Entities
            EntityManager.Update(gameTime);

            if (Quit)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Magenta);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);


            EntityManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}