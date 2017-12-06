using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public class Text : Entity
    {
        static Color Black { get; } = Color.Black;

        public string String { get; set; }
        public Text(TextBase Base, Vector2 Position,string String) : base(Base, Position)
        {
            this.String = String;
        }
        new TextBase Base { get { return base.Base as TextBase; } }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Base.SpriteFont, String, Art.CenterString(Position, Base.SpriteFont, String), Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, Base.LayerDepth);
        }
    }

    public class TextBase : EntityBase
    {
        public static Dictionary<string, TextBase> Dictionary = new Dictionary<string, TextBase>();
        public SpriteFont SpriteFont { get; }
        public static void Initialize()
        {
            Dictionary.Add("Standard", new TextBase(Art.SpriteFont));
        }
        public TextBase(SpriteFont SpriteFont) : base(typeof(Text), "Text", Point.Zero, false, 0.95f)
        {
            this.SpriteFont = SpriteFont;
        }
    }
}