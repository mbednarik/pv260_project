using DAL.Models;
using DAL.Repository;
using DAL.UnitOfWork.Interface;

namespace DAL.UnitOfWork
{
    public class UoWHoldingDiff : IUoWHoldingDiff
    {
        public IRepository<HoldingDiff> HoldingDiffRepository { get; }

        private readonly FundParserDbContext context;

        public UoWHoldingDiff(FundParserDbContext context,
            IRepository<HoldingDiff> holdingDiffRepository)
        {
            this.context = context;
            HoldingDiffRepository = holdingDiffRepository;
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
