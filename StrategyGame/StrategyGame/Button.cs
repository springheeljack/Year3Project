using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public abstract class Button
    {
        public Rectangle rectangle;
        public Texture2D texture;
        public string text;

        public Button(Point position, Texture2D texture, string text)
        {
            this.texture = texture;
            rectangle = new Rectangle(position, texture.Bounds.Size);
            this.text = text;
        }
        public Button(string text)
        {
            this.text = text;
        }

        public abstract void Action();

        public void Update()
        {
            if (MouseExtension.Left == ClickState.Clicked)
                if (MouseExtension.Rectangle.Intersects(rectangle))
                    Action();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
            spriteBatch.DrawString(TextureManager.SpriteFont, text, TextureManager.CenterString(rectangle, TextureManager.SpriteFont, text), Color.Black);
        }

        public void Initialize(Point position, Texture2D texture)
        {
            this.texture = texture;
            rectangle = new Rectangle(position, texture.Bounds.Size);
        }
    }

    public class ButtonQuit : Button
    {
        new static string text = "Quit";
        public ButtonQuit(Point position, Texture2D texture) : base(position, texture, text)
        {
        }
        public ButtonQuit() :base(text)
        {
        }

        public override void Action()
        {
            Game.Quit = true;
        }
    }

    public class ButtonEnterMapEditor : Button
    {
        new static string text = "Map Editor";
        public ButtonEnterMapEditor(Point position, Texture2D texture) : base(position, texture, text)
        {
        }
        public ButtonEnterMapEditor() : base(text)
        {
        }

        public override void Action()
        {
            Game.ChangeScreen(Screen.MapEditor);
        }
    }

    public class ButtonEnterMainMenu : Button
    {
        new static string text = "Main Menu";
        public ButtonEnterMainMenu(Point position, Texture2D texture) : base(position, texture, text)
        {
        }
        public ButtonEnterMainMenu() : base(text)
        {
        }

        public override void Action()
        {
            Game.ChangeScreen(Screen.MainMenu);
        }
    }
}
