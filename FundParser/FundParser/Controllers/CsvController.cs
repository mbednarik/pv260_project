using BL.Services.FundCsvService;
using BL.Services.HoldingDiffService;
using Microsoft.AspNetCore.Mvc;

namespace FundParser.Controllers;

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
    public async Task<IActionResult> CsvUpload()
    {
        // temporary endpoint to update holdings for testing purposes
        await _fundCsvService.UpdateHoldings();
        await _holdingDiffService
            .CalculateAndStoreHoldingDiffs(new DateTime(2024, 1, 1),
                new DateTime(2024, 2, 1));
        
        return Ok();
    }
}