using System.Globalization;

using FundParser.BL.DTOs;
using FundParser.BL.Services.HoldingService;
using FundParser.DAL.Csv;
using FundParser.DAL.Logging;
using FundParser.DAL.Models;
using FundParser.DAL.UnitOfWork;

namespace FundParser.BL.Services.FundCsvService;

public class FundCsvService : IFundCsvService
{
    private readonly IHoldingService _holdingService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICsvDownloader<FundCsvRow> _csvDownloader;
    private readonly ILoggingService _logger;

    public FundCsvService(
        IHoldingService holdingService,
        ICsvDownloader<FundCsvRow> csvDownloader,
        IUnitOfWork unitOfWork,
        ILoggingService logger)
    {
        _holdingService = holdingService;
        _csvDownloader = csvDownloader;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<int> UpdateHoldings(CancellationToken cancellationToken = default)
    {
        // TODO: move this to config
        var url = "http://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
        var rows = await _csvDownloader.DownloadAndParse(url, cancellationToken);

        if (rows == null)
        {
            await _logger.LogError("Unable to download csv from the API", nameof(FundCsvService), cancellationToken);
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
                await _logger.LogError($"Unable to proccess csv row from the API {row}, thrown exception {e.Message}", nameof(FundCsvService));
            }
        }
        return successfulRows;
    }

    private async Task ProcessRow(FundCsvRow row, CancellationToken cancellationToken)
    {
        var holding = ParseFund(row);

        await _holdingService.AddHolding(holding, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken); // Maybe change to bulk insert ?
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