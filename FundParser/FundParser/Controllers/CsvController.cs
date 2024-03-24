using FundParser.BL.Services.FundCsvService;
using FundParser.BL.Services.HoldingDiffService;

using Microsoft.AspNetCore.Mvc;

namespace FundParser.App.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvController : ControllerBase
{
    private readonly IFundCsvService _fundCsvService;
    private readonly IHoldingDiffService _holdingDiffService;

    public CsvController(IFundCsvService fundCsvService, IHoldingDiffService holdingDiffService)
    {
        _fundCsvService = fundCsvService;
        _holdingDiffService = holdingDiffService;
    }

    [HttpPost(Name = "csv")]
    public async Task<IActionResult> CsvUpload(CancellationToken cancellationToken)
    {
        // temporary endpoint to update holdings for testing purposes
        var result = await _fundCsvService.UpdateHoldings(cancellationToken);

        return Ok(result);
    }
}