using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundParser.DAL.Models
{
    public class Holding : BaseEntity
    {

        [Required]
        public DateTime Date { get; set; }

        public int FundId { get; set; }

        public int CompanyId { get; set; }

        [Required]
        public decimal Shares { get; set; }

        [Required]
        public decimal MarketValue { get; set; }

        [Required]
        public decimal Weight { get; set; }

        [ForeignKey(nameof(FundId))]
        public virtual Fund Fund { get; set; } = null!;

        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; } = null!;
    }
}
