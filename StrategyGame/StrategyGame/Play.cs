using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        static List<Building> buildingsToRemove = new List<Building>();
        static List<Unit> units = new List<Unit>();
        static List<Unit> unitsToRemove = new List<Unit>();
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

            units.Add(new UnitCreep(new Vector2(100, 100)));

            units.Add(new UnitCreep(new Vector2(200, 200)));
        }

        public static void Update(GameTime gameTime)
        {
            switch (Screen)
            {
                case PlayScreen.MapList:
                    MapList.Update();
                    foreach (Button b in playButtons)
                        b.Update();
                    break;

                case PlayScreen.Game:
                    //Buildings
                    foreach (Building b in buildings)
                    {
                        b.Update();
                        if (b.Health <= 0)
                            buildingsToRemove.Add(b);
                    }
                    foreach (Building b in buildingsToRemove)
                        buildings.Remove(b);
                    buildingsToRemove.Clear();

                    //Units
                    foreach (Unit u in units)
                    {
                        u.Update(gameTime);
                        if (u.Health <= 0)
                            unitsToRemove.Add(u);
                    }
                    foreach (Unit u in unitsToRemove)
                        units.Remove(u);
                    unitsToRemove.Clear();


                    //Mouse
                    if (MouseExtension.Left == ClickState.Clicked)
                    {
                        foreach (ISelectable s in buildings)
                            if (MouseExtension.Rectangle.Intersects(s.Rectangle))
                                Selected = s;
                        foreach (ISelectable s in units)
                            if (MouseExtension.Rectangle.Intersects(s.Rectangle))
                                Selected = s;
                    }
                    if (MouseExtension.Right == ClickState.Clicked)
                    {
                        if (Selected != null && Selected is Unit)
                        {
                            if (Selected is IAttacker)
                            {
                                IAttacker ia = Selected as IAttacker;
                                ia.AttackTarget = null;
                                (ia as Unit).HasDestination = false;
                                foreach (IHealth ih in buildings)
                                    if (MouseExtension.Rectangle.Intersects(ih.Rectangle))
                                        ia.AttackTarget = ih;
                                foreach (IHealth ih in units)
                                    if (MouseExtension.Rectangle.Intersects(ih.Rectangle))
                                        ia.AttackTarget = ih;
                                if (ia.AttackTarget == ia)
                                    ia.AttackTarget = null;
                            }
                            if (!(Selected is IAttacker) || (Selected is IAttacker) && (Selected as IAttacker).AttackTarget == null)
                            {
                                Unit u = Selected as Unit;
                                u.SetDestination(Mouse.GetState().Position.ToVector2());
                            }
                        }
                    }
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
                    foreach (Unit u in units)
                        u.Draw(spriteBatch);
                    if (Selected != null)
                    {
                        spriteBatch.Draw(Selected.Texture, new Vector2(20, 650), Color.White);
                        spriteBatch.DrawString(TextureManager.SpriteFont, Selected.Name, new Vector2(100, 640), Color.Black);
                        if (Selected is Unit && (Selected as Unit).HasDestination)
                            (Selected as Unit).DrawDestinationFlag(spriteBatch);
                        if (Selected is IAttacker && (Selected as IAttacker).AttackTarget != null)
                            (Selected as IAttacker).DrawAttackReticle(spriteBatch);
                        if (Selected is IHealth)
                        {
                            IHealth ih = Selected as IHealth;
                            string s = ih.Health.ToString() + " / " + ih.MaxHealth.ToString();
                            spriteBatch.DrawString(TextureManager.SpriteFont, s, new Vector2(100, 680), Color.Black);
                        }
                        if (Selected is IAttacker)
                        {
                            IAttacker ia = Selected as IAttacker;
                            spriteBatch.DrawString(TextureManager.SpriteFont, "Damage: " + ia.AttackDamage, new Vector2(200, 640), Color.Black);
                            spriteBatch.DrawString(TextureManager.SpriteFont, "Speed: " + ia.AttackSpeed, new Vector2(200, 680), Color.Black);
                        }
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