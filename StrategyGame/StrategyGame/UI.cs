using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StrategyGame.Extension;
using System.Linq;

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
                    for (int i = 0; i < inv.Items.Count; i++)
                        spriteBatch.DrawString(Art.SmallFont, inv.Items.Keys.ToArray()[i].ToString() +" - " + inv.Items.Values.ToArray()[i].ToString(), new Vector2(970, 140 + (i * 20)), Color.Black);
                }

                if (Global.selected is GOAPAgent)
                {
                    spriteBatch.DrawString(Art.LargeFont, "Thoughts", new Vector2(970, 290), Color.Black);

                    int count = 0;
                    foreach (Text t in (Global.selected as GOAPAgent).GetThoughts())
                    {
                        spriteBatch.DrawDoubleString(Art.SmallFont, t.String, new Vector2(970, 320 + (count * 20)), t.Color);
                        count++;
                    }
                }
            }
        }
    }
}