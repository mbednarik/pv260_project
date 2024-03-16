using System.Globalization;
using BL.DTOs;
using BL.Services.CompanyService;
using BL.Services.FundService;
using BL.Services.HoldingService;
using DAL.Csv;
using DAL.Models;

namespace BL.Services.CsvService;

public class FundCsvService : ICsvService
{
    private readonly IFundService _fundService;
    private readonly ICompanyService _companyService;
    private readonly IHoldingService _holdingService;
    private readonly CsvDownloader<FundCsvRow> _csvDownloader;

    public FundCsvService(IFundService fundService, ICompanyService companyService, IHoldingService holdingService, CsvDownloader<FundCsvRow> csvDownloader)
    {
        _fundService = fundService;
        _companyService = companyService;
        _holdingService = holdingService;
        _csvDownloader = csvDownloader;
    }

    public async Task<bool> InsertRowIntoDb(FundCsvRow row)
    {
        var fund = await _fundService.AddFundIfNotExists(new AddFundDTO
        {
            Name = row.fund,
        });
        if (fund == null)
        {
            return false;
        }

        var company = await _companyService.AddCompanyIfNotExists(new AddCompanyDTO
        {
            Cusip = row.cusip,
            Ticker = row.ticker,
            Name = row.company,
        });
        if (company == null)
        {
            return false;
        }

        var holding = await _holdingService.AddHolding(new AddHoldingDTO
        {
            FundId = fund.Id,
            CompanyId = company.Id,
            Shares = decimal.Parse(row.shares),
            MarketValue = decimal.Parse(row.marketValue.TrimStart('$')),
            Weight = decimal.Parse(row.weight.TrimEnd('%')),
            Date = DateTime.ParseExact(row.date, "MM/dd/yyyy", CultureInfo.InvariantCulture),
        });
        return holding != null;
    }

    public async Task<IEnumerable<FundCsvRow>?> GetRowsFromUrl(string url)
    {
        return await _csvDownloader.DownloadAndParse(url);
    }
}