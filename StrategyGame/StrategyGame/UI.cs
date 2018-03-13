using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        static bool HelpScreen = false;

        public static void Update()
        {
            if (Input.IsKeyHit(Keys.H))
            {
                HelpScreen = !HelpScreen;
                Global.selected = null;
            }
            if (!HelpScreen)
            {
                if (Input.IsLeftClicked())
                {
                    Global.selected = null;
                    foreach (Entity e in EntityManager.Entities)
                        if (Input.MouseRectangle.Intersects(e.Rectangle))
                            Global.selected = e;
                }
                if (Input.IsRightClicked())
                {
                    EntityManager.ToAdd.Add(new Enemy(Input.MouseState.Position.ToVector2()));
                }
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
                else if (Global.selected is Enemy)
                    type += "Enemy";
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
                        if (count == 0)
                            spriteBatch.Draw(Art.Textures["Pixel"], new Rectangle(new Point(970,320),Art.SmallFont.MeasureString(t.String).ToPoint()), Color.Yellow);
                        spriteBatch.DrawDoubleString(Art.SmallFont, t.String, new Vector2(970, 320 + (count * 20)), t.Color);
                        count++;
                    }
                }
            }
            if (HelpScreen)
            {
                Texture2D texture = Art.Textures["Help Screen"];
                spriteBatch.Draw(texture, new Rectangle(Game.WindowWidth / 2 - texture.Width/2-20, Game.WindowHeight / 2 - texture.Height/2, texture.Width, texture.Height), Color.White);
                spriteBatch.DrawDoubleString(Art.LargeFont, "Press H to close help screen", new Vector2(620, 560), Color.Red);
                spriteBatch.DrawDoubleString(Art.SmallFont, "Click on an entity to view information about it.", new Vector2(320, 140), Color.Black);
                spriteBatch.DrawDoubleString(Art.SmallFont, "If the entity is a GOAP agent, a list of its thoughts will be displayed.", new Vector2(320, 170), Color.Black);
            }
            else
            {
                spriteBatch.DrawDoubleString(Art.LargeFont, "Press H to view help screen", new Vector2(10, 680), Color.Red);
            }
        }
    }
}