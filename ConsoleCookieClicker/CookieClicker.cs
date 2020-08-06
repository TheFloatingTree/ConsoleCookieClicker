using ConsoleCookieClicker.Config;
using ConsoleCookieClicker.Entities;
using ConsoleCookieClicker.Models;
using ConsoleCookieClicker.Respositories;
using ConsoleCookieClicker.UI;
using ConsoleCookieClicker.Util;
using System;
using System.Linq;

namespace ConsoleCookieClicker
{
    public class CookieClicker
    {
        // Data
        private CookieClickerDataModel data;

        // UI
        private Cookie cookie;
        private Wave wave;
        private UISelectList upgradeSelectList;
        private UIList upgradeList;

        // Misc
        private double simulationTimer = 0;
        private const double simulationInterval = 30;
        private double autoSaveTimer = 0;
        private const double autoSaveInterval = 300;
        private bool shouldAutoSave = true;

        public CookieClicker()
        {
            Load();
        }

        public void Run()
        {
            while (true)
            {
                DeltaTimer.Step();
                InputManager.Update();
                Update();
                Draw();
                ParticleManager.Run();
                ConsoleGraphics.Draw();
            }
        }

        public void Update()
        {
            if (InputManager.GetKey(ConsoleKey.Escape)) Exit();
            if (InputManager.GetKey(ConsoleKey.Delete)) Reset();
            if (InputManager.GetKey(ConsoleKey.Tab)) shouldAutoSave = !shouldAutoSave;

            simulationTimer += DeltaTimer.Get();
            autoSaveTimer += DeltaTimer.Get();
            if (simulationTimer >= simulationInterval) SimulationTick();
            if (autoSaveTimer >= autoSaveInterval) AutoSave();

            cookie.Update();
            wave.Update();
            upgradeSelectList.Update();
            upgradeList.Update(Console.WindowWidth - 1, 3);
        }

        public void Draw()
        {
            DrawUI();
            wave.Draw();
            cookie.Draw();
            upgradeSelectList.Draw();
            upgradeList.Draw();
        }

        public void SimulationTick()
        {
            simulationTimer -= simulationInterval;
            data.Cookies += data.Upgrades.Where(item => item.Count > 0).Sum(item => item.CookiesPerTick());
        }

        private void BuildUI()
        {
            cookie = new Cookie();
            wave = new Wave();

            upgradeList = new UIList(Console.WindowWidth - 1, 3, data.Upgrades
                .Select(upgrade => upgrade.DisplayText())
                .ToList()
                , ConsoleGraphics.TextAlign.Right);

            upgradeSelectList = new UISelectList(1, 3, data.Upgrades
                .Select(upgrade => upgrade.BuyText())
                .ToList()
                );

            cookie.OnCookiePress += Cookie_OnCookiePress;
            upgradeSelectList.OnItemPress += UpgradeSelectList_OnItemPress;
        }

        private void Cookie_OnCookiePress()
        {
            var cookieIncrease = data.Cookies / 100;
            cookieIncrease = (cookieIncrease <= 1) ? 1 : cookieIncrease;
            data.Cookies += cookieIncrease;
        }

        private void UpgradeSelectList_OnItemPress(string item)
        {
            UpgradeModel upgrade = data.Upgrades.Find(u => item == u.BuyText());

            if (upgrade.Price() > data.Cookies) return;

            data.Cookies -= upgrade.Price();
            string previousText = upgrade.DisplayText();
            upgrade.Count++;
            upgradeList.ReplaceItem(previousText, upgrade.DisplayText());
        }

        private void Load()
        {
            try
            {
                data = CookieClickerRepository.GetMostRecentData();
            }
            catch
            {
                LoadDefaults();
            }
            BuildUI();
        }

        private void Reset()
        {
            CookieClickerRepository.DeleteAllData();
            Load();
        }

        private void AutoSave()
        {
            if (!shouldAutoSave) return;
            autoSaveTimer -= autoSaveInterval;
            try
            {
                CookieClickerRepository.UpdateData(data);
            }
            catch 
            {
                shouldAutoSave = false;
            }
        }

        public void Save()
        {
            CookieClickerRepository.SaveData(data);
        }

        public void Exit()
        {
            Save();
            Environment.Exit(0);
        }

        private void LoadDefaults()
        {
            data = new CookieClickerDataModel();

            data.Upgrades.Add(new UpgradeModel()
            {
                Name = "Auto Clicker",
                BasePrice = 50,
                BaseCookiesPerTick = 1
            });

            data.Upgrades.Add(new UpgradeModel()
            {
                Name = "Grandma",
                BasePrice = 250,
                BaseCookiesPerTick = 5
            });

            data.Upgrades.Add(new UpgradeModel()
            {
                Name = "Farm",
                BasePrice = 1000,
                BaseCookiesPerTick = 25
            });

            data.Upgrades.Add(new UpgradeModel()
            {
                Name = "Mine",
                BasePrice = 15000,
                BaseCookiesPerTick = 100
            });

            data.Upgrades.Add(new UpgradeModel()
            {
                Name = "Factory",
                BasePrice = 100000,
                BaseCookiesPerTick = 500
            });

            data.Upgrades.Add(new UpgradeModel()
            {
                Name = "Bank",
                BasePrice = 500000,
                BaseCookiesPerTick = 2500
            });
        }

        public void DrawUI()
        {
            ConsoleGraphics.Text(1, 1, "Console Cookie Clicker");
            ConsoleGraphics.Text(ConsoleGraphics.ConsoleHalfWidth, 0, "Cookies: " + data.Cookies, ConsoleGraphics.TextAlign.Center);
            ConsoleGraphics.Text(1, Console.WindowHeight - 4, "Press Esc to save and exit");
            ConsoleGraphics.Text(1, Console.WindowHeight - 3, "Press Del to reset");
            ConsoleGraphics.Text(1, Console.WindowHeight - 2, "Press Tab to toggle auto save");
            ConsoleGraphics.Text(
                Console.WindowWidth - 1, 
                Console.WindowHeight - 2, 
                shouldAutoSave ? "Auto save is on" : "Auto save is off", 
                ConsoleGraphics.TextAlign.Right
                );
        }
    }
}
