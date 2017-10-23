using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        public static ClickState Left
        {
            get { return left; }
        }
        static ClickState right = ClickState.Up;
        public static ClickState Right
        {
            get { return right; }
        }
        static Rectangle rectangle = new Rectangle(new Point(0), new Point(1));
        public static Rectangle Rectangle
        {
            get { return rectangle; }
        }

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
            rectangle.Location = mouseState.Position;
        }
    }
}
