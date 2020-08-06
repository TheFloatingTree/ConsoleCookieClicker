using AutoMapper;
using ConsoleCookieClicker.Entities;
using ConsoleCookieClicker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCookieClicker.Config
{
    public static class MappingConfig
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg =>
         {
             cfg.CreateMap<CookieClickerData, CookieClickerDataModel>().ReverseMap();
             cfg.CreateMap<Upgrade, UpgradeModel>().ReverseMap();
         }).CreateMapper();
    }
}
