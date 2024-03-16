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
        // TODO: add url to request body
        var url = "http://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
        var resultRows = await _fundCsvService.GetRowsFromUrl(url);

        if (resultRows == null)
        {
            return BadRequest();
        }

        var successfulInserts = 0;

        foreach (var row in resultRows)
        {
            var successfullyInserted = await _fundCsvService.InsertRowIntoDb(row);
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