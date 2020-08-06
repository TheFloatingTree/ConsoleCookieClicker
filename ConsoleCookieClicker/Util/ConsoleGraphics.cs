using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleCookieClicker.Util
{
    static class ConsoleGraphics
    {
        static public int ConsoleHalfWidth = Console.WindowWidth / 2;
        static public int ConsoleHalfHeight = Console.WindowHeight / 2;

        static private int width = 0;
        static private int height = 0;
        static private char[][] screenBuffer;
        static private List<Tuple<int, int>> changedPixels = new List<Tuple<int, int>>();

        private const char defaultStroke = '@';
        private const char defaultFill = '@';

        public enum TextAlign
        {
            Left,
            Right,
            Center
        }

        static ConsoleGraphics()
        {
            UpdateScreenSize(Console.WindowWidth, Console.WindowHeight);
        }

        static public void Draw()
        {
            if (Console.WindowWidth == 0 || Console.WindowHeight == 0)
                return;

            if (width != Console.WindowWidth || height + 1 != Console.WindowHeight)
            {
                UpdateScreenSize(Console.WindowWidth, Console.WindowHeight);
            }

            // Draw left-right and down, clear screen, set the cursor to top left corner
            for (int i = 0; i < height; i++)
            {
                Console.Write(screenBuffer[i]);
            }

            Clear();
            Console.SetCursorPosition(0, 0);
        }

        static private void UpdateScreenSize(int width, int height)
        {
            // Adjust screen buffer size
            ConsoleHalfWidth = Console.WindowWidth / 2;
            ConsoleHalfHeight = Console.WindowHeight / 2;
            ConsoleGraphics.width = width;
            ConsoleGraphics.height = height - 1;
            screenBuffer = new char[ConsoleGraphics.height][];
            for (int i = 0; i < ConsoleGraphics.height; i++)
            {
                var xArray = new char[ConsoleGraphics.width];
                for (int j = 0; j < ConsoleGraphics.width; j++)
                {
                    xArray[j] = ' ';
                }
                screenBuffer[i] = xArray;
            }
            // Reset cursor to not visible
            Console.CursorVisible = false;
        }

        static private bool CheckBounds(int x, int y)
        {
            if (x < width && y < height && x >= 0 && y >= 0)
                return true;
            else
                return false;
        }

        static public void Pixel(int x, int y, char type = defaultStroke)
        {
            if (CheckBounds(x, y))
            {
                changedPixels.Add(new Tuple<int, int>(x, y));
                screenBuffer[y][x] = type;
            }
        }

        static public void Pixel(double x, double y, char type = defaultStroke)
        {
            Pixel((int)x, (int)y, type);
        }

        static public void Text(int x, int y, string text, TextAlign alignment = TextAlign.Left)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (alignment == TextAlign.Center)
                {
                    Pixel(x + i - text.Length / 2, y, text[i]);
                }
                if (alignment == TextAlign.Right)
                {
                    Pixel(x + i - text.Length, y, text[i]);
                }
                if (alignment == TextAlign.Left)
                {
                    Pixel(x + i, y, text[i]);
                }
            }
        }

        static public void Circle(int x, int y, int r, char stroke = defaultStroke)
        {
            Ellipse(x, y, (int)(r * 2.1), r, stroke); // Magic 2.1 to account for pixel dimensions
        }

        static public void Ellipse(int xc, int yc, int rx, int ry, char stroke = defaultStroke)
        {
            // Adapted from https://saideepdicholkar.blogspot.com/2017/02/midpoint-ellipse-algorithm-ellipse.html

            double p, x, y;
            x = 0;
            y = ry;

            // Two extra pixels to cover gaps caused by rounding errors
            Pixel(xc + rx, yc, stroke);
            Pixel(xc - rx, yc, stroke);

            //Region 1
            p = (ry * ry) - (rx * rx * ry) + (0.25 * rx * rx);
            do
            {
                Pixel(xc + x, yc + y, stroke);
                Pixel(xc + x, yc - y, stroke);
                Pixel(xc - x, yc + y, stroke);
                Pixel(xc - x, yc - y, stroke);
                if (p < 0)
                {
                    x += 1;
                    p = p + 2 * ry * ry * x + ry * ry;
                }
                else
                {
                    x += 1;
                    y -= 1;
                    p = p + 2 * ry * ry * x - 2 * rx * rx * y + ry * ry;
                }
            } while (2 * ry * ry * x < 2 * rx * rx * y);
            //Region 2
            p = (ry * ry * (x + 0.5) * (x + 0.5)) + ((y - 1) * (y - 1) * rx * rx - rx * rx * ry * ry);
            do
            {
                Pixel(xc + x, yc + y, stroke);
                Pixel(xc + x, yc - y, stroke);
                Pixel(xc - x, yc + y, stroke);
                Pixel(xc - x, yc - y, stroke);
                if (p > 0)
                {
                    y -= 1;
                    p = p - 2 * rx * rx * y + rx * rx;
                }
                else
                {
                    x += 1;
                    y -= 1;
                    p = p - 2 * rx * rx * y + 2 * ry * ry * x + rx * rx;
                }
            } while (y != 0);
        }

        static public void FilledCircle(int x, int y, int r, char fill = '@', char stroke = defaultStroke)
        {
            FilledEllipse(x, y, (int)(r * 2.1), r, fill, stroke); // Magic 2.1 to account for pixel dimensions
        }

        static public void FilledEllipse(int xc, int yc, int rx, int ry, char fill = '@', char stroke = defaultStroke)
        {
            // Adapted from https://stackoverflow.com/questions/10322341/simple-algorithm-for-drawing-filled-ellipse-in-c-c

            int hh = ry * ry;
            int ww = rx * rx;
            int hhww = hh * ww;
            int x0 = rx;
            int dx = 0;

            // do the horizontal diameter
            for (int x = -rx; x <= rx; x++)
                Pixel(xc + x, yc, fill);

            // now do both halves at the same time, away from the diameter
            for (int y = 1; y <= ry; y++)
            {
                int x1 = x0 - (dx - 1);  // try slopes of dx - 1 or more
                for (; x1 > 0; x1--)
                    if (x1 * x1 * hh + y * y * ww <= hhww)
                        break;
                dx = x0 - x1;  // current approximation of the slope
                x0 = x1;

                for (int x = -x0; x <= x0; x++)
                {
                    Pixel(xc + x, yc - y, fill);
                    Pixel(xc + x, yc + y, fill);
                }
            }

            Ellipse(xc, yc, rx, ry, stroke);
        }

        static public void Rect(int x, int y, int w, int h, char stroke = defaultStroke)
        {
            // Calculate verticies
            int x1 = x;
            int y1 = y;
            int x2 = x;
            int y2 = y + h;
            int x3 = x + w;
            int y3 = y + h;
            int x4 = x + w;
            int y4 = y;
            // Draw lines
            Line(x1, y1, x2, y2, stroke);
            Line(x2, y2, x3, y3, stroke);
            Line(x3, y3, x4, y4, stroke);
            Line(x4, y4, x1, y1, stroke);
        }

        static public void Line(int x1, int y1, int x2, int y2, char stroke = defaultStroke)
        {
            // DDA Algorithm
            int dx = x2 - x1;
            int dy = y2 - y1;

            int steps = Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy);

            float Xinc = dx / (float)steps;
            float Yinc = dy / (float)steps;

            float x = x1;
            float y = y1;

            for (int i = 0; i < steps; i++)
            {
                Pixel(x, y, stroke);
                x += Xinc;
                y += Yinc;
            }
        }

        static public void Fill(char fill = defaultFill)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Pixel(x, y, fill);
        }

        static public void Clear()
        {
            // Iterate through changed pixels and clear them
            foreach (var pair in changedPixels)
            {
                if (pair.Item1 < width && pair.Item2 < height && pair.Item1 >= 0 && pair.Item2 >= 0)
                {
                    screenBuffer[pair.Item2][pair.Item1] = ' ';
                }
            }
            changedPixels.Clear();
        }
    }
}
