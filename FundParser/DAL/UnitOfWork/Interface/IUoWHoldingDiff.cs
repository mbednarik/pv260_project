using DAL.Models;
using DAL.Repository;

namespace DAL.UnitOfWork.Interface
{
    public interface IUoWHoldingDiff : IUnitOfWork
    {
        IRepository<HoldingDiff> HoldingDiffRepository { get; }
    }
}
