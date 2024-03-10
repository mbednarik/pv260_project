using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Fund : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Holding> Holdings { get; set; }

        public virtual ICollection<HoldingDiff> HoldingDiffs { get; set; }
    }
}
