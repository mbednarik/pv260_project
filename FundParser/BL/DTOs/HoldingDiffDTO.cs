namespace FundParser.BL.DTOs
{
    public class HoldingDiffDTO
    {
        public int Id { get; set; }

        public decimal OldShares { get; set; }

        public decimal SharesChange { get; set; }

        public decimal OldWeight { get; set; }

        public decimal WeightChange { get; set; }

        public string FundName { get; set; }

        public string CompanyName { get; set; }

        public string Ticker { get; set; }
    }
}