using ConsoleCookieClicker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace ConsoleCookieClicker
{
    public class CookieClickerContext : DbContext
    {
        public DbSet<CookieClickerData> Data { get; set; }
        public DbSet<Upgrade> Upgrades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=data.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CookieClickerData>()
                .Property(e => e.Cookies)
                .HasConversion(new ValueConverter<BigInteger, string>(
                    model => model.ToString(),
                    provider => BigInteger.Parse(provider)
                    ));
        }
    }
}
