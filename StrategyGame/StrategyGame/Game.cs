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

        static Screen screen = Screen.MainMenu;

        public static bool Quit = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            IsMouseVisible = true;
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
            switch(screen)
            {
                case Screen.MainMenu:
                    MainMenu.Update();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Magenta);

            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,null,null,null,null);

            switch(screen)
            {
                case Screen.MainMenu:
                    MainMenu.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void ChangeScreen(Screen screen)
        {

        }
    }
}
