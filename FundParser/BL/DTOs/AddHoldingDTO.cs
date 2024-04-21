namespace FundParser.BL.DTOs;

public class AddHoldingDTO
{
    public DateTime Date { get; set; }

    public AddFundDTO Fund { get; set; }

    public AddCompanyDTO Company { get; set; }

    public decimal Shares { get; set; }

    public decimal MarketValue { get; set; }

    public decimal Weight { get; set; }
}