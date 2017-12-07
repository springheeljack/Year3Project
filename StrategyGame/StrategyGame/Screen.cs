using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{

    public class ScreenBase : EntityBase
    {
        new static readonly float LayerDepth = 0.0f;
        public static Dictionary<string, ScreenBase> Dictionary = new Dictionary<string, ScreenBase>();
        //public List<Button> Buttons = new List<Button>();
        //public List<Text> Text = new List<Text>();
        public List<Entity> ScreenEntities = new List<Entity>();
        public static void Initialize()
        {
            //Main Menu
            List<Entity> list = new List<Entity>
            {
                new Text(TextBase.Dictionary["Standard"],new Vector2(640,50),"Strategy Game"),
                new Button(ButtonBase.Bases["Standard"], new Vector2(100, 100), "GotoPlayMenu", "Play"),
                new Button(ButtonBase.Bases["Standard"], new Vector2(100, 200), "GotoMapEditorMenu", "Map Editor"),
                new Button(ButtonBase.Bases["Standard"], new Vector2(100, 300), "Quit", "Quit")
            };
            Dictionary.Add("Main Menu", new ScreenBase(list, "Screen"));

            //Play Menu
            list = new List<Entity>
            {
                new Text(TextBase.Dictionary["Standard"],new Vector2(640,50),"Play Game"),
                new Button(ButtonBase.Bases["Standard"], new Vector2(100, 100), "GotoPlayMenuLoadMap", "Load Map"),
                new Button(ButtonBase.Bases["Standard"], new Vector2(100, 200), "GotoMainMenu", "Main Menu")
            };
            Dictionary.Add("Play Menu", new ScreenBase(list, "Screen"));

            //Play Menu Load Map
            list = new List<Entity>
            {
                new SelectorList(SelectorListBase.Dictionary["Standard"],new Vector2(200,100),Map.MapList),
                new Text(TextBase.Dictionary["Standard"],new Vector2(640,50),"Load Map"),
                new Button(ButtonBase.Bases["Standard"],new Vector2(100,100),"PlayMenuLoadMap", "Load"),
                new Button(ButtonBase.Bases["Standard"],new Vector2(100,200),"GotoPlayMenu", "Back")
            };
            Dictionary.Add("Play Menu Load Map", new ScreenBase(list, "Screen"));

            //Map Editor Menu
            list = new List<Entity>
            {
                new Text(TextBase.Dictionary["Standard"],new Vector2(640,50),"Map Editor"),
                new Button(ButtonBase.Bases["Standard"], new Vector2(100, 100), "GotoMapEditorNewMap", "New Map"),
                //new Button(ButtonBase.Bases["Standard"], new Vector2(100, 200), "LoadMap", "Load Map"),
                new Button(ButtonBase.Bases["Standard"], new Vector2(100, 300), "GotoMainMenu", "Main Menu")
            };
            Dictionary.Add("Map Editor Menu", new ScreenBase(list, "Screen"));

            //Map Editor New Map
            list = new List<Entity>
            {
                new Text(TextBase.Dictionary["Standard"],new Vector2(640,50),"New Map"),
                //new Button(ButtonBase.Bases["Standard"], new Vector2(100, 200), "LoadMap", "Load Map"),
                new Button(ButtonBase.Bases["Standard"], new Vector2(100, 300), "GotoMapEditorMenu", "Back")
            };
            Dictionary.Add("Map Editor New Map", new ScreenBase(list, "Screen"));

            //Play
            list = new List<Entity>
            {

            };
            Dictionary.Add("Play", new ScreenBase(list, "Screen"));
        }
        public ScreenBase(List<Entity> ScreenEntities, string Name) : base(typeof(Screen), Name, new Point(1280, 720), false, LayerDepth)
        {
            this.ScreenEntities = ScreenEntities;
        }
    }
    public class Screen : Entity
    {
        List<Entity> ScreenEntities = new List<Entity>();
        public Screen(EntityBase Base) : base(Base, new Vector2(640, 360))
        {
            EntityManager.ToAdd.Add(this);
            ScreenEntities.AddRange((Base as ScreenBase).ScreenEntities);
            EntityManager.ToAdd.AddRange(ScreenEntities);
        }
        new public void Remove()
        {
            foreach (Entity e in ScreenEntities)
                e.Remove();
            base.Remove();
        }
    }
}