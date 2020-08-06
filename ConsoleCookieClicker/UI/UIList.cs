using ConsoleCookieClicker.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCookieClicker.UI
{
    class UIList
    {
        private int x;
        private int y;
        private List<string> items;
        private ConsoleGraphics.TextAlign alignment;

        public UIList(int x, int y, List<string> items, ConsoleGraphics.TextAlign alignment = ConsoleGraphics.TextAlign.Left)
        {
            this.x = x;
            this.y = y;
            this.items = items;
            this.alignment = alignment;
        }

        public void Update() { }

        public void Update(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void ReplaceItem(string previousItem, string item)
        {
            int index = items.FindIndex(i => i == previousItem);
            items[index] = item;
        }

        public void Draw()
        {
            for (var i = 0; i < items.Count; i++)
            {
                ConsoleGraphics.Text(x, y + i, items[i], alignment);
            }
        }
    }
}
