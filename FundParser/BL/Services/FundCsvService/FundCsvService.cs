using System.Globalization;

using BL.DTOs;
using BL.Services.HoldingService;

using DAL.Csv;
using DAL.Models;
using DAL.UnitOfWork;

namespace BL.Services.FundCsvService;

public class FundCsvService : IFundCsvService
{
    private readonly IHoldingService _holdingService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICsvDownloader<FundCsvRow> _csvDownloader;

    public FundCsvService(
        IHoldingService holdingService,
        ICsvDownloader<FundCsvRow> csvDownloader,
        IUnitOfWork unitOfWork)
    {
        _holdingService = holdingService;
        _csvDownloader = csvDownloader;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> UpdateHoldings(CancellationToken cancellationToken = default)
    {
        // TODO: move this to config
        var url = "http://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
        var rows = await _csvDownloader.DownloadAndParse(url, cancellationToken);

        if (rows == null)
        {
            throw new Exception("Failed to download csv");
        }

        var successfulRows = 0;
        foreach (var row in rows)
        {
            try
            {
                await ProcessRow(row, cancellationToken);
                successfulRows++;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        return successfulRows;
    }

    private async Task ProcessRow(FundCsvRow row, CancellationToken cancellationToken)
    {
        var holding = ParseFund(row);

        await _holdingService.AddHolding(holding, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private static AddHoldingDTO ParseFund(FundCsvRow row)
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