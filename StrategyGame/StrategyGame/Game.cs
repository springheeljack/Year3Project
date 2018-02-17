using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;

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
        //public static Screen Screen;

        public static Entity SelectedEntity = null;
        public static float SelectedLayerDepth = 0.97f;
        //public static int Resources = 1000;

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
            Map.Initialize();
            Map.LoadMap("ISLAND");
            UnitBase.Initialize();
            BuildingBase.Initialize();
            ResourceNodeBase.Initialize();

            //ButtonBase.Initialize();
            //TextBase.Initialize();
            //SelectorListBase.Initialize();
            //ScreenBase.Initialize();
            //UnitBase.Initialize();
            //BuildingBase.Initialize();
            //ResourceNodeBase.Initialize();
            //Recipe.Initialize();

            EntityManager.ToAdd.Add(new Building(BuildingBase.Bases["Stockpile"], new Vector2(200)));
            EntityManager.ToAdd.Add(new Building(BuildingBase.Bases["Forge"], new Vector2(200, 400)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Tree"], new Vector2(300)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Tree"], new Vector2(350)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Tree"], new Vector2(400)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Sticks"], new Vector2(300,200)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Sticks"], new Vector2(200,300)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Sticks"], new Vector2(100, 500)));
            EntityManager.ToAdd.Add(new Unit(UnitBase.Bases["Woodcutter"], new Vector2(100)));
            EntityManager.ToAdd.Add(new Unit(UnitBase.Bases["Blacksmith"], new Vector2(100, 200)));

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //Globals
            Global.gameTime = gameTime;

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

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            if (Map.Loaded)
                Map.Draw(spriteBatch);

            EntityManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}