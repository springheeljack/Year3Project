using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class SelectorList
    {
        public int SelectedIndex { get; set; } = -1;
        List<string> List;
        public Point Position { get; }
        int Spacing = 48;
        public string SelectedString { get { return List[SelectedIndex]; } }

        public SelectorList(List<string> List, Point Position)
        {
            this.List = List;
            this.Position = Position;
        }

        public void Update()
        {
            for (int i = 0; i < List.Count; i++)
            {
                Point measuredString = Art.SpriteFont.MeasureString(List[i]).ToPoint();
                Rectangle textRectangle = new Rectangle(Position.X, Position.Y + (i * Spacing), measuredString.X, measuredString.Y);
                if (MouseExtension.Left == ClickState.Clicked && MouseExtension.Rectangle.Intersects(textRectangle))
                {
                    SelectedIndex = i;
                    break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 pos = Position.ToVector2();

            for (int i = 0; i < List.Count;i++)
            {
                Color color = i == SelectedIndex ? Color.LightGray : Color.Black;
                spriteBatch.DrawString(Art.SpriteFont, List[i], pos, color);
                pos.Y += Spacing;
            }
        }
    }
}