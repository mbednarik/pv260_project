using DAL.Models;
using DAL.Repository;

namespace DAL.UnitOfWork.Interface
{
    public interface IUoWHolding : IUnitOfWork
    {
        IRepository<Holding> HoldingRepository { get; }
    }
}
