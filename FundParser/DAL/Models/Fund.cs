using System.ComponentModel.DataAnnotations;

namespace FundParser.DAL.Models
{
    public class Fund : BaseEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<Holding> Holdings { get; set; } = null!;

        public virtual ICollection<HoldingDiff> HoldingDiffs { get; set; } = null!;
    }
}
