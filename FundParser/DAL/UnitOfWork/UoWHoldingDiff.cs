using DAL.Models;
using DAL.Repository;
using DAL.UnitOfWork.Interface;

namespace DAL.UnitOfWork
{
    public class UoWHoldingDiff : IUoWHoldingDiff
    {
        private readonly FundParserDbContext context;

        public IRepository<HoldingDiff> HoldingDiffRepository { get; }

        public UoWHoldingDiff(FundParserDbContext context,
            IRepository<HoldingDiff> holdingDiffRepository)
        {
            this.context = context;
            HoldingDiffRepository = holdingDiffRepository;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
