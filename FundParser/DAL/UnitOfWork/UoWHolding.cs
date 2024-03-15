using DAL.Models;
using DAL.Repository;
using DAL.UnitOfWork.Interface;

namespace DAL.UnitOfWork
{
    public class UoWHolding : IUoWHolding
    {
        public IRepository<Holding> HoldingRepository { get; }

        private readonly FundParserDbContext _context;

        public UoWHolding(FundParserDbContext context,
            IRepository<Holding> holdingRepository)
        {
            _context = context;
            HoldingRepository = holdingRepository;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
