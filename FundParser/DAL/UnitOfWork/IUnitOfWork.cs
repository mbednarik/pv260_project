using FundParser.DAL.Models;
using FundParser.DAL.Repository;

namespace FundParser.DAL.UnitOfWork
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