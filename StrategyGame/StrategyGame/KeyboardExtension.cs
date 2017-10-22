using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace StrategyGame
{
    public enum KeyState
    {
        Up,
        Hit,
        Held
    }

    public static class KeyboardExtension
    {
        static Dictionary<Keys, KeyState> keyStates = new Dictionary<Keys, KeyState>();
        static Keys[] keys = { Keys.Escape };

        static KeyboardExtension()
        {
            foreach (Keys k in keys)
                keyStates.Add(k, KeyState.Up);
        }

        public static void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            foreach (Keys k in keys)
            {
                if (keyboardState.IsKeyUp(k))
                    keyStates[k] = KeyState.Up;
                else
                {
                    if (keyStates[k] == KeyState.Up)
                        keyStates[k] = KeyState.Hit;
                    else
                        keyStates[k] = KeyState.Held;
                }

            }
        }

        public static bool IsKeyUp(Keys key)
        {
            return keyStates[key] == KeyState.Up;
        }
        public static bool IsKeyHit(Keys key)
        {
            return keyStates[key] == KeyState.Hit;
        }
        public static bool IsKeyHeld(Keys key)
        {
            return keyStates[key] == KeyState.Held || keyStates[key] == KeyState.Hit;
        }
    }
}