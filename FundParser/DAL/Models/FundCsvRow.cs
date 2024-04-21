using CsvHelper.Configuration.Attributes;

namespace FundParser.DAL.Models;

public class FundCsvRow
{
    [Name("date")]
    public string Date { get; set; } 

    [Name("fund")]
    public string Fund { get; set; } 

    [Name("company")]
    public string Company { get; set; } 

    [Name("ticker")]
    public string Ticker { get; set; } 

    [Name("cusip")]
    public string Cusip { get; set; } 

    [Name("shares")]
    public string Shares { get; set; } 

    [Name("market value ($)")]
    public string MarketValue { get; set; } 

    [Name("weight (%)")]
    public string Weight { get; set; } 

    public override string ToString()
    {
        return $"Date: {Date}, Fund: {Fund}, Company: {Company}, Ticker: {Ticker}," +
            $" Cusip: {Cusip}, Shares: {Shares}, MarketValue: {MarketValue}, Weight: {Weight}";
    }
}