using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public static class MapEditor
    {
        static readonly int numOfPauseMenuButtons = 3;
        static readonly int numOfTiles = 3;
        static Button[] pauseMenuButtons = new Button[numOfPauseMenuButtons];
        static Button[] tileButtons = new Button[numOfTiles];
        static Point pauseMenuPosition = new Point(Game.WindowWidth/2-80, 100);
        static Point tileSelectorPosition = new Point(0, 640);
        static int pauseMenuButtonYOffset = 100;
        static string selectedTile = "Grass";
        static int selectedTileIndex = 0;
        static Rectangle selectorRectangle = new Rectangle(tileSelectorPosition, new Point(Game.GameScale * Game.TileSize));
        static Rectangle mapArea = new Rectangle(0, 0, Game.map.Width * Game.GameScale * Game.TileSize, Game.map.Height * Game.GameScale * Game.TileSize);

        static MapEditor()
        {
            pauseMenuButtons[0] = new ButtonEnterMainMenu();
            pauseMenuButtons[1] = new ButtonMapEditorSaveMap();
            pauseMenuButtons[2] = new ButtonQuit();

            for (int i = 0; i < numOfPauseMenuButtons; i++)
            {
                pauseMenuButtons[i].Initialize(new Point(pauseMenuPosition.X, pauseMenuPosition.Y + i * pauseMenuButtonYOffset), TextureManager.UITextures["Button"]);
            }

            for (int i = 0; i < numOfTiles;i++)
            {

                tileButtons[i] = new ButtonMapEditorSelectTile(new Point(tileSelectorPosition.X + (i / 2) * Game.TileSize * Game.GameScale, tileSelectorPosition.Y + (i % 2) * Game.TileSize * Game.GameScale), TextureManager.tilePaths[i],i);
            }
        }

        public static void Update()
        {
            if (MouseExtension.Left == ClickState.Held && MouseExtension.Rectangle.Intersects(mapArea))
                PaintTile();

            foreach (Button b in tileButtons)
                b.Update();

            Game.PauseMenuSwitch();
            if (Game.PauseMenu)
            {
                foreach (Button b in pauseMenuButtons)
                    b.Update();
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Game.map.Draw(spriteBatch);

            foreach (Button b in tileButtons)
                b.Draw(spriteBatch);
            spriteBatch.Draw(TextureManager.UITextures["Selector"], selectorRectangle, Color.White);


            if (Game.PauseMenu)
            {
                spriteBatch.Draw(TextureManager.UITextures["Fade"], Game.FadeRectangle, Color.White);
                foreach (Button b in pauseMenuButtons)
                    b.Draw(spriteBatch);
            }
        }

        public static void ChangeSelectedTile(string tile,Point position,int textureIndex)
        {
            selectedTile = tile;
            selectorRectangle.Location = position;
            selectedTileIndex = textureIndex;
        }

        static void PaintTile()
        {
            int x, y;
            x = MouseExtension.Rectangle.X;
            y = MouseExtension.Rectangle.Y;
            x /= Game.TileSizeScaled;
            y /= Game.TileSizeScaled;
            Game.map.ChangeTile(x, y, selectedTile,selectedTileIndex);
        }
    }
}
