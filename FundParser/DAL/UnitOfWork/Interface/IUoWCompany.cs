using DAL.Models;
using DAL.Repository;

namespace DAL.UnitOfWork.Interface;

public interface IUoWCompany : IUnitOfWork
{
    IRepository<Company> CompanyRepository { get; }
}