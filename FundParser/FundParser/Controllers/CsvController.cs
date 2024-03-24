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
        // await _fundCsvService.UpdateHoldings(cancellationToken);
        await _holdingDiffService
            .CalculateAndStoreHoldingDiffs(new DateTime(2024, 3, 15),
                new DateTime(2024, 3, 15));

        return Ok();
    }
}