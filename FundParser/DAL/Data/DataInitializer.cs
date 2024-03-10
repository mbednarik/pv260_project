using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public static class DataInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            Fund fund1 = new Fund { Id = 1, Name = "Fund1" };
            modelBuilder.Entity<Fund>().HasData(fund1);

            Company company1 = new Company { Id = 1, Name = "Company1", Cusip = "CUSIP1", Ticker = "TICK1" };
            modelBuilder.Entity<Company>().HasData(company1);

            Holding holding1 = new Holding
            {
                Id = 1,
                Date = DateTime.Now,
                FundId = fund1.Id,
                CompanyId = company1.Id,
                Shares = 1000,
                MarketValue = 50000,
                Weight = 0.05m
            };
            modelBuilder.Entity<Holding>().HasData(holding1);
        }
    }
}
