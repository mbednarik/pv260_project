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
        // temporary endpoint to update holdings for testing purposes
        var result = await _fundCsvService.UpdateHoldings(cancellationToken);
        await _logger.LogInformation("Csv imported from the website", nameof(CsvController), cancellationToken);
        return Ok(result);
    }
}