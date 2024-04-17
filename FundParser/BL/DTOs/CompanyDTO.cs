namespace FundParser.BL.DTOs;

public class CompanyDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Cusip { get; set; } = null!;

    public string Ticker { get; set; } = null!;
}