using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class HoldingDiff : BaseEntity
    {

        [ForeignKey("Holding")]
        public int HoldingId { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [Required]
        public decimal Shares { get; set; }

        [Required]
        public decimal SharesChange { get; set; }

        [Required]
        public decimal Weight { get; set; }

        public virtual Fund Fund { get; set; }
        public virtual Company Company { get; set; }
        public virtual Holding Holding { get; set; }
    }
}
