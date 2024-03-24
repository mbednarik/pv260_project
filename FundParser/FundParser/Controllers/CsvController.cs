using BL.Services.FundCsvService;

using Microsoft.AspNetCore.Mvc;

namespace FundParser.Controllers;

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
    public async Task<IActionResult> CsvUpload()
    {
        // temporary endpoint to update holdings for testing purposes
        await _fundCsvService.UpdateHoldings();

        return Ok();
    }
}