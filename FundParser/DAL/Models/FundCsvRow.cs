using CsvHelper.Configuration.Attributes;

namespace FundParser.DAL.Models;

public class FundCsvRow
{
    [Name("date")]
    public string date { get; set; }

    [Name("fund")]
    public string fund { get; set; }

    [Name("company")]
    public string company { get; set; }

    [Name("ticker")]
    public string ticker { get; set; }

    [Name("cusip")]
    public string cusip { get; set; }

    [Name("shares")]
    public string shares { get; set; }

    [Name("market value ($)")]
    public string marketValue { get; set; }

    [Name("weight (%)")]
    public string weight { get; set; }
}