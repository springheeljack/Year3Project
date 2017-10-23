using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public static class MapEditor
    {
        static readonly int numOfButtons = 2;
        static Button[] pauseMenuButtons = new Button[numOfButtons];
        static Point position = new Point(100, 100);
        static int buttonYOffset = 100;

        static MapEditor()
        {
            pauseMenuButtons[0] = new ButtonEnterMainMenu();
            pauseMenuButtons[1] = new ButtonQuit();

            for (int i = 0; i < numOfButtons; i++)
            {
                pauseMenuButtons[i].Initialize(new Point(position.X, position.Y + i * buttonYOffset), TextureManager.UITextures["Button"]);
            }
        }

        public static void Update()
        {
            Game.PauseMenuSwitch();
            if (Game.PauseMenu)
            {
                foreach (Button b in pauseMenuButtons)
                    b.Update();
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Game.map.Draw(spriteBatch);
            if (Game.PauseMenu)
            {
                spriteBatch.Draw(TextureManager.UITextures["Fade"], Game.FadeRectangle, Color.White);
                foreach (Button b in pauseMenuButtons)
                    b.Draw(spriteBatch);
            }
        }
    }
}
