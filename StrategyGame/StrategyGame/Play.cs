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

            Buildings.Add(new BuildingStockpile(new Point(3, 3)));
            Buildings.Add(new BuildingTownCenter(new Point(7, 10)));

            Units.Add(new UnitMelee(new Vector2(100), MeleeUnitBase.BaseDict["Creep"]));
            Units.Add(new UnitMelee(new Vector2(200), MeleeUnitBase.BaseDict["Creep"]));

            ResourceNodes.Add(new ResourceNode(new Point(300), ResourceNodeBase.BaseDict["Iron Rock"]));
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


                    //Mouse
                    if (MouseExtension.Left == ClickState.Clicked)
                    {
                        foreach (ISelectable s in Buildings)
                            if (MouseExtension.Rectangle.Intersects(s.Rectangle))
                                Selected = s;
                        foreach (ISelectable s in Units)
                            if (MouseExtension.Rectangle.Intersects(s.Rectangle))
                                Selected = s;
                        foreach (ISelectable s in ResourceNodes)
                            if (MouseExtension.Rectangle.Intersects(s.Rectangle))
                                Selected = s;
                        if (Selected is IHasSpawnRecipe)
                        {
                            IHasSpawnRecipe isr = Selected as IHasSpawnRecipe;
                            var recipes = isr.GetSpawnRecipes();
                            for (int i = 0; i < recipes.Count; i++)
                            {
                                Rectangle rect = new Rectangle(new Point(500 + (i / 2)*32, 640 + (i % 2)*32), recipes[i].RecipeOutput.Texture.Bounds.Size);
                                if (MouseExtension.Rectangle.Intersects(rect))
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
                                if (Selected is IGatherer)
                                {
                                    var v = Selected as IGatherer;
                                    v.GatherTarget = null;
                                    (v as Unit).HasDestination = false;
                                    if (v.CarriedResources < v.UnitBase.MaxCapacity)
                                    {
                                        foreach (ResourceNode r in ResourceNodes)
                                            if (MouseExtension.Rectangle.Intersects(r.Rectangle))
                                                v.GatherTarget = r;
                                    }
                                }
                                if (!(Selected is IAttacker || Selected is IGatherer)
                                    || (Selected is IAttacker) && (Selected as IAttacker).AttackTarget == null
                                    || (Selected is IGatherer) && (Selected as IGatherer).GatherTarget == null)
                                {
                                    Unit u = Selected as Unit;
                                    u.SetDestination(Mouse.GetState().Position.ToVector2());
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
                        spriteBatch.DrawString(TextureManager.SpriteFont, Selected.Name, new Vector2(100, 640), Color.Black);
                        if (Selected is Unit && (Selected as Unit).HasDestination)
                            (Selected as Unit).DrawDestinationFlag(spriteBatch);
                        if (Selected is IHealth)
                        {
                            IHealth ih = Selected as IHealth;
                            string s = ih.Health.ToString() + " / " + ih.MaxHealth.ToString();
                            spriteBatch.DrawString(TextureManager.SpriteFont, s, new Vector2(100, 680), Color.Black);
                        }
                        if (Selected is IAttacker)
                        {
                            IAttacker ia = Selected as IAttacker;
                            spriteBatch.DrawString(TextureManager.SpriteFont, "Damage: " + ia.UnitBase.AttackDamage, new Vector2(200, 640), Color.Black);
                            spriteBatch.DrawString(TextureManager.SpriteFont, "Speed: " + ia.UnitBase.AttackSpeed, new Vector2(200, 680), Color.Black);
                            if (ia.AttackTarget != null)
                                ia.DrawAttackReticle(spriteBatch);
                        }
                        if (Selected is IHasSpawnRecipe)
                        {
                            IHasSpawnRecipe isr = Selected as IHasSpawnRecipe;
                            var recipes = isr.GetSpawnRecipes();
                            for (int i = 0; i < recipes.Count;i++)
                            {
                                spriteBatch.Draw(recipes[i].RecipeOutput.Texture, new Vector2(500 + (i / 2)*32, 640 + (i % 2)*32), Color.White);
                            }
                        }
                        if (Selected is ResourceNode)
                        {
                            var v = Selected as ResourceNode;
                            spriteBatch.DrawString(TextureManager.SpriteFont, "Resources left: " + v.Resources.ToString(), new Vector2(500, 640), Color.Black);
                        }
                        if (Selected is IGatherer)
                        {
                            var v = Selected as IGatherer;
                            spriteBatch.DrawString(TextureManager.SpriteFont, "Carrying: " + v.CarriedResources.ToString() + " / " + v.UnitBase.MaxCapacity.ToString(), new Vector2(500, 640), Color.Black);
                            if (v.GatherTarget != null)
                                v.DrawGatherReticle(spriteBatch);
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