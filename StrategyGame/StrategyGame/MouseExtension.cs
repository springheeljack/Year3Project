using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame
{
    public enum ClickState
    {
        Up,
        Clicked,
        Held
    }

    public static class MouseExtension
    {
        static ClickState left = ClickState.Up;
        static ClickState right = ClickState.Up;

        public static void Update()
        {
            MouseState mouseState = Mouse.GetState();
            //Left
            if (mouseState.LeftButton == ButtonState.Released)
                left = ClickState.Up;
            else if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (left == ClickState.Up)
                    left = ClickState.Clicked;
                else
                    left = ClickState.Held;
            }
            //Right
            if (mouseState.RightButton == ButtonState.Released)
                right = ClickState.Up;
            else if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (right == ClickState.Up)
                    right = ClickState.Clicked;
                else
                    right = ClickState.Held;
            }
        }
    }
}
