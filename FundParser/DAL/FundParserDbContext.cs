using DAL.Data;
using DAL.Models;

using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class FundParserDbContext : DbContext
    {
        public DbSet<Fund> Funds { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Holding> Holdings { get; set; }

        public DbSet<HoldingDiff> HoldingDiffs { get; set; }

        public FundParserDbContext() : base(Init())
        {
        }

        public FundParserDbContext(DbContextOptions<FundParserDbContext> options)
        : base(options)
        {
        }

        public static DbContextOptions Init()
        {
            return new DbContextOptionsBuilder<FundParserDbContext>()
                .UseLazyLoadingProxies()
                .Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }
    }
}
