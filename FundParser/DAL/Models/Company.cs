using System.ComponentModel.DataAnnotations;
namespace FundParser.DAL.Models
{
    public class Company : BaseEntity
    {
        [Required]
        public string Name { get; set; } 

        [Required]
        public string Cusip { get; set; } 

        public string Ticker { get; set; } 

        public virtual ICollection<Holding> Holdings { get; set; } 

        public virtual ICollection<HoldingDiff> HoldingDiffs { get; set; } 
    }
}
