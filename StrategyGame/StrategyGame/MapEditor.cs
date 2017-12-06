﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StrategyGame
{
    public static class MapEditor
    {
        static readonly int numOfPauseMenuButtons = 4;
        static readonly int numOfTiles = 3;
        static Button[] pauseMenuButtons = new Button[numOfPauseMenuButtons];
        static Button[] tileButtons = new Button[numOfTiles];
        static Point pauseMenuPosition = new Point(Game.WindowWidth/2-80, 100);
        static Point tileSelectorPosition = new Point(0, 640);
        static Point saveTextInputPosition = new Point(640, 64);
        //static int pauseMenuButtonYOffset = 100;
        static string selectedTile = "Grass";
        static int selectedTileIndex = 0;
        static Rectangle selectorRectangle = new Rectangle(tileSelectorPosition, new Point(Game.GameScale * Game.TileSize));
        static Rectangle mapArea = new Rectangle(0, 0, Map.Width * Game.GameScale * Game.TileSize, Map.Height * Game.GameScale * Game.TileSize);
        static bool isSaving = false;
        static bool isPaused = false;
        public static bool IsSaving
        {
            set { isSaving = value; }
        }
        static bool isLoading = false;
        public static bool IsLoading
        {
            set { isLoading = value; }
        }

        static MapEditor()
        {
            //pauseMenuButtons[0] = new ButtonEnterMainMenu();
            //pauseMenuButtons[1] = new ButtonMapEditorSaveMap();
            //pauseMenuButtons[2] = new ButtonMapEditorLoadMap();
            //pauseMenuButtons[3] = new ButtonQuit();

            for (int i = 0; i < numOfPauseMenuButtons; i++)
            {
                //pauseMenuButtons[i].Initialize(new Point(pauseMenuPosition.X, pauseMenuPosition.Y + i * pauseMenuButtonYOffset), Art.Textures["Button"]);
            }

            for (int i = 0; i < numOfTiles;i++)
            {

                //tileButtons[i] = new ButtonMapEditorSelectTile(
                //    new Point(tileSelectorPosition.X + (i / 2) * Game.TileSize * Game.GameScale, tileSelectorPosition.Y + (i % 2) * Game.TileSize * Game.GameScale), Art.TilePaths[i],i);
            }
        }

        public static void Update()
        {
            if (Input.KeyboardState.IsKeyHit(Keys.Escape))
            {
                if (!isSaving && !isLoading)
                    isPaused = !isPaused;
                else
                {
                    isLoading = false;
                    isSaving = false;
                    KeyboardExtension.StopReadingInput();
                }
            }

            if (isPaused)
            {
                if (isSaving)
                {
                    if (KeyboardExtension.IsKeyHit(Keys.Enter))
                    {
                        KeyboardExtension.StopReadingInput();
                        Map.SaveMap(KeyboardExtension.CurrentInput);
                        isSaving = false;
                    }
                }
                else if(isLoading)
                {
                    if (KeyboardExtension.IsKeyHit(Keys.Enter))
                    {
                        KeyboardExtension.StopReadingInput();
                        Map.LoadMap(KeyboardExtension.CurrentInput);
                        isLoading = false;
                    }
                }
                else
                {
                    //foreach (Button b in pauseMenuButtons)
                    //    b.Update();
                }
            }
            else
            {
                if (MouseExtension.Left == ClickState.Held && MouseExtension.Rectangle.Intersects(mapArea))
                    PaintTile();

                //foreach (Button b in tileButtons)
                //    b.Update();
            }

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch);

            foreach (Button b in tileButtons)
                b.Draw(spriteBatch);
            spriteBatch.Draw(Art.Textures["Selector"], selectorRectangle, Color.White);


            if (isPaused)
            {
                spriteBatch.Draw(Art.Textures["Fade"], Game.FadeRectangle, Color.White);
                foreach (Button b in pauseMenuButtons)
                    b.Draw(spriteBatch);
                if (KeyboardExtension.IsReadingInput)
                {
                    spriteBatch.Draw(Art.Textures["Fade"], Game.FadeRectangle, Color.White);
                    spriteBatch.DrawString(Art.SpriteFont, KeyboardExtension.CurrentInput,
                    Art.CenterString(new Rectangle(saveTextInputPosition, new Point(0)), Art.SpriteFont, KeyboardExtension.CurrentInput), Color.White);
                }
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
            Map.ChangeTile(x, y, selectedTile,selectedTileIndex);
        }
    }
}