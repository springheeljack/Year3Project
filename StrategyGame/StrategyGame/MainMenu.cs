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
        static Button[] buttons = new Button[2];

        static MainMenu()
        {
            buttons[0] = new Button(ButtonType.OpenMapEditor);
            buttons[1] = new Button(ButtonType.QuitGame);
        }

        public static void Update()
        {

        }

        public static void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
