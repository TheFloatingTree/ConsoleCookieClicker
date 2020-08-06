using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ConsoleCookieClicker.Models
{
    public class CookieClickerDataModel
    {
        public int Id { get; set; }
        public BigInteger Cookies { get; set; }
        public List<UpgradeModel> Upgrades { get; } = new List<UpgradeModel>();
    }
}
