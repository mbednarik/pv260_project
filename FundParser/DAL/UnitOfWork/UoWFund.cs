using DAL.Models;
using DAL.Repository;
using DAL.UnitOfWork.Interface;

namespace DAL.UnitOfWork;

public class UoWFund : IUoWFund
{
    public IRepository<Fund> FundRepository { get; }
    private readonly FundParserDbContext _context;
    
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

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

}