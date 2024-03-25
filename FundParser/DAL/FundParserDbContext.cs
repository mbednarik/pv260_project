using FundParser.DAL.Data;
using FundParser.DAL.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FundParser.DAL
{
    public class FundParserDbContext : DbContext
    {
        public DbSet<Fund> Funds { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Holding> Holdings { get; set; }

        public DbSet<HoldingDiff> HoldingDiffs { get; set; }

        public FundParserDbContext() : base()
        {
        }

        public FundParserDbContext(DbContextOptions<FundParserDbContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("dataAppsettings.json")
                .Build();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("SqlLite"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();

            modelBuilder.Entity<Holding>().Navigation(h => h.Fund).AutoInclude();
            modelBuilder.Entity<Holding>().Navigation(h => h.Company).AutoInclude();

            base.OnModelCreating(modelBuilder);
        }
    }
}
