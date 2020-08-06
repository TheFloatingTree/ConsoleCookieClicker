using ConsoleCookieClicker.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCookieClicker.UI
{
    public class Wave
    {
        private double counter = 0;

        public void Update()
        {
            counter += 0.05;
        }

        public void Draw()
        {

            for (var i = 0; i < Console.WindowWidth; i++)
            {
                var angle = i + counter;
                var yOffset = (Math.Sin(angle * 0.05) * 2);
                var y = (Console.WindowHeight - 10) + yOffset;
                ConsoleGraphics.Pixel(i, y, '#');
            }
        }
    }
}
