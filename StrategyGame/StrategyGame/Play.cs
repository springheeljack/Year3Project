using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace StrategyGame
{
    public enum PlayScreen
    {
        MapList,
        Game
    }

    public static class Play
    {
        static List<Button> playButtons = new List<Button>();
        static List<Building> buildings = new List<Building>();
        public static SelectorList MapList { get; }
        public static PlayScreen Screen { get; set; } = PlayScreen.MapList;
        static ISelectable Selected = null;

        static Play()
        {
            FileInfo directory = new FileInfo("Content/Map/");
            FileInfo[] files = directory.Directory.GetFiles("*.sgmap");
            List<string> fileNames = new List<string>();
            foreach (FileInfo fi in files)
                fileNames.Add(Path.GetFileNameWithoutExtension(fi.Name));
            MapList = new SelectorList(fileNames, new Point(100, 100));

            playButtons.Add(new ButtonPlayLoadMap(new Point(600, 100)));
            playButtons.Add(new ButtonEnterMainMenu(new Point(600, 200)));

            buildings.Add(new BuildingStockpile(new Point(3, 3)));
            buildings.Add(new BuildingTownCenter(new Point(7, 10)));
        }

        public static void Update()
        {
            switch (Screen)
            {
                case PlayScreen.MapList:
                    MapList.Update();
                    foreach (Button b in playButtons)
                        b.Update();
                    break;
                case PlayScreen.Game:
                    foreach (Building b in buildings)
                        b.Update();
                    foreach (ISelectable s in buildings)
                        if (MouseExtension.Left == ClickState.Clicked && MouseExtension.Rectangle.Intersects(s.DrawingRectangle))
                            Selected = s;
                    break;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            switch (Screen)
            {
                case PlayScreen.MapList:
                    MapList.Draw(spriteBatch);
                    foreach (Button b in playButtons)
                        b.Draw(spriteBatch);
                    break;
                case PlayScreen.Game:
                    Game.map.Draw(spriteBatch);
                    foreach (Building b in buildings)
                        b.Draw(spriteBatch);
                    if (Selected != null)
                    {
                        spriteBatch.Draw(Selected.Texture, new Vector2(100, 640), Color.White);
                        spriteBatch.DrawString(TextureManager.SpriteFont, Selected.Name, new Vector2(0, 640), Color.Black);
                    }
                    break;
            }
        }

        public static void ChangeScreen(PlayScreen screen)
        {
            Screen = screen;
        }
    }
}