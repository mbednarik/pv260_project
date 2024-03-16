﻿using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public static class DataInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var fund1 = new Fund { Id = 1, Name = "Fund1" };
            modelBuilder.Entity<Fund>().HasData(fund1);

            var company1 = new Company { Id = 1, Name = "Company1", Cusip = "CUSIP1", Ticker = "TICK1" };
            modelBuilder.Entity<Company>().HasData(company1);

            var holding1 = new Holding
            {
                Id = 1,
                Date = new DateTime(2024, 1, 1),
                FundId = fund1.Id,
                CompanyId = company1.Id,
                Shares = 1000,
                MarketValue = 50000,
                Weight = 0.05m
            };
            
            var holding2 = new Holding
            {
                Id = 2,
                Date = new DateTime(2024, 2, 1),
                FundId = fund1.Id,
                CompanyId = company1.Id,
                Shares = 2000,
                MarketValue = 100000,
                Weight = 0.1m
            };

            modelBuilder.Entity<Holding>().HasData(holding1);
            modelBuilder.Entity<Holding>().HasData(holding2);
        }
    }
}
