using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StrategyGame
{
    //public enum PlayScreen
    //{
    //    MapList,
    //    Game
    //}

    //public static class Play
    //{
    //    static List<Button> PlayButtons = new List<Button>();
    //    public static SelectorList MapList { get; }
    //    public static PlayScreen Screen { get; set; } = PlayScreen.MapList;
    //    static Entity Selected = null;
    //    public static int Resources = 1000;
    //    static List<Entity> Entities = EntityManager.Entities;

    //    static Play()
    //    {
    //        FileInfo directory = new FileInfo("Content/Map/");
    //        FileInfo[] files = directory.Directory.GetFiles("*.sgmap");
    //        List<string> fileNames = new List<string>();
    //        foreach (FileInfo fi in files)
    //            fileNames.Add(Path.GetFileNameWithoutExtension(fi.Name));
    //        //MapList = new SelectorList(fileNames, new Point(100, 100));

    //        //PlayButtons.Add(new ButtonPlayLoadMap(new Point(600, 100)));
    //        //PlayButtons.Add(new ButtonEnterMainMenu(new Point(600, 200)));

    //        Entities.Add(new BuildingTownCenter(BuildingBase.Dictionary["Town Center"], new Vector2(128, 128)));

    //        Entities.Add(new UnitGatherer(UnitBaseGatherer.Dictionary["Miner"], new Vector2(70, 100)));
    //        Entities.Add(new UnitGatherer(UnitBaseGatherer.Dictionary["Miner"], new Vector2(100, 100)));
    //        Entities.Add(new UnitGatherer(UnitBaseGatherer.Dictionary["Miner"], new Vector2(130, 100)));

    //        Entities.Add(new ResourceNode(ResourceNodeBase.Dictionary["Iron Rock"], new Vector2(400)));
    //        Entities.Add(new ResourceNode(ResourceNodeBase.Dictionary["Iron Rock"], new Vector2(432)));
    //    }

    //    public static void Update(GameTime gameTime)
    //    {
    //        switch (Screen)
    //        {
    //            case PlayScreen.MapList:
    //                MapList.Update(gameTime);
    //                //foreach (Button b in PlayButtons)
    //                //    b.Update();
    //                break;

    //            case PlayScreen.Game:
                    

    //                //Mouse
    //                if (MouseExtension.Left == ClickState.Clicked)
    //                {
    //                    foreach (Entity e in Entities.Where(n => n.Base.Selectable))
    //                        if (MouseExtension.Rectangle.Intersects(e.Rectangle))
    //                            Selected = e;
    //                    if (Selected is IHasRecipe)
    //                    {
    //                        IHasRecipe ihr = Selected as IHasRecipe;
    //                        for (int i = 0; i < ihr.Recipes.Count; i++)
    //                        {
    //                            Rectangle rect = new Rectangle(new Point(500 + (i / 2) * 40, 645 + (i % 2) * 40), ihr.Recipes[i].Output.Size);
    //                            if (MouseExtension.Rectangle.Intersects(rect))
    //                                if (Resources >= ihr.Recipes[i].Cost)
    //                                    ihr.Recipes[i].Complete(Selected.Rectangle.Center.ToVector2());
    //                        }
    //                    }
    //                }
    //                if (MouseExtension.Right == ClickState.Clicked)
    //                {
    //                    if (Selected != null)
    //                    {
    //                        if (Selected is Unit)
    //                        {
    //                            if (Selected is IAttacker)
    //                            {
    //                                IAttacker ia = Selected as IAttacker;
    //                                ia.AttackTarget = null;
    //                                (ia as Unit).HasDestination = false;
    //                                foreach (IHealth ih in Entities.OfType<IHealth>())
    //                                    if (MouseExtension.Rectangle.Intersects(ih.Rectangle))
    //                                        ia.AttackTarget = ih;
    //                                if (ia.AttackTarget == ia)
    //                                    ia.AttackTarget = null;
    //                            }
    //                            if (!(Selected is IAttacker)
    //                                || (Selected is IAttacker) && (Selected as IAttacker).AttackTarget == null)
    //                            {
    //                                Unit u = Selected as Unit;
    //                                u.SetDestination(Mouse.GetState().Position.ToVector2());
    //                            }
    //                            if (Selected is IGatherer)
    //                            {
    //                                var v = Selected as IGatherer;
    //                                if (v.CarriedResources < v.Base.MaxCapacity)
    //                                {
    //                                    foreach (ResourceNode r in Entities.OfType<ResourceNode>())
    //                                        if (MouseExtension.Rectangle.Intersects(r.Rectangle))
    //                                        {
    //                                            v.GatherTarget = r;
    //                                            (v as Unit).HasDestination = false;
    //                                        }
    //                                }
    //                                foreach (Building b in Entities.OfType<Building>().Where(n => (n.Base as BuildingBase).Depositable))
    //                                    if (MouseExtension.Rectangle.Intersects(b.Rectangle))
    //                                    {
    //                                        v.DepositTarget = b;
    //                                        (v as Unit).HasDestination = false;
    //                                    }
    //                                if ((v as Unit).HasDestination == true)
    //                                {
    //                                    (v as IGatherer).DepositTarget = null;
    //                                    (v as IGatherer).GatherTarget = null;
    //                                    (v as IGatherer).CurrentTarget = null;
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                break;
    //        }
    //    }

    //    public static void Draw(SpriteBatch spriteBatch)
    //    {
    //        switch (Screen)
    //        {
    //            case PlayScreen.MapList:
    //                MapList.Draw(spriteBatch);
    //                foreach (Button b in PlayButtons)
    //                    b.Draw(spriteBatch);
    //                break;
    //            case PlayScreen.Game:
    //                Map.Draw(spriteBatch);

    //                if (Selected != null)
    //                {
    //                    spriteBatch.Draw(Selected.Base.Texture, new Vector2(20, 650), Color.White);
    //                    spriteBatch.DrawString(Art.SpriteFont, Selected.Base.Name, new Vector2(100, 640), Color.Black);
    //                    if (Selected is Unit && (Selected as Unit).HasDestination)
    //                        (Selected as Unit).DrawDestinationFlag(spriteBatch);
    //                    if (Selected is IHealth)
    //                    {
    //                        IHealth ih = Selected as IHealth;
    //                        string s = ih.Health.ToString() + " / " + ih.MaxHealth.ToString();
    //                        spriteBatch.DrawString(Art.SpriteFont, s, new Vector2(100, 680), Color.Black);
    //                    }
    //                    if (Selected is IAttacker)
    //                    {
    //                        IAttacker ia = Selected as IAttacker;
    //                        spriteBatch.DrawString(Art.SpriteFont, "Damage: " + ia.Base.AttackDamage, new Vector2(200, 640), Color.Black);
    //                        spriteBatch.DrawString(Art.SpriteFont, "Speed: " + ia.Base.AttackSpeed, new Vector2(200, 680), Color.Black);
    //                        if (ia.AttackTarget != null)
    //                            ia.DrawAttackReticle(spriteBatch);
    //                    }
    //                    if (Selected is IHasRecipe)
    //                    {
    //                        IHasRecipe ihr = Selected as IHasRecipe;
    //                        for (int i = 0; i < ihr.Recipes.Count; i++)
    //                        {
    //                            Rectangle rectangle = new Rectangle(new Point(500 + (i / 2) * 40, 645 + (i % 2) * 40), ihr.Recipes[i].Output.Size);
    //                            spriteBatch.Draw(ihr.Recipes[i].Output.Texture, rectangle, Color.White);
    //                            if (MouseExtension.Rectangle.Intersects(rectangle))
    //                            {
    //                                float nameLength = Art.SpriteFont.MeasureString(ihr.Recipes[i].Output.Name).X;
    //                                float costLength = Art.SpriteFont.MeasureString(ihr.Recipes[i].Cost.ToString()).X;
    //                                float length = nameLength > costLength ? nameLength : costLength;

    //                                Color c = Resources >= ihr.Recipes[i].Cost ? Color.Lime : Color.Red;

    //                                Rectangle tooltipBackground = new Rectangle(MouseExtension.Rectangle.X - (int)length, MouseExtension.Rectangle.Y - 80, (int)length, 80);
    //                                spriteBatch.Draw(Art.Textures["Fade"], tooltipBackground, Color.White);
    //                                spriteBatch.DrawString(Art.SpriteFont, ihr.Recipes[i].Output.Name, tooltipBackground.Location.ToVector2(), c);
    //                                spriteBatch.DrawString(Art.SpriteFont, ihr.Recipes[i].Cost.ToString(), tooltipBackground.Location.ToVector2()+new Vector2(0,40), c);
    //                            }
    //                        }
    //                    }
    //                    if (Selected is ResourceNode)
    //                    {
    //                        var v = Selected as ResourceNode;
    //                        spriteBatch.DrawString(Art.SpriteFont, "Resources left: " + v.Resources.ToString(), new Vector2(500, 640), Color.Black);
    //                    }
    //                    if (Selected is UnitGatherer)
    //                    {
    //                        var v = Selected as UnitGatherer;
    //                        spriteBatch.DrawString(Art.SpriteFont, "Carrying: " + v.CarriedResources.ToString() + " / " + v.Base.MaxCapacity.ToString(), new Vector2(500, 640), Color.Black);
    //                        v.DrawGatherReticles(spriteBatch);
    //                    }
    //                }
    //                spriteBatch.DrawString(Art.SpriteFont, "Resources: " + Resources.ToString(), new Vector2(800, 640), Color.Black);
    //                break;
    //        }
    //    }

    //    public static void ChangeScreen(PlayScreen screen)
    //    {
    //        Screen = screen;
    //    }
    //}
}