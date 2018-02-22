using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StrategyGame.Extension;

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
        public static readonly Rectangle ScreenRectangle = new Rectangle(0, 0, WindowWidth, WindowHeight);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Entity SelectedEntity = null;

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
            Recipe.Initialize();
            Gather.Initialize();
            UnitBase.Initialize();
            BuildingBase.Initialize();
            ResourceNodeBase.Initialize();

            EntityManager.ToAdd.Add(new Building(BuildingBase.Bases["Stockpile"], new Vector2(400,200)));
            EntityManager.ToAdd.Add(new Building(BuildingBase.Bases["Stockpile"], new Vector2(700, 300)));
            EntityManager.ToAdd.Add(new Building(BuildingBase.Bases["Forge"], new Vector2(200, 400)));
            EntityManager.ToAdd.Add(new Building(BuildingBase.Bases["Smelter"], new Vector2(400, 400)));
            EntityManager.ToAdd.Add(new Building(BuildingBase.Bases["Windmill"], new Vector2(600, 400)));
            EntityManager.ToAdd.Add(new Building(BuildingBase.Bases["Bakery"], new Vector2(800, 200)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Tree"], new Vector2(300)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Tree"], new Vector2(350)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Sticks"], new Vector2(300, 200)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Sticks"], new Vector2(200, 300)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Sticks"], new Vector2(100, 500)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Iron Rock"], new Vector2(200, 100)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Coal Rock"], new Vector2(100, 200)));
            EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Bases["Farm"], new Vector2(600, 200)));


            EntityManager.ToAdd.Add(new Miner(new Vector2(100)));
            EntityManager.ToAdd.Add(new Woodcutter(new Vector2(100, 200)));
            EntityManager.ToAdd.Add(new Blacksmith(new Vector2(100, 300)));
            EntityManager.ToAdd.Add(new Farmer(new Vector2(500, 250)));
            EntityManager.ToAdd.Add(new Miller(new Vector2(500, 400)));
            EntityManager.ToAdd.Add(new Baker(new Vector2(700, 400)));
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            Global.gameTime = gameTime;

            Input.Update();
            EntityManager.Update();
            UI.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Magenta);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            if (Map.Loaded)
            {
                spriteBatch.Draw(Art.Textures["Background"], ScreenRectangle, Color.White);
                Map.Draw(spriteBatch);
            }

            EntityManager.Draw(spriteBatch);

            UI.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}