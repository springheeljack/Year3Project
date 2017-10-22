using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StrategyGame
{
    public enum Screen
    {
        MainMenu,
        MapEditor
    }

    public class Game : Microsoft.Xna.Framework.Game
    {
        public static readonly int TileSize = 16;
        public static readonly int GameScale = 2;
        public static readonly int WindowWidth = 1280;
        public static readonly int WindowHeight = 720;
        public static readonly Point WindowPosition = new Point(200);

        static Screen screen = Screen.MainMenu;

        public static bool Quit = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Map map = new Map();

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

            TextureManager.LoadContent(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (!MainMenu.Initialized)
                MainMenu.Initialize();

            //Input
            MouseExtension.Update();

            switch (screen)
            {
                case Screen.MainMenu:
                    MainMenu.Update();
                    break;
            }

            if (Quit)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Magenta);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            switch (screen)
            {
                case Screen.MainMenu:
                    MainMenu.Draw(spriteBatch);
                    break;
                case Screen.MapEditor:
                    map.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void ChangeScreen(Screen newScreen)
        {
            switch (newScreen)
            {
                case Screen.MapEditor:
                    screen = newScreen;
                    map.LoadMap("test");
                    break;
            }
        }
    }
}