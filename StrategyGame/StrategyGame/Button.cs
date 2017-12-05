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
        public Button(Point position, string text)
        {
            this.text = text;
            texture = Art.UITextures["Button"];
            rectangle = new Rectangle(position, texture.Bounds.Size);
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
            spriteBatch.DrawString(Art.SpriteFont, text, Art.CenterString(rectangle, Art.SpriteFont, text), Color.Black);
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
        public ButtonQuit() : base(text)
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

    public class ButtonEnterPlay : Button
    {
        new static string text = "Play";
        public ButtonEnterPlay(Point position, Texture2D texture) : base(position, texture, text)
        {
        }
        public ButtonEnterPlay() : base(text)
        {
        }

        public override void Action()
        {
            Game.ChangeScreen(Screen.Play);
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
        public ButtonEnterMainMenu(Point position) : base(position, text)
        {
        }

        public override void Action()
        {
            Game.ChangeScreen(Screen.MainMenu);
        }
    }

    public class ButtonMapEditorSelectTile : Button
    {
        int textureIndex;
        string tile;
        public string Tile
        {
            get { return tile; }
            set
            {
                tile = value;
                texture = Art.TileTextures[tile];
            }
        }
        public ButtonMapEditorSelectTile(Point position, string tile, int textureIndex) : base("")
        {
            this.tile = tile;
            this.textureIndex = textureIndex;
            Initialize(position, Art.TileTextures[tile]);
            Resize();
        }
        public override void Action()
        {
            MapEditor.ChangeSelectedTile(tile, rectangle.Location,textureIndex);
        }

        void Resize()
        {
            rectangle.Width *= 2;
            rectangle.Height *= 2;
        }
    }

    public class ButtonMapEditorSaveMap : Button
    {
        new static string text = "Save Map";
        public ButtonMapEditorSaveMap(Point position, Texture2D texture) : base(position, texture, text)
        {
        }
        public ButtonMapEditorSaveMap() : base(text)
        {
        }

        public override void Action()
        {
            MapEditor.IsSaving = true;
            KeyboardExtension.StartReadingInput();
        }
    }

    public class ButtonMapEditorLoadMap : Button
    {
        new static string text = "Load Map";
        public ButtonMapEditorLoadMap(Point position, Texture2D texture) : base(position, texture, text)
        {
        }
        public ButtonMapEditorLoadMap() : base(text)
        {
        }

        public override void Action()
        {
            MapEditor.IsLoading = true;
            KeyboardExtension.StartReadingInput();
        }
    }

    public class ButtonPlayLoadMap : Button
    {
        new static string text = "Load Map";
        public ButtonPlayLoadMap(Point position, Texture2D texture) : base(position, texture, text)
        {
        }
        public ButtonPlayLoadMap() : base(text)
        {
        }
        public ButtonPlayLoadMap(Point position) : base(position, text)
        {
        }

        public override void Action()
        {
            Game.map.LoadMap(Play.MapList.SelectedString);
            Play.ChangeScreen(PlayScreen.Game);
        }
    }
}