using ConsoleCookieClicker.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConsoleCookieClicker.UI
{
    class UISelectList
    {
        private int x;
        private int y;
        private List<string> items;
        private int selectedIndex = 0;
        private ConsoleGraphics.TextAlign alignment;

        public delegate void OnItemPressHandler(string item);
        public event OnItemPressHandler OnItemPress;

        public UISelectList(int x, int y, List<string> items, ConsoleGraphics.TextAlign alignment = ConsoleGraphics.TextAlign.Left)
        {
            this.x = x;
            this.y = y;
            this.items = items;
            this.alignment = alignment;
        }

        public void Update()
        {
            if (InputManager.GetKey(ConsoleKey.DownArrow) || InputManager.GetKey(ConsoleKey.S))
            {
                selectedIndex = (selectedIndex + 1) % items.Count();
            }

            if (InputManager.GetKey(ConsoleKey.UpArrow) || InputManager.GetKey(ConsoleKey.W))
            {
                selectedIndex = (selectedIndex - 1) % items.Count();
                if (selectedIndex < 0) selectedIndex = items.Count() - 1;
            }

            if (InputManager.GetKey(ConsoleKey.Enter))
            {
                OnItemPress?.Invoke(items[selectedIndex]);
            }
        }

        public void Draw()
        {
            for (var i = 0; i < items.Count(); i++)
            {
                string item = items[i];
                string outText = (item == items[selectedIndex]) ? "> " + item : item;
                ConsoleGraphics.Text(x, y + i, outText, alignment);
            }
        }
    }
}
