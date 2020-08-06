using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ConsoleCookieClicker.Util
{
    public static class DeltaTimer
    {
        private const double perfectFrameTime = 1000 / 60;
        private static double deltaTime = 0;
        private static Stopwatch stopwatch = new Stopwatch();

        public static double Step()
        {
            deltaTime = stopwatch.Elapsed.TotalMilliseconds / perfectFrameTime;
            stopwatch.Restart();
            return deltaTime;
        }

        public static double Get()
        {
            return deltaTime;
        }
    }
}
