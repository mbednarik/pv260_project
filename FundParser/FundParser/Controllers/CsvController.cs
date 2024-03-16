using BL.Services.CsvService;
using Microsoft.AspNetCore.Mvc;

namespace FundParser.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvController : ControllerBase
{
    private readonly ICsvService _csvService;

    public CsvController(ICsvService csvService)
    {
        _csvService = csvService;
    }

    [HttpPost(Name = "csv")]
    public async Task<IActionResult> CsvUpload()
    {
        // TODO: add url to request body
        var url = "http://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
        var resultRows = await _csvService.GetRowsFromUrl(url);

        if (resultRows == null)
        {
            return BadRequest();
        }

        var successfulInserts = 0;

        foreach (var row in resultRows)
        {
            var successfullyInserted = await _csvService.InsertRowIntoDb(row);
            if (successfullyInserted)
            {
                successfulInserts++;
            }
        }

        return Ok(new
        {
            InsertedRows = successfulInserts,
        });
    }
}