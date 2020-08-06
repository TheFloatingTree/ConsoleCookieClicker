using AutoMapper;
using ConsoleCookieClicker.Config;
using ConsoleCookieClicker.Entities;
using ConsoleCookieClicker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCookieClicker.Respositories
{
    public static class CookieClickerRepository
    {
        public static CookieClickerDataModel GetMostRecentData()
        {
            IMapper mapper = MappingConfig.Mapper;

            using (var db = new CookieClickerContext())
            {
                CookieClickerData cookieClickerData = db.Data.OrderByDescending(d => d.Id).First();
                List<Upgrade> upgrades = db.Upgrades.Where(u => u.CookieClickerDataId == cookieClickerData.Id).ToList();

                CookieClickerDataModel cookieClickerDataModel = mapper.Map<CookieClickerDataModel>(cookieClickerData);
                List<UpgradeModel> upgradeModels = upgrades.Select(u => mapper.Map<UpgradeModel>(u)).ToList();
                cookieClickerDataModel.Upgrades.AddRange(upgradeModels);
                return cookieClickerDataModel;
            }
        }

        public static void SaveData(CookieClickerDataModel cookieClickerDataModel)
        {
            IMapper mapper = MappingConfig.Mapper;

            using (var db = new CookieClickerContext())
            {
                CookieClickerData cookieClickerData = mapper.Map<CookieClickerData>(cookieClickerDataModel);
                cookieClickerData.Id = 0; // Since we're making a new one we need a new PK
                List<Upgrade> upgrades = cookieClickerDataModel.Upgrades.Select(u => mapper.Map<Upgrade>(u)).ToList();

                db.Data.Add(cookieClickerData);
                db.SaveChanges();

                upgrades.ForEach(u => u.CookieClickerDataId = cookieClickerData.Id);

                db.Upgrades.AddRange(upgrades);
                db.SaveChangesAsync();
            }
        }

        public static void UpdateData(CookieClickerDataModel cookieClickerDataModel)
        {
            IMapper mapper = MappingConfig.Mapper;

            using (var db = new CookieClickerContext())
            {
                CookieClickerData oldCookieClickerData = db.Data.OrderByDescending(d => d.Id).First();
                List<Upgrade> oldUpgrades = db.Upgrades.Where(u => u.CookieClickerDataId == oldCookieClickerData.Id).ToList();

                oldCookieClickerData.Cookies = cookieClickerDataModel.Cookies;
                for (var i = 0; i < oldUpgrades.Count; i++)
                {
                    oldUpgrades[i].Count = cookieClickerDataModel.Upgrades[i].Count;
                }

                db.SaveChanges();
            }
        }

        public static void DeleteData(CookieClickerDataModel cookieClickerDataModel)
        {
            using (var db = new CookieClickerContext())
            {
                CookieClickerData cookieClickerData = db.Data.Where(d => d.Id == cookieClickerDataModel.Id).FirstOrDefault();
                List<Upgrade> upgrades = db.Upgrades.Where(u => u.CookieClickerDataId == cookieClickerData.Id).ToList();

                db.Remove(cookieClickerData);
                db.RemoveRange(upgrades);

                db.SaveChanges();
            }
        }

        public static void DeleteAllData()
        {
            using (var db = new CookieClickerContext())
            {
                List<CookieClickerData> cookieClickerData = db.Data.ToList();
                List<Upgrade> upgrades = db.Upgrades.ToList();

                db.RemoveRange(cookieClickerData);
                db.RemoveRange(upgrades);

                db.SaveChanges();
            }
        }
    }
}
