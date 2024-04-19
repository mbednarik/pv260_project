using System.Globalization;

using FundParser.BL.DTOs;
using FundParser.BL.Services.CsvParsingService;
using FundParser.BL.Services.DownloaderService;
using FundParser.BL.Services.HoldingService;
using FundParser.BL.Services.LoggingService;
using FundParser.DAL.Models;
using FundParser.DAL.UnitOfWork;

using Microsoft.Extensions.Configuration;

namespace FundParser.BL.Services.FundCsvService;

public class FundCsvService : IFundCsvService
{
    private const string CsvUrlSectionKey = "FundCsvUrl";

    private readonly IHoldingService _holdingService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ICsvParsingService<FundCsvRow> _csvParsingService;
    private readonly IDownloaderService _downloaderService;
    private readonly ILoggingService _logger;
    private readonly List<(string, string)> _csvRequestHeaders;

    public FundCsvService(
        IHoldingService holdingService,
        ICsvParsingService<FundCsvRow> csvParsingService,
        IDownloaderService downloaderService,
        IUnitOfWork unitOfWork,
        ILoggingService logger,
        IConfiguration configuration)
    {
        _holdingService = holdingService;
        _csvParsingService = csvParsingService;
        _downloaderService = downloaderService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _csvRequestHeaders = [("Accept", "text/csv"),
            ("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0")
        ];
    }

    public async Task<int> UpdateHoldings(CancellationToken cancellationToken = default)
    {
        var url = _configuration.GetRequiredSection(CsvUrlSectionKey).Value;
        if (url is null)
        {
            await _logger.LogError($"{CsvUrlSectionKey} is not set in the configuration",
                nameof(FundCsvService), cancellationToken);
            throw new Exception($"{CsvUrlSectionKey} is not set in the configuration");
        }

        var csvString = await _downloaderService.DownloadTextFileAsStringAsync(url, _csvRequestHeaders, cancellationToken)
            ?? throw new Exception("Failed to download csv");

        var csvRows = _csvParsingService.ParseString(csvString, cancellationToken)
            ?? throw new Exception("Failed to parse csv");

        var successfulRows = 0;
        foreach (var row in csvRows)
        {
            try
            {
                await ProcessRow(row, cancellationToken);
                successfulRows++;
            }
            catch (Exception e)
            {
                await _logger.LogError($"Unable to proccess csv row from the API {row}, thrown exception {e.Message}",
                    nameof(FundCsvService), cancellationToken);
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
            Name = row.Fund,
        };

        var company = new AddCompanyDTO
        {
            Cusip = row.Cusip,
            Ticker = row.Ticker,
            Name = row.Company,
        };

        var holding = new AddHoldingDTO
        {
            Fund = fund,
            Company = company,
            Shares = decimal.Parse(row.Shares),
            MarketValue = decimal.Parse(row.MarketValue.TrimStart('$')),
            Weight = decimal.Parse(row.Weight.TrimEnd('%')),
            Date = DateTime.ParseExact(row.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture),
        };

        return holding;
    }
}