using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCookieClicker.Entities
{
    public class Upgrade
    {
        public int Id { get; set; }
        public int CookieClickerDataId { get; set; }
        public string Name { get; set; }
        public int BasePrice { get; set; }
        public int BaseCookiesPerTick { get; set; }
        public int Count { get; set; }
    }
}
