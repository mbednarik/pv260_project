using DAL.Models;
using DAL.Repository;
using DAL.UnitOfWork.Interface;

namespace DAL.UnitOfWork;

public class UoWCompany : IUoWCompany
{
    private readonly FundParserDbContext _context;

    public IRepository<Company> CompanyRepository { get; }

    public UoWCompany(FundParserDbContext context,
        IRepository<Company> companyRepository)
    {
        _context = context;
        CompanyRepository = companyRepository;
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