using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StrategyGame
{

}
    //public class ButtonBase : EntityBase
    //{
    //    new static readonly float LayerDepth = 0.9f;
    //    public static Dictionary<string, ButtonBase> Bases = new Dictionary<string, ButtonBase>();
    //    public static Dictionary<string, Action> Actions = new Dictionary<string, Action>();
    //    new static string Name = "Button";
    //    public static void Initialize()
    //    {
    //        //Bases
    //        Bases.Add("Standard", new ButtonBase(new Point(160, 80)));

    //        //Actions
    //        Actions.Add("Quit", Quit);
    //        Actions.Add("GotoPlayMenu", GotoPlayMenu);
    //        Actions.Add("GotoMainMenu", GotoMainMenu);
    //        Actions.Add("GotoPlayMenuLoadMap", GotoPlayMenuLoadMap);
    //        Actions.Add("GotoMapEditorMenu", GotoMapEditorMenu);
    //        Actions.Add("GotoMapEditorNewMap", GotoMapEditorNewMap);
    //        Actions.Add("PlayMenuLoadMap", PlayMenuLoadMap);
    //    }

    //    //Actions
    //    private static void Quit()
    //    {
    //        Game.Quit = true;
    //    }
    //    private static void GotoPlayMenu()
    //    {
    //        Game.Screen.Remove();
    //        Game.Screen = new Screen(ScreenBase.Dictionary["Play Menu"]);
    //    }
    //    private static void GotoPlayMenuLoadMap()
    //    {
    //        Game.Screen.Remove();
    //        Game.Screen = new Screen(ScreenBase.Dictionary["Play Menu Load Map"]);
    //    }
    //    private static void GotoMainMenu()
    //    {
    //        Game.Screen.Remove();
    //        Game.Screen = new Screen(ScreenBase.Dictionary["Main Menu"]);
    //    }
    //    private static void GotoMapEditorMenu()
    //    {
    //        Game.Screen.Remove();
    //        Game.Screen = new Screen(ScreenBase.Dictionary["Map Editor Menu"]);
    //    }
    //    private static void GotoMapEditorNewMap()
    //    {
    //        Game.Screen.Remove();
    //        Game.Screen = new Screen(ScreenBase.Dictionary["Map Editor New Map"]);
    //    }
    //    private static void PlayMenuLoadMap()
    //    {
    //        Map.LoadMap(((Game.Screen.Base as ScreenBase).ScreenEntities.Find(x => x is SelectorList) as SelectorList).GetSelected());
    //        EntityManager.ToAdd.Add(new UnitGatherer(UnitBaseGatherer.Dictionary["Miner"], new Vector2(70, 100)));
    //        EntityManager.ToAdd.Add(new UnitGatherer(UnitBaseGatherer.Dictionary["Miner"], new Vector2(100, 100)));
    //        EntityManager.ToAdd.Add(new UnitGatherer(UnitBaseGatherer.Dictionary["Miner"], new Vector2(130, 100)));
    //        EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Dictionary["Iron Rock"], new Vector2(400)));
    //        EntityManager.ToAdd.Add(new ResourceNode(ResourceNodeBase.Dictionary["Iron Rock"], new Vector2(432)));
    //        Game.Screen.Remove();
    //        //Game.Screen = new Screen(ScreenBase.Dictionary["Map Editor New Map"]);
    //    }

    //    public ButtonBase(Point Size) : base(typeof(Button), Name, Size, false, LayerDepth) { }
    //}

    //public class Button : Entity
    //{
    //    string Text;
    //    public Action Action;
    //    public Button(ButtonBase Base,Vector2 Position,string Action,string Text) : base(Base, Position)
    //    {
    //        this.Action = ButtonBase.Actions[Action];
    //        this.Text = Text;
    //    }
    //    public override void Update(GameTime gameTime)
    //    {
    //        if (Input.MouseState.IsLeftClicked() && Input.MouseRectangle.Intersects(Rectangle))
    //            Action();

    //        base.Update(gameTime);
    //    }

    //    public override void Draw(SpriteBatch spriteBatch)
    //    {
    //        spriteBatch.DrawString(Art.SpriteFont, Text, Art.CenterString(Rectangle, Art.SpriteFont, Text), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
    //        base.Draw(spriteBatch);
    //    }
    //}

    ////public abstract class Button
    ////{
    ////    public Rectangle rectangle;
    ////    public Texture2D texture;
    ////    public string text;
    ////    public Button(Point position, Texture2D texture, string text)
    ////    {
    ////        this.texture = texture;
    ////        rectangle = new Rectangle(position, texture.Bounds.Size);
    ////        this.text = text;
    ////    }
    ////    public Button(string text)
    ////    {
    ////        this.text = text;
    ////    }
    ////    public Button(Point position, string text)
    ////    {
    ////        this.text = text;
    ////        texture = Art.Textures["Button"];
    ////        rectangle = new Rectangle(position, texture.Bounds.Size);
    ////    }
    //    public abstract void Action();
    //    public void Update()
    //    {
    //        if (MouseExtension.Left == ClickState.Clicked)
    //            if (MouseExtension.Rectangle.Intersects(rectangle))
    //                Action();
    //    }
    //    public void Draw(SpriteBatch spriteBatch)
    //    {
    //        spriteBatch.Draw(texture, rectangle, Color.White);
    //        spriteBatch.DrawString(Art.SpriteFont, text, Art.CenterString(rectangle, Art.SpriteFont, text), Color.Black);
    //    }
    //    public void Initialize(Point position, Texture2D texture)
    //    {
    //        this.texture = texture;
    //        rectangle = new Rectangle(position, texture.Bounds.Size);
    //    }
    //}







    //public class ButtonEnterMapEditor : Button
    //{
    //    new static string text = "Map Editor";
    //    public ButtonEnterMapEditor(Point position, Texture2D texture) : base(position, texture, text)
    //    {
    //    }
    //    public ButtonEnterMapEditor() : base(text)
    //    {
    //    }

    //    public override void Action()
    //    {
    //        Game.ChangeScreen(Screen.MapEditor);
    //    }
    //}





    //public class ButtonMapEditorSelectTile : Button
    //{
    //    int textureIndex;
    //    string tile;
    //    public string Tile
    //    {
    //        get { return tile; }
    //        set
    //        {
    //            tile = value;
    //            texture = Art.Textures[tile];
    //        }
    //    }
    //    public ButtonMapEditorSelectTile(Point position, string tile, int textureIndex) : base("")
    //    {
    //        this.tile = tile;
    //        this.textureIndex = textureIndex;
    //        Initialize(position, Art.Textures[tile]);
    //        Resize();
    //    }
    //    public override void Action()
    //    {
    //        MapEditor.ChangeSelectedTile(tile, rectangle.Location,textureIndex);
    //    }

    //    void Resize()
    //    {
    //        rectangle.Width *= 2;
    //        rectangle.Height *= 2;
    //    }
    //}

    //public class ButtonMapEditorSaveMap : Button
    //{
    //    new static string text = "Save Map";
    //    public ButtonMapEditorSaveMap(Point position, Texture2D texture) : base(position, texture, text)
    //    {
    //    }
    //    public ButtonMapEditorSaveMap() : base(text)
    //    {
    //    }

    //    public override void Action()
    //    {
    //        MapEditor.IsSaving = true;
    //        KeyboardExtension.StartReadingInput();
    //    }
    //}

    //public class ButtonMapEditorLoadMap : Button
    //{
    //    new static string text = "Load Map";
    //    public ButtonMapEditorLoadMap(Point position, Texture2D texture) : base(position, texture, text)
    //    {
    //    }
    //    public ButtonMapEditorLoadMap() : base(text)
    //    {
    //    }

    //    public override void Action()
    //    {
    //        MapEditor.IsLoading = true;
    //        KeyboardExtension.StartReadingInput();
    //    }
    //}

    //public class ButtonPlayLoadMap : Button
    //{
    //    new static string text = "Load Map";
    //    public ButtonPlayLoadMap(Point position, Texture2D texture) : base(position, texture, text)
    //    {
    //    }
    //    public ButtonPlayLoadMap() : base(text)
    //    {
    //    }
    //    public ButtonPlayLoadMap(Point position) : base(position, text)
    //    {
    //    }

    //    public override void Action()
    //    {
    //        Game.map.LoadMap(Play.MapList.SelectedString);
    //        Play.ChangeScreen(PlayScreen.Game);
    //    }
    //}
