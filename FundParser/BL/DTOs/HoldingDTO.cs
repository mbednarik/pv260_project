using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs
{
    public class HoldingDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Shares { get; set; }
        public decimal MarketValue { get; set; }
        public decimal Weight { get; set; }
    }
}
