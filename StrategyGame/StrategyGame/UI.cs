using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyGame.Extension;

namespace StrategyGame
{
    public struct Text
    {
        public string String { get; private set; }
        public Color Color { get; private set; }
        public Text(string String, Color Color)
        {
            this.String = String;
            this.Color = Color;
        }
    }

    public static class UI
    {
        public static void Update()
        {
            if (Input.IsLeftClicked())
            {
                Global.selected = null;
                foreach (Entity e in EntityManager.Entities)
                    if (Input.MouseRectangle.Intersects(e.Rectangle))
                        Global.selected = e;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (Global.selected != null)
            {
                spriteBatch.Draw(Art.Textures["Selector"], Global.selected.Rectangle, Color.White);

                spriteBatch.DrawString(Art.LargeFont, Global.selected.Name, new Vector2(970, 10), Color.Black);

                string type = "Type: ";
                if (Global.selected is Building)
                    type += "Building";
                else if (Global.selected is Unit)
                    type += "Unit";
                else if (Global.selected is ResourceNode)
                    type += "Resource Node";
                spriteBatch.DrawString(Art.SmallFont, type, new Vector2(970, 40), Color.Black);

                Inventory inv = null;
                if (Global.selected is Unit)
                    inv = (Global.selected as Unit).Inventory;
                else if (Global.selected is Building)
                    inv = (Global.selected as Building).Inventory;
                if (inv != null)
                {
                    spriteBatch.DrawString(Art.LargeFont, "Inventory", new Vector2(970, 110), Color.Black);
                    for (int i = 0; i < inv.ItemCount; i++)
                        spriteBatch.DrawString(Art.SmallFont, Inventory.GetItemName(inv.Items[i]), new Vector2(970, 140 + (i * 20)), Color.Black);
                }

                if (Global.selected is GOAPAgent)
                {
                    spriteBatch.DrawString(Art.LargeFont, "Thoughts", new Vector2(970, 290), Color.Black);

                    int count = 0;
                    foreach (Text t in (Global.selected as GOAPAgent).GetThoughts())
                    {
                        //spriteBatch.DrawString(Art.SmallFont, t.String, new Vector2(970, 470 + (count * 20)), t.Color);
                        spriteBatch.DrawDoubleString(Art.SmallFont, t.String, new Vector2(970, 320 + (count * 20)), t.Color);
                        count++;
                    }
                }
            }
        }

        //public static void DrawDoubleString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        //{
        //    spriteBatch.DrawString(spriteFont, text, position, color);
        //}
    }
}