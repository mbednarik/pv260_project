using CsvHelper.Configuration.Attributes;

namespace FundParser.DAL.Models;

public class FundCsvRow
{
    [Name("date")]
    public string date { get; set; } = null!;

    [Name("fund")]
    public string fund { get; set; } = null!;

    [Name("company")]
    public string company { get; set; } = null!;

    [Name("ticker")]
    public string ticker { get; set; } = null!;

    [Name("cusip")]
    public string cusip { get; set; } = null!;

    [Name("shares")]
    public string shares { get; set; } = null!;

    [Name("market value ($)")]
    public string marketValue { get; set; } = null!;

    [Name("weight (%)")]
    public string weight { get; set; } = null!;
}