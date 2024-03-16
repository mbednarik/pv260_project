using System.Globalization;
using BL.DTOs;
using BL.Services.CompanyService;
using BL.Services.FundService;
using BL.Services.HoldingService;
using DAL.Csv;
using DAL.Models;
using DAL.UnitOfWork.Interface;

namespace BL.Services.FundCsvService;

public class FundCsvService : IFundCsvService
{
    private readonly IFundService _fundService;
    private readonly ICompanyService _companyService;
    private readonly IHoldingService _holdingService;
    private readonly IUoWFund _uowFund;
    private readonly IUoWCompany _uowCompany;
    private readonly IUoWHolding _uowHolding;
    private readonly CsvDownloader<FundCsvRow> _csvDownloader;

    // TODO: rework this to use a single UoW, separate PR
    public FundCsvService(IFundService fundService, ICompanyService companyService, IHoldingService holdingService,
        CsvDownloader<FundCsvRow> csvDownloader, IUoWFund uowFund, IUoWCompany uowCompany, IUoWHolding uowHolding)
    {
        _fundService = fundService;
        _companyService = companyService;
        _holdingService = holdingService;
        _csvDownloader = csvDownloader;
        _uowFund = uowFund;
        _uowCompany = uowCompany;
        _uowHolding = uowHolding;
    }

    public async Task<int> UpdateHoldings()
    {
        // TODO: move this to config
        var url = "http://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
        var rows = await _csvDownloader.DownloadAndParse(url);

        if (rows == null)
        {
            throw new Exception("Failed to download csv");
        }

        var successfulRows = 0;
        foreach (var row in rows)
        {
            try
            {
                await ParseRow(row);
                successfulRows++;
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        return successfulRows;
    }

    private (AddFundDTO, AddCompanyDTO, AddHoldingDTO) ParseRowComponents(FundCsvRow row)
    {
        var fund = new AddFundDTO
        {
            Name = row.fund,
        };

        var company = new AddCompanyDTO
        {
            Cusip = row.cusip,
            Ticker = row.ticker,
            Name = row.company,
        };
            
        var holding = new AddHoldingDTO
        {
            Shares = decimal.Parse(row.shares),
            MarketValue = decimal.Parse(row.marketValue.TrimStart('$')),
            Weight = decimal.Parse(row.weight.TrimEnd('%')),
            Date = DateTime.ParseExact(row.date, "MM/dd/yyyy", CultureInfo.InvariantCulture),
        };

        return (fund, company, holding);
    }

    private async Task ParseRow(FundCsvRow row)
    {
        var (fund, company, holding) = ParseRowComponents(row);
        
        await _fundService.PrepareFundIfNotExists(fund);
        await _companyService.PrepareCompanyIfNotExists(company);
        await _holdingService.PrepareHolding(holding);
        
        // TODO: rework this to use a single UoW, separate PR
        await _uowFund.CommitAsync();
        await _uowCompany.CommitAsync();
        await _uowHolding.CommitAsync();
    }
}