//using System.Collections.Generic;
//using Microsoft.Xna.Framework.Input;

//namespace StrategyGame
//{
//    public enum KeyState
//    {
//        Up,
//        Hit,
//        Held
//    }

//    public static class KeyboardExtension
//    {
//        static Dictionary<Keys, KeyState> keyStates = new Dictionary<Keys, KeyState>();
//        static Dictionary<Keys, KeyState> textInputKeyStates = new Dictionary<Keys, KeyState>();
//        static Keys[] keys = { Keys.Escape, Keys.Enter, Keys.Back };
//        static List<Keys> textInputKeys = new List<Keys>();
//        static bool isReadingInput = false;
//        static string currentInput = "";

//        static KeyboardExtension()
//        {
//            foreach (Keys k in keys)
//                keyStates.Add(k, KeyState.Up);
//            for (int i = 65; i < 91; i++)
//                textInputKeys.Add((Keys)i);
//            foreach (Keys k in textInputKeys)
//                textInputKeyStates.Add(k, KeyState.Up);
//        }

//        public static void Update()
//        {
//            KeyboardState keyboardState = Keyboard.GetState();
//            foreach (Keys k in keys)
//            {
//                if (keyboardState.IsKeyUp(k))
//                    keyStates[k] = KeyState.Up;
//                else
//                {
//                    if (keyStates[k] == KeyState.Up)
//                        keyStates[k] = KeyState.Hit;
//                    else
//                        keyStates[k] = KeyState.Held;
//                }
//            }
//            foreach (Keys k in textInputKeys)
//            {
//                if (keyboardState.IsKeyUp(k))
//                    textInputKeyStates[k] = KeyState.Up;
//                else
//                {
//                    if (textInputKeyStates[k] == KeyState.Up)
//                        textInputKeyStates[k] = KeyState.Hit;
//                    else
//                        textInputKeyStates[k] = KeyState.Held;
//                }
//            }

//            if (isReadingInput)
//            {
//                foreach (Keys k in textInputKeys)
//                {
//                    if (textInputKeyStates[k] == KeyState.Hit)
//                        currentInput += (char)k;
//                }
//                if (keyStates[Keys.Back] == KeyState.Hit && currentInput.Length > 0)
//                {
//                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
//                }
//            }
//        }

//        public static bool IsReadingInput
//        {
//            get { return isReadingInput; }
//        }

//        public static void StartReadingInput()
//        {
//            currentInput = "";
//            isReadingInput = true;
//        }

//        public static void StopReadingInput()
//        {
//            isReadingInput = false;
//        }

//        public static string CurrentInput
//        {
//            get { return currentInput; }
//        }

//        public static bool IsKeyUp(Keys key)
//        {
//            return keyStates[key] == KeyState.Up;
//        }
//        public static bool IsKeyHit(Keys key)
//        {
//            return keyStates[key] == KeyState.Hit;
//        }
//        public static bool IsKeyHeld(Keys key)
//        {
//            return keyStates[key] == KeyState.Held || keyStates[key] == KeyState.Hit;
//        }
//    }
//}