using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StrategyGame
{
    public static class Input
    {
        public static KeyboardState KeyboardState, LastKeyboardState;
        public static MouseState MouseState, LastMouseState;
        public static Rectangle MouseRectangle = new Rectangle(0, 0, 1, 1);

        public static void Update()
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            LastMouseState = MouseState;
            MouseState = Mouse.GetState();

            MouseRectangle.Location = MouseState.Position;           
        }

        public static bool IsKeyHit(this KeyboardState keyboardState, Keys key)
        {
            return KeyboardState.IsKeyDown(key) && KeyboardState.IsKeyUp(key);
        }

        public static bool IsLeftClicked()
        {
            return MouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton == ButtonState.Released;
        }
        public static bool IsRightClicked()
        {
            return MouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton == ButtonState.Released;
        }
    }
}