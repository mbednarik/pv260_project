namespace FundParser.BL.DTOs
{
    public class HoldingDTO
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Shares { get; set; }

        public decimal MarketValue { get; set; }

        public decimal Weight { get; set; }
    }
}
