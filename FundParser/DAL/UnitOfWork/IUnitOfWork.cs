using DAL.Models;
using DAL.Repository;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<Fund> FundRepository { get; }

        public IRepository<Company> CompanyRepository { get; }

        public IRepository<Holding> HoldingRepository { get; }

        public IRepository<HoldingDiff> HoldingDiffRepository { get; }

        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}