namespace BL.DTOs;

public class AddHoldingDTO
{
    public DateTime Date { get; set; }
    public int FundId { get; set; }
    public int CompanyId { get; set; }
    public decimal Shares { get; set; }
    public decimal MarketValue { get; set; }
    public decimal Weight { get; set; }
}