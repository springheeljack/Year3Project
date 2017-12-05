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
        static List<Button> PlayButtons = new List<Button>();
        public static List<Building> Buildings = new List<Building>();
        static List<Building> BuildingsToRemove = new List<Building>();
        public static List<Unit> Units = new List<Unit>();
        static List<Unit> UnitsToRemove = new List<Unit>();
        public static List<ResourceNode> ResourceNodes = new List<ResourceNode>();
        static List<ResourceNode> ResourceNodesToRemove = new List<ResourceNode>();
        public static SelectorList MapList { get; }
        public static PlayScreen Screen { get; set; } = PlayScreen.MapList;
        static ISelectable Selected = null;
        public static int Resources = 1000;

        static Play()
        {
            FileInfo directory = new FileInfo("Content/Map/");
            FileInfo[] files = directory.Directory.GetFiles("*.sgmap");
            List<string> fileNames = new List<string>();
            foreach (FileInfo fi in files)
                fileNames.Add(Path.GetFileNameWithoutExtension(fi.Name));
            MapList = new SelectorList(fileNames, new Point(100, 100));

            PlayButtons.Add(new ButtonPlayLoadMap(new Point(600, 100)));
            PlayButtons.Add(new ButtonEnterMainMenu(new Point(600, 200)));

            Buildings.Add(new BuildingTownCenter(new Point(3, 4)));

            Units.Add(new UnitGatherer(new Vector2(70, 100), UnitBaseGatherer.Dictionary["Miner"]));
            Units.Add(new UnitGatherer(new Vector2(100, 100), UnitBaseGatherer.Dictionary["Miner"]));
            Units.Add(new UnitGatherer(new Vector2(130, 100), UnitBaseGatherer.Dictionary["Miner"]));

            ResourceNodes.Add(new ResourceNode(new Point(400), ResourceNodeBase.Dictionary["Iron Rock"]));
            ResourceNodes.Add(new ResourceNode(new Point(432), ResourceNodeBase.Dictionary["Iron Rock"]));
        }

        public static void Update(GameTime gameTime)
        {
            switch (Screen)
            {
                case PlayScreen.MapList:
                    MapList.Update();
                    foreach (Button b in PlayButtons)
                        b.Update();
                    break;

                case PlayScreen.Game:
                    //Buildings
                    foreach (Building b in Buildings)
                    {
                        b.Update(gameTime);
                        if (b.Health <= 0)
                            BuildingsToRemove.Add(b);
                    }
                    foreach (Building b in BuildingsToRemove)
                        Buildings.Remove(b);
                    BuildingsToRemove.Clear();

                    //Units
                    foreach (Unit u in Units)
                    {
                        u.Update(gameTime);
                        if (u.Health <= 0)
                            UnitsToRemove.Add(u);
                    }
                    foreach (Unit u in UnitsToRemove)
                        Units.Remove(u);
                    UnitsToRemove.Clear();

                    //Resource Nodes
                    foreach (ResourceNode r in ResourceNodes)
                    {
                        if (!r.Gatherable)
                            ResourceNodesToRemove.Add(r);
                    }
                    foreach (ResourceNode r in ResourceNodesToRemove)
                        ResourceNodes.Remove(r);
                    ResourceNodesToRemove.Clear();


                    //Mouse
                    if (MouseExtension.Left == ClickState.Clicked)
                    {
                        foreach (ISelectable s in Buildings)
                            if (MouseExtension.Rectangle.Intersects(s.Rectangle))
                                Selected = s;
                        foreach (ISelectable s in ResourceNodes)
                            if (MouseExtension.Rectangle.Intersects(s.Rectangle))
                                Selected = s;
                        foreach (ISelectable s in Units)
                            if (MouseExtension.Rectangle.Intersects(s.Rectangle))
                                Selected = s;
                        if (Selected is IHasUnitRecipe)
                        {
                            IHasUnitRecipe isr = Selected as IHasUnitRecipe;
                            var recipes = isr.GetUnitRecipes();
                            for (int i = 0; i < recipes.Count; i++)
                            {
                                Rectangle rect = new Rectangle(new Point(500 + (i / 2) * 40, 645 + (i % 2) * 40), recipes[i].RecipeOutput.Texture.Bounds.Size);
                                rect.Width *= Game.GameScale;
                                rect.Height *= Game.GameScale;
                                if (MouseExtension.Rectangle.Intersects(rect))
                                    if (Resources >= recipes[i].Cost)
                                        recipes[i].Output(Selected.Rectangle.Center.ToVector2());
                            }
                        }
                    }
                    if (MouseExtension.Right == ClickState.Clicked)
                    {
                        if (Selected != null)
                        {
                            if (Selected is Unit)
                            {
                                if (Selected is IAttacker)
                                {
                                    IAttacker ia = Selected as IAttacker;
                                    ia.AttackTarget = null;
                                    (ia as Unit).HasDestination = false;
                                    foreach (IHealth ih in Buildings)
                                        if (MouseExtension.Rectangle.Intersects(ih.Rectangle))
                                            ia.AttackTarget = ih;
                                    foreach (IHealth ih in Units)
                                        if (MouseExtension.Rectangle.Intersects(ih.Rectangle))
                                            ia.AttackTarget = ih;
                                    if (ia.AttackTarget == ia)
                                        ia.AttackTarget = null;
                                }
                                if (!(Selected is IAttacker)
                                    || (Selected is IAttacker) && (Selected as IAttacker).AttackTarget == null)
                                {
                                    Unit u = Selected as Unit;
                                    u.SetDestination(Mouse.GetState().Position.ToVector2());
                                }
                                if (Selected is IGatherer)
                                {
                                    var v = Selected as IGatherer;
                                    //v.GatherTarget = null;
                                    //v.DepositTarget = null;
                                    if (v.CarriedResources < v.Base.MaxCapacity)
                                    {
                                        foreach (ResourceNode r in ResourceNodes)
                                            if (MouseExtension.Rectangle.Intersects(r.Rectangle))
                                            {
                                                v.GatherTarget = r;
                                                (v as Unit).HasDestination = false;
                                            }
                                    }
                                    foreach (IResourceDeposit ird in Buildings)
                                        if (MouseExtension.Rectangle.Intersects(ird.Rectangle))
                                        {
                                            v.DepositTarget = ird;
                                            (v as Unit).HasDestination = false;
                                        }
                                    if ((v as Unit).HasDestination == true)
                                    {
                                        (v as IGatherer).DepositTarget = null;
                                        (v as IGatherer).GatherTarget = null;
                                        (v as IGatherer).CurrentTarget = null;
                                    }
                                }
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
                    foreach (Button b in PlayButtons)
                        b.Draw(spriteBatch);
                    break;
                case PlayScreen.Game:
                    Game.map.Draw(spriteBatch);
                    foreach (Building b in Buildings)
                        b.Draw(spriteBatch);
                    foreach (ResourceNode r in ResourceNodes)
                        r.Draw(spriteBatch);
                    foreach (Unit u in Units)
                        u.Draw(spriteBatch);
                    if (Selected != null)
                    {
                        spriteBatch.Draw(Selected.Texture, new Vector2(20, 650), Color.White);
                        spriteBatch.DrawString(Art.SpriteFont, Selected.Name, new Vector2(100, 640), Color.Black);
                        if (Selected is Unit && (Selected as Unit).HasDestination)
                            (Selected as Unit).DrawDestinationFlag(spriteBatch);
                        if (Selected is IHealth)
                        {
                            IHealth ih = Selected as IHealth;
                            string s = ih.Health.ToString() + " / " + ih.MaxHealth.ToString();
                            spriteBatch.DrawString(Art.SpriteFont, s, new Vector2(100, 680), Color.Black);
                        }
                        if (Selected is IAttacker)
                        {
                            IAttacker ia = Selected as IAttacker;
                            spriteBatch.DrawString(Art.SpriteFont, "Damage: " + ia.Base.AttackDamage, new Vector2(200, 640), Color.Black);
                            spriteBatch.DrawString(Art.SpriteFont, "Speed: " + ia.Base.AttackSpeed, new Vector2(200, 680), Color.Black);
                            if (ia.AttackTarget != null)
                                ia.DrawAttackReticle(spriteBatch);
                        }
                        if (Selected is IHasUnitRecipe)
                        {
                            IHasUnitRecipe isr = Selected as IHasUnitRecipe;
                            var recipes = isr.GetUnitRecipes();
                            for (int i = 0; i < recipes.Count; i++)
                            {
                                Rectangle rectangle = new Rectangle(new Point(500 + (i / 2) * 40, 645 + (i % 2) * 40), recipes[i].RecipeOutput.Texture.Bounds.Size);
                                rectangle.Width *= Game.GameScale;
                                rectangle.Height *= Game.GameScale;
                                spriteBatch.Draw(recipes[i].RecipeOutput.Texture, rectangle, Color.White);
                                if (MouseExtension.Rectangle.Intersects(rectangle))
                                {
                                    float nameLength = Art.SpriteFont.MeasureString(recipes[i].RecipeOutput.Name).X;
                                    float costLength = Art.SpriteFont.MeasureString(recipes[i].Cost.ToString()).X;
                                    float length = nameLength > costLength ? nameLength : costLength;

                                    Color c = Resources >= recipes[i].Cost ? Color.Lime : Color.Red;

                                    Rectangle tooltipBackground = new Rectangle(MouseExtension.Rectangle.X - (int)length, MouseExtension.Rectangle.Y - 80, (int)length, 80);
                                    spriteBatch.Draw(Art.UITextures["Fade"], tooltipBackground, Color.White);
                                    spriteBatch.DrawString(Art.SpriteFont, recipes[i].RecipeOutput.Name, tooltipBackground.Location.ToVector2(), c);
                                    spriteBatch.DrawString(Art.SpriteFont, recipes[i].Cost.ToString(), tooltipBackground.Location.ToVector2()+new Vector2(0,40), c);
                                }
                            }
                        }
                        if (Selected is ResourceNode)
                        {
                            var v = Selected as ResourceNode;
                            spriteBatch.DrawString(Art.SpriteFont, "Resources left: " + v.Resources.ToString(), new Vector2(500, 640), Color.Black);
                        }
                        if (Selected is IGatherer)
                        {
                            var v = Selected as IGatherer;
                            spriteBatch.DrawString(Art.SpriteFont, "Carrying: " + v.CarriedResources.ToString() + " / " + v.Base.MaxCapacity.ToString(), new Vector2(500, 640), Color.Black);
                            if (v.GatherTarget != null)
                                v.DrawGatherReticle(spriteBatch);
                            if (v.DepositTarget != null)
                                v.DrawDepositReticle(spriteBatch);
                        }
                    }
                    spriteBatch.DrawString(Art.SpriteFont, "Resources: " + Resources.ToString(), new Vector2(800, 640), Color.Black);
                    break;
            }
        }

        public static void ChangeScreen(PlayScreen screen)
        {
            Screen = screen;
        }
    }
}