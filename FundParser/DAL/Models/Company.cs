using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DAL.Models
{
    public class Company : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Cusip { get; set; }

        [Required]
        public string Ticker { get; set; }

        public virtual ICollection<Holding> Holdings { get; set; }

        public virtual ICollection<HoldingDiff> HoldingDiffs { get; set; }
    }
}
