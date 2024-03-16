using System.Globalization;

using BL.DTOs;
using BL.Services.HoldingService;

using DAL.Csv;
using DAL.Models;
using DAL.UnitOfWork.Interface;

namespace BL.Services.FundCsvService;

public class FundCsvService : IFundCsvService
{
    private readonly IHoldingService _holdingService;
    private readonly IUoWHolding _uowHolding;
    private readonly CsvDownloader<FundCsvRow> _csvDownloader;

    // TODO: rework this to use a single UoW, separate PR
    public FundCsvService(
        IHoldingService holdingService,
        CsvDownloader<FundCsvRow> csvDownloader,
        IUoWHolding uowHolding)
    {
        _holdingService = holdingService;
        _csvDownloader = csvDownloader;
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

    private async Task ParseRow(FundCsvRow row)
    {
        var holding = ParseFund(row);

        await _holdingService.AddHolding(holding);

        // TODO: rework this to use a single UoW, separate PR
        await _uowHolding.CommitAsync();
    }

    private AddHoldingDTO ParseFund(FundCsvRow row)
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
            Fund = fund,
            Company = company,
            Shares = decimal.Parse(row.shares),
            MarketValue = decimal.Parse(row.marketValue.TrimStart('$')),
            Weight = decimal.Parse(row.weight.TrimEnd('%')),
            Date = DateTime.ParseExact(row.date, "MM/dd/yyyy", CultureInfo.InvariantCulture),
        };

        return holding;
    }
}