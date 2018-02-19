//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace StrategyGame
//{
//    public class SelectorList
//    {
//        public int SelectedIndex { get; set; } = -1;
//        List<string> List;
//        public Point Position { get; }
//        int Spacing = 48;
//        public string SelectedString { get { return List[SelectedIndex]; } }

//        public SelectorList(List<string> List, Point Position)
//        {
//            this.List = List;
//            this.Position = Position;
//        }

//        public void Update()
//        {
//            for (int i = 0; i < List.Count; i++)
//            {
//                Point measuredString = Art.SpriteFont.MeasureString(List[i]).ToPoint();
//                Rectangle textRectangle = new Rectangle(Position.X, Position.Y + (i * Spacing), measuredString.X, measuredString.Y);
//                if (MouseExtension.Left == ClickState.Clicked && MouseExtension.Rectangle.Intersects(textRectangle))
//                {
//                    SelectedIndex = i;
//                    break;
//                }
//            }
//        }

//        public void Draw(SpriteBatch spriteBatch)
//        {
//            Vector2 pos = Position.ToVector2();

//            for (int i = 0; i < List.Count; i++)
//            {
//                Color color = i == SelectedIndex ? Color.LightGray : Color.Black;
//                spriteBatch.DrawString(Art.SpriteFont, List[i], pos, color);
//                pos.Y += Spacing;
//            }
//        }
//    }

//    public class SelectorList : Entity
//    {
//        static Color Black { get; } = Color.Black;
//        static Color Lime { get; } = Color.Lime;

//        public List<string> Strings { get; }
//        new SelectorListBase Base { get { return base.Base as SelectorListBase; } }
//        int Selected { get; set; } = -1;
//        public string GetSelected() { return Strings[Selected]; }
//        public SelectorList(SelectorListBase Base, Vector2 Position, List<string> Strings) : base(Base, Position)
//        {
//            this.Strings = Strings;
//        }

//        public override void Update(GameTime gameTime)
//        {
//            if (Input.MouseState.IsLeftClicked())
//                for (int i = 0; i < Strings.Count; i++)
//                {
//                    Rectangle rect = new Rectangle((Position + new Vector2(0.0f, Base.Spacing * i)).ToPoint(), Base.SpriteFont.MeasureString(Strings[i]).ToPoint());
//                    if (rect.Intersects(Input.MouseRectangle))
//                        Selected = i;
//                }
//        }

//        public override void Draw(SpriteBatch spriteBatch)
//        {
//            for (int i = 0; i < Strings.Count; i++)
//                spriteBatch.DrawString(Base.SpriteFont, Strings[i], Position + new Vector2(0.0f, Base.Spacing * i), i == Selected ? Lime : Black);
//        }
//    }

//    public class SelectorListBase : EntityBase
//    {
//        public static Dictionary<string, SelectorListBase> Dictionary = new Dictionary<string, SelectorListBase>();
//        public float Spacing { get; }
//        public SpriteFont SpriteFont { get; }
//        public SelectorListBase(SpriteFont SpriteFont, float Spacing) : base(typeof(SelectorList), "Selector List", Point.Zero, false, 0.96f)
//        {
//            this.SpriteFont = SpriteFont;
//            this.Spacing = Spacing;
//        }
//        public static void Initialize()
//        {
//            Dictionary.Add("Standard", new SelectorListBase(Art.SpriteFont, 40.0f));
//        }
//    }
//}