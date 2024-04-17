using System.ComponentModel.DataAnnotations;
namespace FundParser.DAL.Models
{
    public class Company : BaseEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Cusip { get; set; } = null!;

        public string Ticker { get; set; } = null!;

        public virtual ICollection<Holding> Holdings { get; set; } = null!;

        public virtual ICollection<HoldingDiff> HoldingDiffs { get; set; } = null!;
    }
}
