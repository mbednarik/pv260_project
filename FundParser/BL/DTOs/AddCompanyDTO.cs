namespace FundParser.BL.DTOs;

public class AddCompanyDTO
{
    public string Name { get; set; } = null!;

    public string Cusip { get; set; } = null!;

    public string Ticker { get; set; } = null!;
}