using FundParser.DAL.Models;
using FundParser.DAL.Repository;

namespace FundParser.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly FundParserDbContext _context;

    public IRepository<Fund> FundRepository { get; }

    public IRepository<Company> CompanyRepository { get; }

    public IRepository<Holding> HoldingRepository { get; }

    public IRepository<HoldingDiff> HoldingDiffRepository { get; }

    public UnitOfWork(
        FundParserDbContext context,
        IRepository<Fund> fundRepository,
        IRepository<Company> companyRepository,
        IRepository<Holding> holdingRepository,
        IRepository<HoldingDiff> holdingDiffRepository)
    {
        _context = context;
        FundRepository = fundRepository;
        CompanyRepository = companyRepository;
        HoldingRepository = holdingRepository;
        HoldingDiffRepository = holdingDiffRepository;
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