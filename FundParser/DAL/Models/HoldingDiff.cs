using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundParser.DAL.Models
{
    public class HoldingDiff : BaseEntity
    {
        public int OldHoldingId { get; set; }

        public int NewHoldingId { get; set; }

        public int FundId { get; set; }

        public int CompanyId { get; set; }

        [Required]
        public decimal OldShares { get; set; }

        [Required]
        public decimal SharesChange { get; set; }

        [Required]
        public decimal OldWeight { get; set; }

        [Required]
        public decimal WeightChange { get; set; }

        [ForeignKey(nameof(FundId))]
        public virtual Fund Fund { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }

        [ForeignKey(nameof(OldHoldingId))]
        public virtual Holding OldHolding { get; set; }

        [ForeignKey(nameof(NewHoldingId))]
        public virtual Holding NewHolding { get; set; }
    }
}