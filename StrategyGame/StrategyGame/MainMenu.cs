using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public static class MainMenu
    {
        static readonly int numOfButtons = 2;
        static Button[] buttons = new Button[numOfButtons];
        static Point position = new Point(100, 100);
        static int buttonYOffset = 100;

        static bool initialized = false;
        public static bool Initialized
        {
            get { return initialized; }
        }

        public static void Initialize()
        {
            buttons[0] = new ButtonEnterMapEditor();
            buttons[1] = new ButtonQuit();

            for (int i = 0; i < numOfButtons; i++)
            {
                buttons[i].Initialize(new Point(position.X, position.Y + i * buttonYOffset), TextureManager.UITextures["Button"]);
            }
        }

        public static void Update()
        {
            foreach (Button b in buttons)
                b.Update();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button b in buttons)
                b.Draw(spriteBatch);
        }
    }
}