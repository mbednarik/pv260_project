using DAL.Models;
using DAL.Repository;
using DAL.UnitOfWork.Interface;

namespace DAL.UnitOfWork
{
    public class UoWHolding : IUoWHolding
    {
        public IRepository<Holding> HoldingRepository { get; }

        private readonly FundParserDbContext context;

        public UoWHolding(FundParserDbContext context,
            IRepository<Holding> holdingRepository)
        {
            this.context = context;
            HoldingRepository = holdingRepository;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
