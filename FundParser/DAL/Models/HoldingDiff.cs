using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class HoldingDiff : BaseEntity
    {
        public int? OldHoldingId { get; set; }

        public int? NewHoldingId { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [Required]
        public decimal OldShares { get; set; }

        [Required]
        public decimal SharesChange { get; set; }

        [Required]
        public decimal OldWeight { get; set; }

        [Required]
        public decimal WeightChange { get; set; }

        public virtual Fund Fund { get; set; }

        public virtual Company Company { get; set; }

        [ForeignKey("OldHoldingId")]
        public virtual Holding? OldHolding { get; set; }

        [ForeignKey("NewHoldingId")]
        public virtual Holding? NewHolding { get; set; }
    }
}