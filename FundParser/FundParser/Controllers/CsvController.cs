using FundParser.BL.Exceptions;
using FundParser.BL.Services.FundCsvService;
using FundParser.BL.Services.LoggingService;
using Microsoft.AspNetCore.Mvc;

namespace FundParser.App.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvController : ControllerBase
{
    private readonly IFundCsvService _fundCsvService;
    private readonly ILoggingService _logger;

    public CsvController(IFundCsvService fundCsvService,
        ILoggingService logger)
    {
        _fundCsvService = fundCsvService;
        _logger = logger;
    }

    [HttpPost(Name = "csv")]
    public async Task<IActionResult> CsvUpload(CancellationToken cancellationToken)
    {
        var result = await _fundCsvService.UpdateHoldings(cancellationToken);
        if (result == 0)
        {
            return Ok("No rows were updated!");
        }
        var message = $"Successfully imported {result} rows from API";
        await _logger.LogInformation(message, nameof(CsvController), cancellationToken);
        return Ok(message);
    }
}