using DAL.Models;
using DAL.Repository;
using DAL.UnitOfWork.Interface;

namespace DAL.UnitOfWork
{
    public class UoWHolding : IUoWHolding
    {
        private readonly FundParserDbContext _context;

        public IRepository<Holding> HoldingRepository { get; }

        public UoWHolding(FundParserDbContext context,
            IRepository<Holding> holdingRepository)
        {
            _context = context;
            HoldingRepository = holdingRepository;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
