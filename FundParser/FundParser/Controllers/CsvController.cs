using FundParser.BL.Services.FundCsvService;

using Microsoft.AspNetCore.Mvc;

namespace FundParser.App.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvController : ControllerBase
{
    private readonly IFundCsvService _fundCsvService;

    public CsvController(IFundCsvService fundCsvService)
    {
        _fundCsvService = fundCsvService;
    }

    [HttpPost(Name = "csv")]
    public async Task<IActionResult> CsvUpload(CancellationToken cancellationToken)
    {
        // temporary endpoint to update holdings for testing purposes
        await _fundCsvService.UpdateHoldings(cancellationToken);

        return Ok();
    }
}