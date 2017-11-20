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
        public static SelectorList MapList { get; }
        public static PlayScreen Screen { get; set; } = PlayScreen.MapList;

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
                    break;
            }
        }

        public static void ChangeScreen(PlayScreen screen)
        {
            Screen = screen;
        }
    }
}