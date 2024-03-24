using DAL.Models;
using DAL.Repository;
using DAL.UnitOfWork.Interface;

namespace DAL.UnitOfWork;

public class UoWFund : IUoWFund
{
    private readonly FundParserDbContext _context;

    public IRepository<Fund> FundRepository { get; }

    public UoWFund(FundParserDbContext context,
        IRepository<Fund> fundRepository)
    {
        _context = context;
        FundRepository = fundRepository;
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