using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConsoleCookieClicker.Models
{
    public class UpgradeModel
    {
        public string Name { get; set; }
        public int BasePrice { get; set; }
        public int BaseCookiesPerTick { get; set; }
        public int Count { get; set; }

        public int CookiesPerTick()
        {
            return Count * BaseCookiesPerTick;
        }

        public int Price()
        {
            return BasePrice;
        }

        public string DisplayText() 
        {
            return Name + ": " + Count + " making " + CookiesPerTick() + " cookies per tick";
        }

        public string BuyText()
        {
            return "Buy " + Name + " for " + Price() + " cookies";
        }
    }
}
