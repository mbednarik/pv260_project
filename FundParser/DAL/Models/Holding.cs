using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Holding : BaseEntity
    {

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("Fund")]
        public int FundId { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [Required]
        public decimal Shares { get; set; }

        [Required]
        public decimal MarketValue { get; set; }

        [Required]
        public decimal Weight { get; set; }

        public virtual Fund Fund { get; set; }

        public virtual Company Company { get; set; }
    }
}
