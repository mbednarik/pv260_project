using DAL.Models;
using DAL.Repository;

namespace DAL.UnitOfWork.Interface;

public interface IUoWFund : IUnitOfWork
{
    IRepository<Fund> FundRepository { get; }
}