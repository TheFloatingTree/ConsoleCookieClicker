using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleCookieClicker.Util
{
    static class InputManager
    {
        private static Thread inputWorker;
        private static ConsoleKey? currentKey;
        private static ConsoleKey? frameBoundCurrentKey;
        private static bool didInit = false;

        public static void Start()
        {
            inputWorker = new Thread(InputWorker);
            inputWorker.Start();
        }

        public static void Update()
        {
            if (!didInit)
            {
                didInit = true;
                Start();
            }

            frameBoundCurrentKey = currentKey;
            currentKey = null;
        }

        public static bool GetKey(ConsoleKey key)
        {
            return frameBoundCurrentKey == key;
        }

        private static void InputWorker()
        {
            while (true)
            {
                currentKey = Console.ReadKey(true).Key;
            }
        }
    }
}
