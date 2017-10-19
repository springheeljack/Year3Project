using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public enum ButtonType
    {
        QuitGame,
        OpenMapEditor
    }

    public class Button
    {
        Rectangle rectangle;
        Texture2D texture;

        ButtonType buttonType;
        public Button(ButtonType ButtonType)
        {
            buttonType = ButtonType;
        }

        public void Action()
        {
            switch(buttonType)
            {
                case ButtonType.QuitGame:
                    Game.Quit = true;
                    break;
                case ButtonType.OpenMapEditor:
                    Game.ChangeScreen(Screen.MapEditor);
                    break;
            }
        }
    }
}
